// ============================================================================
// AUTO GENERATED - DO NOT MODIFY
// Generated at: 2025-08-03 01:01:38
// ============================================================================

using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using CryStar.Field.Data;

/// <summary>
/// マップデータの定数クラス
/// </summary>
public static class MasterMapData
{
    // プレハブパス定数
    private const string OWNROOM_PREFAB_PATH = "Field/Map/Map_OwnRoomPrefab.prefab";
    private const string OFFICEAREA_PREFAB_PATH = "Field/Map/Map_OfficeAreaPrefab.prefab";
    private const string PARKAREA_PREFAB_PATH = "Field/Map/Map_ParkAreaPrefab.prefab";

    private const string PREFAB_PATH = "Assets/_iCON/Runtime/Prefabs/";
    
    /// <summary>
    /// プレハブを安全に取得（都度読み込み）
    /// </summary>
    private static GameObject LoadPrefabSafely(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        
#if UNITY_EDITOR
        var editorPath = $"{PREFAB_PATH}{path}";
        return AssetDatabase.LoadAssetAtPath<GameObject>(editorPath);
#else
        // ランタイムでは Resources.Load を使用
        // .prefab拡張子を除去してResourcesフォルダ相対パスにする
        string resourcesPath = path.Replace(".prefab", "");
        return Resources.Load<GameObject>(resourcesPath);
#endif
    }

    /// <summary>
    /// OwnRoomのプレハブを取得
    /// </summary>
    public static GameObject OwnRoomPrefab => LoadPrefabSafely(OWNROOM_PREFAB_PATH);

    /// <summary>
    /// OfficeAreaのプレハブを取得
    /// </summary>
    public static GameObject OfficeAreaPrefab => LoadPrefabSafely(OFFICEAREA_PREFAB_PATH);

    /// <summary>
    /// ParkAreaのプレハブを取得
    /// </summary>
    public static GameObject ParkAreaPrefab => LoadPrefabSafely(PARKAREA_PREFAB_PATH);

    /// <summary>
    /// マップデータを動的に生成（プレハブは都度読み込み）
    /// </summary>
    private static MapData CreateMapData(int id, string name, string displayName, string prefabPath)
    {
        return new MapData(id, name, displayName, LoadPrefabSafely(prefabPath));
    }

    /// <summary>
    /// マップデータ定義
    /// </summary>
    private static readonly Dictionary<int, (string name, string displayName, string prefabPath)> _mapDefinitions = new()
    {
        { 1, ("OwnRoom", "自室", "Field/Map/Map_OwnRoomPrefab.prefab") },
        { 2, ("OfficeArea", "事務所", "Field/Map/Map_OfficeAreaPrefab.prefab") },
        { 3, ("ParkArea", "園内", "Field/Map/Map_ParkAreaPrefab.prefab") },
    };

    /// <summary>
    /// IDからマップデータを取得
    /// </summary>
    public static MapData GetMapData(int id)
    {
        if (_mapDefinitions.TryGetValue(id, out var definition))
        {
            return CreateMapData(id, definition.name, definition.displayName, definition.prefabPath);
        }
        return null;
    }

    /// <summary>
    /// 管理名からマップデータを取得
    /// </summary>
    public static MapData GetMapDataByName(string name)
    {
        foreach (var kvp in _mapDefinitions)
        {
            if (kvp.Value.name == name)
            {
                var def = kvp.Value;
                return CreateMapData(kvp.Key, def.name, def.displayName, def.prefabPath);
            }
        }
        return null;
    }

    /// <summary>
    /// 表示名からマップデータを取得
    /// </summary>
    public static MapData GetMapDataByDisplayName(string displayName)
    {
        foreach (var kvp in _mapDefinitions)
        {
            if (kvp.Value.displayName == displayName)
            {
                var def = kvp.Value;
                return CreateMapData(kvp.Key, def.name, def.displayName, def.prefabPath);
            }
        }
        return null;
    }

    /// <summary>
    /// IDからマップのプレハブを取得
    /// </summary>
    public static GameObject GetMapPrefab(int id)
    {
        if (_mapDefinitions.TryGetValue(id, out var definition))
        {
            return LoadPrefabSafely(definition.prefabPath);
        }
        return null;
    }

    /// <summary>
    /// 管理名からマップのプレハブを取得
    /// </summary>
    public static GameObject GetMapPrefabByName(string name)
    {
        foreach (var kvp in _mapDefinitions)
        {
            if (kvp.Value.name == name)
            {
                return LoadPrefabSafely(kvp.Value.prefabPath);
            }
        }
        return null;
    }

    /// <summary>
    /// 表示名からマップのプレハブを取得
    /// </summary>
    public static GameObject GetMapPrefabByDisplayName(string displayName)
    {
        foreach (var kvp in _mapDefinitions)
        {
            if (kvp.Value.displayName == displayName)
            {
                return LoadPrefabSafely(kvp.Value.prefabPath);
            }
        }
        return null;
    }

    /// <summary>
    /// 全マップのIDリストを取得
    /// </summary>
    public static IEnumerable<int> GetAllMapIds()
    {
        return _mapDefinitions.Keys;
    }

    /// <summary>
    /// 全マップデータを取得
    /// </summary>
    public static IEnumerable<MapData> GetAllMapDatas()
    {
        foreach (var kvp in _mapDefinitions)
        {
            var def = kvp.Value;
            yield return CreateMapData(kvp.Key, def.name, def.displayName, def.prefabPath);
        }
    }
}
