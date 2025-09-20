using System.Collections.Generic;
using CryStar.CommandBattle.Data;
using CryStar.CommandBattle.Enums;
using CryStar.CommandBattle.Execution;
using CryStar.Core;
using Cysharp.Threading.Tasks;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// Execute_Model
    /// </summary>
    public class ExecuteModel
    {
        /// <summary>
        /// BattleManager
        /// </summary>
        private BattleManager _battleManager;

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            _battleManager = ServiceLocator.GetLocal<BattleManager>();
        }

        /// <summary>
        /// 登録されているコマンドのリストを取得する
        /// </summary>
        /// <returns></returns>
        public List<BattleCommandEntryData> GetCommandList()
        {
            TryGetBattleManager();
            return _battleManager.CreateCommandList();
        }

        /// <summary>
        /// 演出を実行したあと実行メッセージを取得する
        /// </summary>
        public async UniTask<string> ExecuteCommandAndGetMessage(BattleCommandEntryData entry)
        {
            return await _battleManager.ExecuteCommandAsync(entry);
        }

        /// <summary>
        /// バトル結果に併せて次の処理を行う
        /// </summary>
        public async UniTask Next()
        {
            // バトル実行を待つ
            var result = await _battleManager.CheckBattleEndAsync();

            if (result.isFinish)
            {
                // バトルが終了している場合は勝敗に合わせてステートを変更
                _battleManager.CoordinatorManager.TransitionToPhase(result.isWin ? BattlePhaseType.Win : BattlePhaseType.Lose);
            }
            else
            {
                // バトルが終了していなければ最初の選択に戻る
                _battleManager.CoordinatorManager.TransitionToPhase(BattlePhaseType.FirstSelect);
            }
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
    }
}