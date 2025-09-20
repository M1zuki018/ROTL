using System.Collections.Generic;
using CryStar.GameEvent.Data;
using CryStar.GameEvent.Enums;

public static class MasterGameEvent
{
    private static readonly Dictionary<int, GameEventSequenceData> _eventData = new Dictionary<int, GameEventSequenceData>()
    {
        {
            1, new GameEventSequenceData(true,
                new GameEventExecutionData(ExecutionType.Sequential, 
                    new GameEventData[]
                    {
                        new(GameEventType.PlayStory, new GameEventParameters(intParam: 1)),
                    }),
                new GameEventExecutionData(ExecutionType.Sequential,
                    new GameEventData[]
                    {
                        new(GameEventType.EventTransition, new GameEventParameters(intParam: 2)),
                    }))
        },
        {
            2, new GameEventSequenceData(true,
                new GameEventExecutionData(ExecutionType.Sequential, 
                    new GameEventData[]
                    {
                        new(GameEventType.PlayStory, new GameEventParameters(intParam: 2)),
                    }),
                new GameEventExecutionData(ExecutionType.Sequential,
                    new GameEventData[]
                    {
                        new(GameEventType.EventTransition, new GameEventParameters(intParam: 3)),
                    }))
        },
        {
            3, new GameEventSequenceData(true,
                new GameEventExecutionData(ExecutionType.Parallel, 
                    new GameEventData[]
                    {
                        new(GameEventType.Objective, new GameEventParameters(stringParam: "衣装スタッフに声をかける")),
                        new(GameEventType.StoryPreload, new GameEventParameters(intArrayParam: new []{3,4,5})),
                    }))
        },
        {
            4, new GameEventSequenceData(false,
                new GameEventExecutionData(ExecutionType.Sequential,
                    new GameEventData[]
                    {
                        new(GameEventType.BattleStart),
                    }))
        },
        {
            5, new GameEventSequenceData(true,
                new GameEventExecutionData(ExecutionType.Sequential, 
                    new GameEventData[]
                    {
                        new(GameEventType.PlayStory, new GameEventParameters(intParam: 7)),
                    }),
                new GameEventExecutionData(ExecutionType.Sequential,
                    new GameEventData[]
                    {
                        new(GameEventType.GameClear),
                    }))
        }
    };
    
    /// <summary>
    /// ゲームイベント実行データを取得する
    /// </summary>
    public static GameEventSequenceData GetGameEventSequenceData(int eventId)
    {
        return _eventData[eventId];
    }

    /// <summary>
    /// 登録されているゲームイベントの個数を取得する
    /// </summary>
    public static int GetGameEventCount()
    {
        return _eventData.Count;
    }

    /// <summary>
    /// 引数で指定したイベントが実行時に自動的に始まるかチェックする
    /// </summary>
    public static bool IsEventOnStart(int eventId)
    {
        return _eventData[eventId].PlayOnStart;
    }
}
