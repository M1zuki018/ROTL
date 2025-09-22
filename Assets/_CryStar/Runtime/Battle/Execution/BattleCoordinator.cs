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

        /// <summary>
        /// 現在の画面の上に新しい画面をオーバーレイとして表示する
        /// </summary>
        public void PushCoordinator(BattlePhaseType phaseType)
        {
            base.PushCoordinator((int)phaseType);
        }
    }
}
