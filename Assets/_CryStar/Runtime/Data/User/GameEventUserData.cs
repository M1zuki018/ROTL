using System;

namespace CryStar.Data.User
{
    /// <summary>
    /// ゲームイベントのセーブデータ用クラス
    /// </summary>
    [Serializable]
    public class GameEventUserData : CachedUserDataBase
    {
        public GameEventUserData(int userId) : base(userId) { }

        public int GetLastClearCount()
        {
            // まだ一つもクリアしていない場合は1を返す
            if (DataCache.Count == 0)
            {
                return 1;
            }
        
            // 1から順番に未クリアのイベントを探す
            for (int eventId = 1; eventId < MasterGameEvent.GetGameEventCount() + 1; eventId++)
            {
                if (!DataCache.ContainsKey(eventId))
                {
                    return eventId;
                }
            }
        
            // 全てのイベントをクリアしている場合は-1を返す
            return -1;
        }
    }
}
