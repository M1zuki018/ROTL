using System;
using System.Collections.Generic;
using CryStar.Core;
using CryStar.Core.Enums;
using CryStar.Core.UserData;
using CryStar.Data.User;
using CryStar.GameEvent.Core;
using CryStar.GameEvent.Data;
using CryStar.GameEvent.Enums;
using CryStar.GameEvent.Execution;
using CryStar.GameEvent.Factory;
using CryStar.Utility;
using CryStar.Utility.Enum;
using Cysharp.Threading.Tasks;
using iCON.System;

namespace CryStar.GameEvent.Initialization
{
    /// <summary>
    /// Game Event Manager
    /// </summary>
    public class GameEventManager : CustomBehaviour
    {
        /// <summary>
        /// 各ゲームイベントの列挙型と処理を行うHandlerのインスタンスのkvp
        /// </summary>
        private Dictionary<GameEventType, GameEventHandlerBase> _handlers = new Dictionary<GameEventType, GameEventHandlerBase>();
        
        /// <summary>
        /// 現在実行中のイベント完了待機用
        /// </summary>
        private Dictionary<int, UniTaskCompletionSource> _eventCompletionSources = new Dictionary<int, UniTaskCompletionSource>();
        
        /// <summary>
        /// UserDataManager
        /// </summary>
        private UserDataManager _userDataManager;
        
        /// <summary>
        /// ストーリーユーザーデータ
        /// </summary>
        private StoryUserData StoryUserData => _userDataManager.CurrentUserData.StoryUserData;
        
        /// <summary>
        /// ゲームイベントユーザーデータ
        /// </summary>
        private GameEventUserData GameEventUserData => _userDataManager.CurrentUserData.GameEventUserData;
        
        /// <summary>
        /// Bind
        /// </summary>
        public override async UniTask OnBind()
        {
            await base.OnBind();
            
            // インゲーム・バトルシーンで利用するためGlobalサービスに登録
            ServiceLocator.Register(this, ServiceType.Global);
            
            // 念のためゲームイベントシステムが初期化されていることを確認する
            GameEventInitializer.Initialize();
            _handlers = GameEventFactory.CreateAllHandlers(ServiceLocator.GetLocal<InGameManager>());
            
            _userDataManager = ServiceLocator.GetGlobal<UserDataManager>();
            StoryUserData.OnStorySave += Check;
        }

        public override async UniTask OnStart()
        {
            await base.OnStart();

            var lastEventId = GameEventUserData.GetLastClearCount();
            
            // イベントIDが0以上が帰ってきていれば、イベントを実行する
            // NOTE: 全てクリア済みの場合は-1が返されるので、イベントは実行されない
            if (lastEventId > 0)
            {
                if (MasterGameEvent.IsEventOnStart(lastEventId))
                {
                    // 次のイベントが実行時に即座に開始するものの場合のみ、Eventを始める
                    await PlayEvent(lastEventId);   
                }
            }
        }
        
        private void OnDestroy()
        {
            StoryUserData.OnStorySave -= Check;
        }

        /// <summary>
        /// ストーリー読了時にトリガーすべきイベントがある場合の処理
        /// </summary>
        private void Check(int storyId)
        { 
            var data = MasterStoryTriggerEvent.GetTriggerEventData(storyId);
            if (data == null)
            {
                return;
            }
            
            PlayEvent(data.EventId).Forget();
        }

        /// <summary>
        /// イベントIDを元にイベントを実行する
        /// </summary>
        public async UniTask PlayEvent(int eventID)
        {
            var sequenceData = MasterGameEvent.GetGameEventSequenceData(eventID);
            
            // イベント開始時に登録されている処理を実行
            await Execute(sequenceData.StartEvent);
            
            // 終わったらイベント終了時に登録されている処理を実行
            await Execute(sequenceData.EndEvent);
            
            // セーブデータにクリア情報を記録
            GameEventUserData.AddData(eventID);
        }

        /// <summary>
        /// イベント実行
        /// </summary>
        private async UniTask Execute(GameEventExecutionData eventData)
        {
            if (eventData == null)
            {
                // イベントデータがnullの場合はreturn
                // NOTE: EndEventがない場合、正常な動作でもnullが渡されることがある
                return;
            }
            
            switch (eventData.ExecutionType)
            {
                // 順次実行
                case ExecutionType.Sequential:
                    await ExecuteSequential(eventData);
                    break;
                // 並行実行
                case ExecutionType.Parallel:
                    await ExecuteParallel(eventData);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// ゲームイベントを順次実行していく
        /// </summary>
        private async UniTask ExecuteSequential(GameEventExecutionData eventData)
        {
            try
            {
                foreach (var data in eventData.EventDataArray)
                {
                    var eventType = data.EventType;
                
                    if (!_handlers.TryGetValue(eventType, out var handler))
                    {
                        LogUtility.Warning($"未登録のイベントタイプです: {eventType}", LogCategory.System);
                        continue;
                    }
                    
                    // 各イベントを順次実行（前のイベントが完了してから次を実行）
                    await handler.HandleGameEvent(data.Parameters);
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error($"オーダー実行中にエラーが発生: {ex.Message}", LogCategory.System);
            }
        }

        /// <summary>
        /// ゲームイベントを並列実行
        /// </summary>
        private async UniTask ExecuteParallel(GameEventExecutionData eventData)
        {
            try
            {
                var tasks = new List<UniTask>();

                foreach (var data in eventData.EventDataArray)
                {
                    var eventType = data.EventType;

                    if (!_handlers.TryGetValue(eventType, out var handler))
                    {
                        LogUtility.Warning($"未登録のイベントタイプです: {eventType}", LogCategory.System);
                        continue;
                    }

                    // 各イベントのタスクをリストに追加（実行はまだしない）
                    tasks.Add(handler.HandleGameEvent(data.Parameters));
                }

                // 全てのタスクを並列実行し、全て完了するまで待機
                await UniTask.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                LogUtility.Error($"オーダー実行中にエラーが発生: {ex.Message}", LogCategory.System);
            }
        }
    }
}