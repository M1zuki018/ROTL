// ============================================================================
// AUTO GENERATED - DO NOT MODIFY
// Generated at: 2025-08-31 14:30:25
// ============================================================================

using System.Collections.Generic;
using System.Linq;
using CryStar.Item.Data;
using CryStar.Item.Enums;
using CryStar.MasterData;
using CryStar.Utility;
using UnityEngine;

/// <summary>
/// アイテム情報の定数クラス
/// </summary>
public class MasterItem : AddressableLabelMaster<int, ItemData>
{
    public override LoadPriority Priority => LoadPriority.Cached;
    protected override string Label => MasterDataAddresses.ITEM;
    
    protected override void LoadFromJson(string json)
    {
        var itemData = JsonUtility.FromJson<ItemMasterJson>(json);
        
        // Jsonから読み込むことができたデータで辞書を作成
        _data = new Dictionary<int, ItemData>(itemData.items.Count);
        foreach (var item in itemData.items)
        {
            _data[item.Id] = item;
        }
        
        LogUtility.Verbose($"[{typeof(MasterItem)}] Loaded {_data.Count} items");
    }
    
    /// <summary>
    /// IDからアイテムデータを取得
    /// </summary>
    public static ItemData GetItem(int id)
    {
        var master = MasterDataManager.Instance.Get<MasterItem>();
        return master._data.GetValueOrDefault(id, null);
    }

    /// <summary>
    /// アイテム名からアイテムデータを取得
    /// </summary>
    public static ItemData GetItemByName(string name)
    {
        var master = MasterDataManager.Instance.Get<MasterItem>();
        foreach (var kvp in master._data)
        {
            if (kvp.Value.Name == name)
                return kvp.Value;
        }
        return null;
    }

    /// <summary>
    /// カテゴリIDで絞り込んだアイテムリストを取得
    /// </summary>
    public static List<ItemData> GetItemsByCategory(int categoryId)
    {
        var master = MasterDataManager.Instance.Get<MasterItem>();
        
        // カテゴリIDが一致するものを抽出
        return master._data.Where(item => item.Value.CategoryId == categoryId)
            .Select(item => item.Value).ToList();
    }

    /// <summary>
    /// レアリティで絞り込んだアイテムリストを取得
    /// </summary>
    public static List<ItemData> GetItemsByRarity(RarityType rarity)
    {
        var master = MasterDataManager.Instance.Get<MasterItem>();
        return master._data.Where(item => item.Value.Rarity == rarity)
            .Select(item => item.Value).ToList();
    }

    /// <summary>
    /// 戦闘中使用可能なアイテムリストを取得
    /// </summary>
    public static List<ItemData> GetBattleUsableItems()
    {
        var master = MasterDataManager.Instance.Get<MasterItem>();
        return master._data.Where(item => item.Value.UseInBattle)
            .Select(item => item.Value).ToList();
    }

    /// <summary>
    /// フィールドで使用可能なアイテムリストを取得
    /// </summary>
    public static List<ItemData> GetFieldUsableItems()
    {
        var master = MasterDataManager.Instance.Get<MasterItem>();
        return master._data.Where(item => item.Value.UseInField)
            .Select(item => item.Value).ToList();
    }

    /// <summary>
    /// 全アイテムデータを取得
    /// </summary>
    public static List<ItemData> GetAllItems()
    {
        var master = MasterDataManager.Instance.Get<MasterItem>();
        return master._data.Values.ToList();
    }

    /// <summary>
    /// ソート順でソートされたアイテムリストを取得
    /// </summary>
    public static List<ItemData> GetItemsSortedByOrder()
    {
        var master = MasterDataManager.Instance.Get<MasterItem>();
        
        var items = new List<ItemData>(master._data.Values);
        items.Sort((a, b) => a.SortOrder.CompareTo(b.SortOrder));
        return items;
    }

    /// <summary>
    /// 指定したアイテムの最大スタック数を取得
    /// </summary>
    public static int GetMaxStackCount(int itemId)
    {
        var master = MasterDataManager.Instance.Get<MasterItem>();
        return master._data[itemId].MaxStackCount;
    }
}