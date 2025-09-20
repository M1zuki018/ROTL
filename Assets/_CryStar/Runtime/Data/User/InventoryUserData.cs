using System;
using System.Collections.Generic;

namespace CryStar.Data.User
{
    /// <summary>
    /// アイテムインベントリのユーザーデータ
    /// </summary>
    [Serializable]
    public class InventoryUserData : CachedUserDataBase
    {
        /// <summary>
        /// データが更新されたときに呼ばれるコールバック
        /// </summary>
        public event Action OnInventoryChanged;
        
        public InventoryUserData(int userId) : base(userId) { }

        /// <summary>
        /// 所持数を取得する
        /// </summary>
        public int GetCount(int itemId)
        {
            if (_dataCache.ContainsKey(itemId))
            {
                return _dataCache[itemId];
            }
            
            // 未所持の場合は0を返却
            return 0;
        }
        
        /// <summary>
        /// アイテムを入手
        /// </summary>
        public void GetItem(int itemId, int count)
        {
            _dataCache[itemId] = count;
            OnInventoryChanged?.Invoke();
        }

        /// <summary>
        /// 所持しているアイテムIDの配列を作成
        /// </summary>
        public List<int> GetAllItemIds()
        {
            var idArray = new List<int>();
            foreach (var key in _dataCache.Keys)
            {
                idArray.Add(key);
            }
            
            return idArray;
        }
    }
}
