using System;
using System.Collections.Generic;
using CryStar.Core;
using CryStar.Core.Enums;
using CryStar.Core.UserData;
using CryStar.Data.User;
using CryStar.Item.Data;
using CryStar.Utility;
using UnityEngine;

namespace CryStar.Item
{
    /// <summary>
    /// アイテムインベントリを管理するマネージャークラス
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {
        /// <summary>
        /// インベントリが更新されたときに呼ばれるコールバック
        /// </summary>
        public event Action OnInventoryChanged;
        
        /// <summary>
        /// UserDataManager
        /// </summary>
        private UserDataManager _userDataManager;
        
        /// <summary>
        /// インベントリユーザーデータ
        /// </summary>
        public InventoryUserData InventoryUserData => _userDataManager.CurrentUserData.InventoryUserData;
        
        private void Awake()
        {
            // GlobalServiceに既に自身が登録されているかチェックする
            if (ServiceLocator.IsRegisteredGlobal<InventoryManager>())
            {
                // 既に登録されていた場合は、登録されているInstanceが自分ではない場合、オブジェクトを削除する
                if (ServiceLocator.GetGlobal<InventoryManager>() != this)
                {
                    Destroy(gameObject);
                }
                
                // 初期化済みなので早期return
                return;
            }
            
            // 未登録の場合、GlobalServiceとDDOLに登録
            ServiceLocator.Register(this, ServiceType.Global);
            DontDestroyOnLoad(this);
            
            _userDataManager = ServiceLocator.GetGlobal<UserDataManager>();
            InventoryUserData.OnInventoryChanged += OnInventoryChanged;
        }

        private void OnDestroy()
        {
            if (InventoryUserData != null)
            {
                InventoryUserData.OnInventoryChanged -= OnInventoryChanged;
            }
        }

        /// <summary>
        /// アイテムを入手
        /// </summary>
        public bool GetItem(int itemId, int count)
        {
            // 最大スタック数
            var maxCount = MasterItem.GetMaxStackCount(itemId);
            
            // 現在の所持数と入手数の合計
            var sum = InventoryUserData.GetCount(itemId) + count;

            // ユーザーデータに記録
            InventoryUserData.GetItem(itemId, Mathf.Min(sum, maxCount));
            
            // 余剰分が出ていないかboolで結果を返す
            return sum <= maxCount;
        }

        public List<ItemData> GetAllItems()
        {
            var itemDataList = new List<ItemData>();
            foreach (var itemId in InventoryUserData.GetAllItemIds())
            {
                itemDataList.Add(MasterItem.GetItem(itemId));
            }

            return itemDataList;
        }

        /// <summary>
        /// アイテムを消費する
        /// </summary>
        public bool TryUseItem(int itemId, int count)
        {
            var itemData = MasterItem.GetItem(itemId);
            if (itemData == null)
            {
                LogUtility.Warning($"アイテムが取得できませんでした。ID: {itemId}");
                return false;
            }
            
            // 所持数を確認する
            var possessions = InventoryUserData.GetCount(itemId);
            if (possessions - count < 0)
            {
                // 所持数がマイナスになってしまう場合は個数が足りないとして、falseを返してリターン
                return false;
            }

            // TODO: マスターデータを元に、フィールドで使えるか、バトルで使えるかを確認する処理を入れる
            
            return true;
        }

        /// <summary>
        /// 現在のアイテムの所持数を入手する
        /// </summary>
        public IReadOnlyDictionary<int, int> GetItemCounts()
        {
            return InventoryUserData.DataCache;
        }
    }
}
