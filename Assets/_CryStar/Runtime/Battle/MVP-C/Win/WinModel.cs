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
        public (string name, int experience) GetResultData()
        {
            TryGetBattleManager();
            return _battleManager.GetResultData();
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