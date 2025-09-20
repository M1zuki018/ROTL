using System;
using System.Threading;
using CryStar.CommandBattle.Enums;
using CryStar.CommandBattle.Execution;
using CryStar.Core;
using Cysharp.Threading.Tasks;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// TryEscape_Model
    /// </summary>
    public class TryEscapeModel : IDisposable
    {
        /// <summary>
        /// 乱数生成器（staticで再利用できるようにする）
        /// </summary>
        private static readonly Random _random = new Random();
        
        /// <summary>
        /// 逃走成功確率
        /// TODO: プレイヤーの逃走成功率の情報を読み取ったりして、適切な値を取得できるようにする
        /// </summary>
        private const int DEFAULT_ESCAPE_RATE = 3;
        
        /// <summary>
        /// 逃走失敗キャンバスを表示しておく時間（秒）
        /// </summary>
        private const float FAILURE_DISPLAY_INTERVAL = 3f;
        
        /// <summary>
        /// BattleManager
        /// </summary>
        private BattleManager _battleManager;
        
        /// <summary>
        /// CancellationTokenSource
        /// </summary>
        private CancellationTokenSource _cts;        

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            _battleManager = ServiceLocator.GetLocal<BattleManager>();
        }

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            Dispose();
        }

        /// <summary>
        /// 逃走できるか試す
        /// </summary>
        public async UniTask ExecuteEscapeAttemptAsync()
        {
            TryGetBattleManager();
            
            // 逃走判定を行う
            bool isEscapeSuccessful = RollEscapeAttempt();

            if (isEscapeSuccessful)
            {
                // TODO: 逃走が成功した場合の処理
            }
            else
            {
                // 逃走失敗時の処理を行う
                await HandleEscapeFailureAsync(_battleManager);
            }
        }
        
        /// <summary>
        /// 逃走失敗時の処理
        /// </summary>
        private async UniTask HandleEscapeFailureAsync(BattleManager manager)
        {
            // 念のためキャンセルトークンソースをクリーンアップしておく
            Dispose();
            _cts = new CancellationTokenSource();

            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(FAILURE_DISPLAY_INTERVAL), cancellationToken: _cts.Token);
            }
            catch (OperationCanceledException)
            {
                // 何かキーを押したら待機時間をスキップされるようにするので例外は特に出さない
            }
            finally
            {
                // 選択に戻る
                manager.CoordinatorManager.TransitionToPhase(BattlePhaseType.FirstSelect);
            }
        }
        
        /// <summary>
        /// 逃走判定を行う
        /// </summary>
        private bool RollEscapeAttempt()
        {
            // 1-101の範囲で乱数を生成し、逃走成功率と比較する
            int roll = _random.Next(1, 101);
            return roll < DEFAULT_ESCAPE_RATE;
        }
        
        /// <summary>
        /// バトルマネージャーが取得できているか確認し、取得できていなかったらServiceLocatorから取得する
        /// </summary>
        private void TryGetBattleManager()
        {
            if (_battleManager == null)
            {
                _battleManager = ServiceLocator.GetLocal<BattleManager>();
            }
        }
        
        /// <summary>
        /// キャンセルトークンソースのクリーンアップ
        /// </summary>
        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
    }
}