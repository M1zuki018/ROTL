using System;

namespace CryStar.Data.User
{
    /// <summary>
    /// ユーザーデータの基底クラス
    /// </summary>
    public abstract class UserDataBase
    {
        protected int _userId;
        protected long _lastSaveTime;
    
        /// <summary>
        /// ユーザーID
        /// </summary>
        public int UserId => _userId;
    
        /// <summary>
        /// 最後にセーブした時間
        /// </summary>
        public long LastSaveTime => _lastSaveTime;
    
        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected UserDataBase(int userId)
        {
            _userId = userId;
            _lastSaveTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
        
        /// <summary>
        /// 最後にセーブした時間を更新する
        /// </summary>
        public virtual void UpdateSaveTime()
        {
            _lastSaveTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }   
}