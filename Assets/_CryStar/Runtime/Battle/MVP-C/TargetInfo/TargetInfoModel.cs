using CryStar.CommandBattle.Enums;
using CryStar.CommandBattle.Execution;
using CryStar.Core;
using Cysharp.Threading.Tasks;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// TargetInfo_Model
    /// </summary>
    public class TargetInfoModel
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
        /// ターゲット情報を閉じてターン開始時の選択に映る
        /// </summary>
        public void StartBattle()
        {
            TryGetBattleManager();
            _battleManager.PlaySelectedSe(true).Forget();
            
            // 最初の行動選択へ移る
            _battleManager.CoordinatorManager.TransitionToPhase(BattlePhaseType.FirstSelect);
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