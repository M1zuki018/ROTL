using CryStar.GameEvent.Attributes;
using CryStar.GameEvent.Data;
using CryStar.GameEvent.Enums;
using Cysharp.Threading.Tasks;
using iCON.System;

namespace CryStar.GameEvent.Execution
{
    /// <summary>
    /// StoryPreload - ストーリーデータの事前ロード
    /// </summary>
    [GameEventHandler(GameEventType.StoryPreload)]
    public class StoryPreloadGameEvent : GameEventHandlerBase
    {
        public override GameEventType SupportedGameEventType => GameEventType.StoryPreload;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StoryPreloadGameEvent(InGameManager inGameManager) : base(inGameManager) { }
        
        /// <summary>
        /// ストーリーデータの事前ロードを行う
        /// </summary>
        public override async UniTask HandleGameEvent(GameEventParameters parameters)
        {
            await InGameManager.PreloadStoryAsync(parameters.IntArrayParam);
        }
    }
}