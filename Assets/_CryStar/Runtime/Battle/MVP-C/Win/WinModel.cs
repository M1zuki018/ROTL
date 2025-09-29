using CryStar.CommandBattle.Execution;
using CryStar.Core;
using CryStar.Core.Enums;
using CryStar.Data.Scene;
using Cysharp.Threading.Tasks;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// Win_Model
    /// </summary>
    public class WinModel
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

        public void Exit()
        {
            TryGetBattleManager();
            _battleManager.ResetLogs();
        }

        /// <summary>
        /// BGMを停止する
        /// </summary>
        public void FinishBGM()
        {
            TryGetBattleManager();
            _battleManager.FinishBGM();
        }

        /// <summary>
        /// バトル結果のデータを取得する
        /// </summary>
        public async UniTask GetResultData()
        {
            TryGetBattleManager();
            
            // 結果を取得
            var resultData = _battleManager.GetResultData();
            
            // ログ表示
            _battleManager.SetLog($"{resultData.name}の勝利");

            await UniTask.Delay(100);
            
            _battleManager.SetLog($"経験値{resultData.experience}を手に入れた");
        }

        /// <summary>
        /// インゲームシーンにもどる
        /// </summary>
        public async UniTask TransitionToInGameScene()
        {
            await ServiceLocator.GetGlobal<SceneLoader>().LoadSceneAsync(new SceneTransitionData(SceneType.InGame, false, true));
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