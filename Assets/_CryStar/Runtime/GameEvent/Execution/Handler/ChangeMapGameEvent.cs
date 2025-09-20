using CryStar.GameEvent.Attributes;
using CryStar.GameEvent.Data;
using CryStar.GameEvent.Enums;
using Cysharp.Threading.Tasks;
using iCON.System;

namespace CryStar.GameEvent.Execution
{
    /// <summary>
    /// ChangeMap - マップ変更
    /// </summary>
    [GameEventHandler(GameEventType.ChangeMap)]
    public class ChangeMapGameEvent : GameEventHandlerBase
    {
        public override GameEventType SupportedGameEventType => GameEventType.ChangeMap;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ChangeMapGameEvent(InGameManager inGameManager) : base(inGameManager) { }
        
        /// <summary>
        /// マップ変更を行う
        /// </summary>
        public override UniTask HandleGameEvent(GameEventParameters parameters)
        {
            InGameManager.RemoveAndShowMap(parameters.IntParam);
            return UniTask.CompletedTask;
        }
    }
}