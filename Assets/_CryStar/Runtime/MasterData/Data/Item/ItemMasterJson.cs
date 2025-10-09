using System;
using System.Collections.Generic;
using CryStar.Item.Data;

/// <summary>
/// MasterItem
/// </summary>
[Serializable]
public class ItemMasterJson
{
    public string version;
    public List<ItemData> items;
}