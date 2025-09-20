using System.Collections.Generic;
using CryStar.GameEvent.Data;

public static class MasterStoryTriggerEvent
{
    private static Dictionary<int, StoryEndTriggerEventData> _triggerEventDataList = new Dictionary<int, StoryEndTriggerEventData>()
    {
        { 6, new StoryEndTriggerEventData(4, 0)}
    };

    /// <summary>
    /// 辞書の中にイベントをトリガーすべきものがないか検索する
    /// </summary>
    public static StoryEndTriggerEventData GetTriggerEventData(int storyId)
    {
        if (_triggerEventDataList.TryGetValue(storyId, out var data))
        {
            return data;
        }
        
        return null;
    } 
}
