using CryStar.CommandBattle.Enums;

namespace CryStar.CommandBattle.Execution
{
    /// <summary>
    /// バトルシーンのMVP-Cパターンを管理するマネージャークラス
    /// </summary>
    public class BattleCoordinator : CoordinatorManagerBase
    {
        /// <summary>
        /// コーディネーターを切り替える
        /// </summary>
        public void TransitionToPhase(BattlePhaseType phaseType)
        {
            base.TransitionTo((int)phaseType);
        }
    }
}
