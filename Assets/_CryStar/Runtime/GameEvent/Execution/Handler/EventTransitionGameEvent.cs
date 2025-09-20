using CryStar.Core;
using CryStar.GameEvent.Attributes;
using CryStar.GameEvent.Data;
using CryStar.GameEvent.Enums;
using CryStar.GameEvent.Initialization;
using Cysharp.Threading.Tasks;
using iCON.System;

namespace CryStar.GameEvent.Execution
{
    /// <summary>
    /// EventTransition - 別のイベントを実行
    /// </summary>
    [GameEventHandler(GameEventType.EventTransition)]
    public class EventTransitionGameEvent : GameEventHandlerBase
    {
        /// <summary>
        /// Event Manager
        /// </summary>
        private GameEventManager _eventManager;
        
        public override GameEventType SupportedGameEventType => GameEventType.EventTransition;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EventTransitionGameEvent(InGameManager inGameManager) : base(inGameManager) { }
        
        /// <summary>
        /// 別のゲームイベントを実行する
        /// </summary>
        public override async UniTask HandleGameEvent(GameEventParameters parameters)
        {
            if (_eventManager == null)
            {
                // nullの場合はGlobalServiceから取得してくる
                _eventManager = ServiceLocator.GetGlobal<GameEventManager>();
            }
            
            await _eventManager.PlayEvent(parameters.IntParam);
        }
    }
}