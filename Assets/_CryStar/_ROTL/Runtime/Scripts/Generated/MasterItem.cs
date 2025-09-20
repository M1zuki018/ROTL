// ============================================================================
// AUTO GENERATED - DO NOT MODIFY
// Generated at: 2025-08-31 14:30:25
// ============================================================================

using System.Collections.Generic;
using CryStar.Item.Data;
using CryStar.Item.Enums;

/// <summary>
/// アイテム情報の定数クラス
/// </summary>
public static class MasterItem
{
    private static readonly Dictionary<int, ItemData> _itemData = new Dictionary<int, ItemData>
    {
        {
            1, new ItemData(1, "ヒールポーション", "HPを50回復する薬草から作られた回復薬", 
                RarityType.Common, "Assets/AssetStoreTools/Images/iCON_Icon.png",
                99, 100, 10, 1, 1, true, true)
        },
        {
            2, new ItemData(2, "ハイヒールポーション", "HPを150回復する高級な回復薬", 
                RarityType.Rare, "Assets/AssetStoreTools/Images/iCON_Icon.png",
                50, 200, 50, 1, 1, true, true)
        },
        {
            3, new ItemData(3, "マナポーション", "MPを30回復する魔法の薬", 
                RarityType.Common, "Assets/AssetStoreTools/Images/iCON_Icon.png",
                99, 150, 15, 1, 2, true, true)
        },
        {
            4, new ItemData(4, "鉄の剣", "よく鍛えられた鉄製の剣。攻撃力+15", 
                RarityType.Common, "Assets/AssetStoreTools/Images/iCON_Icon.png",
                1, 300, 100, 2, 1, false, false)
        },
        {
            5, new ItemData(5, "銀の指輪", "魔法防御力を高める美しい銀の指輪", 
                RarityType.Uncommon, "Assets/AssetStoreTools/Images/iCON_Icon.png",
                1, 400, 200, 2, 3, false, false)
        },
        {
            6, new ItemData(6, "炎の石", "炎の魔法を強化する赤く輝く石", 
                RarityType.Rare, "Assets/AssetStoreTools/Images/iCON_Icon.png",
                10, 500, 300, 3, 1, false, true)
        },
        {
            7, new ItemData(7, "古い鍵", "何かを開くための古びた鍵", 
                RarityType.Common, "Assets/AssetStoreTools/Images/iCON_Icon.png",
                5, 600, 0, 4, 1, false, true)
        },
        {
            8, new ItemData(8, "竜の鱗", "伝説の竜から剥がれ落ちた貴重な鱗", 
                RarityType.Legendary, "Assets/AssetStoreTools/Images/iCON_Icon.png",
                3, 700, 1000, 3, 2, false, false)
        },
        {
            9, new ItemData(9, "パン", "焼きたての美味しいパン。HPを10回復", 
                RarityType.Common, "Assets/AssetStoreTools/Images/iCON_Icon.png",
                20, 50, 5, 1, 3, false, true)
        },
        {
            10, new ItemData(10, "魔法の杖", "魔力を増幅する神秘的な杖。魔法攻撃力+25", 
                RarityType.Epic, "Assets/AssetStoreTools/Images/iCON_Icon.png",
                1, 800, 500, 2, 2, false, false)
        }
    };

    /// <summary>
    /// IDからアイテムデータを取得
    /// </summary>
    public static ItemData GetItem(int id)
    {
        return _itemData.GetValueOrDefault(id, null);
    }

    /// <summary>
    /// アイテム名からアイテムデータを取得
    /// </summary>
    public static ItemData GetItemByName(string name)
    {
        foreach (var kvp in _itemData)
        {
            if (kvp.Value.Name == name)
                return kvp.Value;
        }
        return null;
    }

    /// <summary>
    /// カテゴリIDで絞り込んだアイテムリストを取得
    /// </summary>
    public static IEnumerable<ItemData> GetItemsByCategory(int categoryId)
    {
        foreach (var kvp in _itemData)
        {
            if (kvp.Value.CategoryId == categoryId)
                yield return kvp.Value;
        }
    }

    /// <summary>
    /// レアリティで絞り込んだアイテムリストを取得
    /// </summary>
    public static IEnumerable<ItemData> GetItemsByRarity(RarityType rarity)
    {
        foreach (var kvp in _itemData)
        {
            if (kvp.Value.Rarity == rarity)
                yield return kvp.Value;
        }
    }

    /// <summary>
    /// 戦闘中使用可能なアイテムリストを取得
    /// </summary>
    public static IEnumerable<ItemData> GetBattleUsableItems()
    {
        foreach (var kvp in _itemData)
        {
            if (kvp.Value.UseInBattle)
                yield return kvp.Value;
        }
    }

    /// <summary>
    /// フィールドで使用可能なアイテムリストを取得
    /// </summary>
    public static IEnumerable<ItemData> GetFieldUsableItems()
    {
        foreach (var kvp in _itemData)
        {
            if (kvp.Value.UseInField)
                yield return kvp.Value;
        }
    }

    /// <summary>
    /// 全アイテムのIDリストを取得
    /// </summary>
    public static IEnumerable<int> GetAllItemIds()
    {
        return _itemData.Keys;
    }

    /// <summary>
    /// 全アイテムデータを取得
    /// </summary>
    public static IEnumerable<ItemData> GetAllItems()
    {
        return _itemData.Values;
    }

    /// <summary>
    /// ソート順でソートされたアイテムリストを取得
    /// </summary>
    public static IEnumerable<ItemData> GetItemsSortedByOrder()
    {
        var items = new List<ItemData>(_itemData.Values);
        items.Sort((a, b) => a.SortOrder.CompareTo(b.SortOrder));
        return items;
    }

    /// <summary>
    /// 指定したアイテムの最大スタック数を取得
    /// </summary>
    public static int GetMaxStackCount(int itemId)
    {
        return _itemData[itemId].MaxStackCount;
    }
}