using System;

namespace CryStar.Data.User
{
    /// <summary>
    /// ストーリーのユーザーデータ
    /// </summary>
    [Serializable]
    public class StoryUserData : CachedUserDataBase
    {
        /// <summary>
        /// ストーリーがセーブされたときのコールバック
        /// </summary>
        public event Action<int> OnStorySave;
        
        public StoryUserData(int userId) : base(userId) { }
        
        /// <summary>
        /// クリアしたか
        /// </summary>
        public override void AddData(int id)
        {
            if (!_dataCache.ContainsKey(id))
            {
                // 未クリアの場合、コールバックを呼び出す
                OnStorySave?.Invoke(id);
            }
            
            base.AddData(id);
        }
    }   
}