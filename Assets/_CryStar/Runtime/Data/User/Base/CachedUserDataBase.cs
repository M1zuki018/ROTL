using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryStar.Data.User
{
    /// <summary>
    /// キャッシュ機能をもつユーザーデータのクラス
    /// </summary>
    [Serializable]
    public abstract class CachedUserDataBase : UserDataBase
    {
        [SerializeField] protected List<IdCountPairData> _dataList = new List<IdCountPairData>();

        protected Dictionary<int, int> _dataCache = new Dictionary<int, int>();
        
        /// <summary>
        /// セーブデータのリストのプロパティ（復元用）
        /// </summary>
        public List<IdCountPairData> DataList => _dataList;
        
        /// <summary>
        /// EventClearDataのキャッシュ
        /// </summary>
        public IReadOnlyDictionary<int, int> DataCache => _dataCache;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected CachedUserDataBase(int userId) : base(userId) { }
        
        /// <summary>
        /// データ復元用
        /// </summary>
        public void SetRestorationData(List<IdCountPairData> restorationData)
        {
            if (_dataList == null)
            {
                _dataList = new List<IdCountPairData>();
            }
            
            _dataList.Clear();
            _dataList = restorationData;
            
            // 実行時用のDictionaryを構築する
            BuildCache();
        }

        /// <summary>
        /// クリアデータを更新
        /// </summary>
        public virtual void AddData(int id)
        {
            if (_dataCache.ContainsKey(id))
            {
                _dataCache[id]++;
            }
            else
            {
                _dataCache[id] = 1;
            }
            
            UpdateSaveDataList(id);
        }
        
        /// <summary>
        /// 前提をクリアしているか
        /// </summary>
        public bool IsPremiseStoryClear(int id)
        {
            return _dataCache.ContainsKey(id);
        }
        
        /// <summary>
        /// セーブデータを更新する
        /// </summary>
        protected void UpdateSaveDataList(int id)
        {
            // シリアライズ用リストを更新
            var existingData = _dataList.Find(x => x.Id == id);
            if (existingData != null)
            {
                existingData.Count = _dataCache[id];
            }
            else
            {
                _dataList.Add(new IdCountPairData(id, 1));
            }
        }
        
        /// <summary>
        /// パフォーマンス向上のためのキャッシュを構築
        /// </summary>
        protected virtual void BuildCache()
        {
            _dataCache = new Dictionary<int, int>();
            if (_dataList != null)
            {
                foreach (var eventData in _dataList)
                {
                    _dataCache[eventData.Id] = eventData.Count;
                }
            }
        }
    }
}