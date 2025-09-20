using CryStar.GameEvent.Attributes;
using CryStar.GameEvent.Data;
using CryStar.GameEvent.Enums;
using Cysharp.Threading.Tasks;
using iCON.System;

namespace CryStar.GameEvent.Execution
{
    /// <summary>
    /// Objective - 目標表示
    /// </summary>
    [GameEventHandler(GameEventType.Objective)]
    public class ObjectiveGameEvent : GameEventHandlerBase
    {
        public override GameEventType SupportedGameEventType => GameEventType.Objective;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ObjectiveGameEvent(InGameManager inGameManager) : base(inGameManager) { }
        
        /// <summary>
        /// 目標UIを表示する
        /// </summary>
        public override async UniTask HandleGameEvent(GameEventParameters parameters)
        {
            await InGameManager.ShowObjective(parameters.StringParam);
        }
    }
}