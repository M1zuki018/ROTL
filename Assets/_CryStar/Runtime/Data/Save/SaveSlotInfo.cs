using System;
using UnityEngine;

namespace CryStar.Data
{
    /// <summary>
    /// セーブスロット情報
    /// </summary>
    [Serializable]
    public class SaveSlotInfo
    {
        [SerializeField] private int _slotIndex;
        [SerializeField] private int _userId;
        [SerializeField] private long _lastSaveTime;
        [SerializeField] private bool _isCurrentSlot;
        
        /// <summary>
        /// セーブスロットのIndex
        /// </summary>
        public int SlotIndex => _slotIndex;
        
        /// <summary>
        /// ユーザーID
        /// </summary>
        public int UserId => _userId;
        
        /// <summary>
        /// 最後にセーブした時間
        /// </summary>
        public long LastSaveTime => _lastSaveTime;
        
        /// <summary>
        /// 現在選択中のスロットか
        /// </summary>
        public bool IsCurrentSlot => _isCurrentSlot;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SaveSlotInfo(int slotIndex, int userId, long lastSaveTime, bool isCurrentSlot = false)
        {
            _slotIndex = slotIndex;
            _userId = userId;
            _lastSaveTime = lastSaveTime;
            _isCurrentSlot = isCurrentSlot;
        }
        
        /// <summary>
        /// 最後の保存時間を日時形式で取得
        /// </summary>
        public DateTime GetLastSaveDateTime()
        {
            return DateTimeOffset.FromUnixTimeSeconds(LastSaveTime).DateTime;
        }

        /// <summary>
        /// 最後の保存時間を文字列で取得
        /// </summary>
        public string GetLastSaveTimeString()
        {
            return GetLastSaveDateTime().ToString("yyyy/MM/dd HH:mm:ss");
        }
    }
}