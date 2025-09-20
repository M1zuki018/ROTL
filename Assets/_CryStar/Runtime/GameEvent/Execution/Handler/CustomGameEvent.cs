using CryStar.GameEvent.Attributes;
using CryStar.GameEvent.Data;
using CryStar.GameEvent.Enums;
using Cysharp.Threading.Tasks;
using iCON.System;

namespace CryStar.GameEvent.Execution
{
    /// <summary>
    /// Custom - イベントハンドラーの説明
    /// </summary>
    [GameEventHandler(GameEventType.Custom)]
    public class CustomGameEvent : GameEventHandlerBase
    {
        public override GameEventType SupportedGameEventType => GameEventType.Custom;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CustomGameEvent(InGameManager inGameManager) : base(inGameManager) { }
        
        public override UniTask HandleGameEvent(GameEventParameters parameters)
        {
            return UniTask.CompletedTask;
        }
    }
}