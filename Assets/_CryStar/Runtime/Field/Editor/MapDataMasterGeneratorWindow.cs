using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace CryStar.Field.Editor
{
    /// <summary>
    /// MapDataMasterの辞書を生成するウィンドウ
    /// </summary>
    public class SafeMapDataMasterGeneratorWindow : BaseMasterGeneratorWindow
    {
        private string _prefabOutputPath = "Assets/_iCON/Runtime/Prefabs/";

        [MenuItem("CryStar/Field/Safe Map Data Master Generator")]
        public static void ShowWindow()
        {
            GetWindow<SafeMapDataMasterGeneratorWindow>("Safe Map Data Master Generator");
        }

        protected override void UniqueGUI()
        {
            base.UniqueGUI();

            EditorGUILayout.Space();
            GUILayout.Label("Unique Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            _prefabOutputPath = EditorGUILayout.TextField("Prefab Output Path", _prefabOutputPath);
        }

        protected override string GetWindowTitle() => "Safe Map Data Master Generator";

        protected override void GenerateClass(IList<IList<object>> data)
        {
            var sb = new StringBuilder();

            sb.AppendLine("// ============================================================================");
            sb.AppendLine("// AUTO GENERATED - DO NOT MODIFY");
            sb.AppendLine($"// Generated at: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine("// ============================================================================");
            sb.AppendLine();
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using UnityEngine;");
            sb.AppendLine("#if UNITY_EDITOR");
            sb.AppendLine("using UnityEditor;");
            sb.AppendLine("#endif");
            sb.AppendLine("using CryStar.Field.Data;");
            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine("/// マップデータの定数クラス");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public static class {_className}");
            sb.AppendLine("{");

            // プレハブパスの定数定義
            sb.AppendLine("    // プレハブパス定数");
            var prefabPaths = new Dictionary<string, string>();
            
            foreach (var row in data)
            {
                if (row.Count < 3) continue;
                var name = row[1].ToString();
                var prefabPath = CreatePrefab(name);
                
                if (!string.IsNullOrEmpty(prefabPath))
                {
                    // Assets/_iCON/Runtime/Prefabs/ から相対パスを生成
                    var relativePath = prefabPath.Replace(_prefabOutputPath, "");
                    sb.AppendLine($"    private const string {name.ToUpper()}_PREFAB_PATH = \"{relativePath}\";");
                }
            }
            sb.AppendLine();
            
            // PREFAB_PATH定数を追加
            sb.AppendLine($"    private const string PREFAB_PATH = \"{_prefabOutputPath}\";");
            sb.AppendLine("    ");

            // プレハブ取得メソッド（参照を保持しない）
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// プレハブを安全に取得（都度読み込み）");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    private static GameObject LoadPrefabSafely(string path)");
            sb.AppendLine("    {");
            sb.AppendLine("        if (string.IsNullOrEmpty(path)) return null;");
            sb.AppendLine();
            sb.AppendLine("#if UNITY_EDITOR");
            sb.AppendLine("        var editorPath = $\"{PREFAB_PATH}{path}\";");
            sb.AppendLine("        return AssetDatabase.LoadAssetAtPath<GameObject>(editorPath);");
            sb.AppendLine("#else");
            sb.AppendLine("        // ランタイムでは Resources.Load を使用");
            sb.AppendLine("        // .prefab拡張子を除去してResourcesフォルダ相対パスにする");
            sb.AppendLine("        string resourcesPath = path.Replace(\".prefab\", \"\");");
            sb.AppendLine("        return Resources.Load<GameObject>(resourcesPath);");
            sb.AppendLine("#endif");
            sb.AppendLine("    }");
            sb.AppendLine();

            // 個別プレハブ取得プロパティ
            foreach (var kvp in prefabPaths)
            {
                var name = kvp.Key;
                sb.AppendLine($"    /// <summary>");
                sb.AppendLine($"    /// {name}のプレハブを取得");
                sb.AppendLine($"    /// </summary>");
                sb.AppendLine($"    public static GameObject {name}Prefab => LoadPrefabSafely({name.ToUpper()}_PREFAB_PATH);");
                sb.AppendLine();
            }

            // データ辞書の生成（プレハブは都度取得）
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// マップデータを動的に生成（プレハブは都度読み込み）");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    private static MapData CreateMapData(int id, string name, string displayName, string prefabPath)");
            sb.AppendLine("    {");
            sb.AppendLine("        return new MapData(id, name, displayName, LoadPrefabSafely(prefabPath));");
            sb.AppendLine("    }");
            sb.AppendLine();

            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// マップデータ定義");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    private static readonly Dictionary<int, (string name, string displayName, string prefabPath)> _mapDefinitions = new()");
            sb.AppendLine("    {");

            foreach (var row in data)
            {
                if (row.Count < 3) continue;

                var id = int.Parse(row[0].ToString());
                var name = row[1].ToString();
                var displayName = row[2].ToString();
                var prefabPath = prefabPaths.GetValueOrDefault(name, "");

                sb.AppendLine($"        {{ {id}, (\"{name}\", \"{displayName}\", \"{prefabPath}\") }},");
            }

            sb.AppendLine("    };");
            sb.AppendLine();

            // メソッド群
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// IDからマップデータを取得");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    public static MapData GetMapData(int id)");
            sb.AppendLine("    {");
            sb.AppendLine("        if (_mapDefinitions.TryGetValue(id, out var definition))");
            sb.AppendLine("        {");
            sb.AppendLine("            return CreateMapData(id, definition.name, definition.displayName, definition.prefabPath);");
            sb.AppendLine("        }");
            sb.AppendLine("        return null;");
            sb.AppendLine("    }");
            sb.AppendLine();

            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// 管理名からマップデータを取得");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    public static MapData GetMapDataByName(string name)");
            sb.AppendLine("    {");
            sb.AppendLine("        foreach (var kvp in _mapDefinitions)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (kvp.Value.name == name)");
            sb.AppendLine("            {");
            sb.AppendLine("                var def = kvp.Value;");
            sb.AppendLine("                return CreateMapData(kvp.Key, def.name, def.displayName, def.prefabPath);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("        return null;");
            sb.AppendLine("    }");
            sb.AppendLine();

            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// 表示名からマップデータを取得");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    public static MapData GetMapDataByDisplayName(string displayName)");
            sb.AppendLine("    {");
            sb.AppendLine("        foreach (var kvp in _mapDefinitions)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (kvp.Value.displayName == displayName)");
            sb.AppendLine("            {");
            sb.AppendLine("                var def = kvp.Value;");
            sb.AppendLine("                return CreateMapData(kvp.Key, def.name, def.displayName, def.prefabPath);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("        return null;");
            sb.AppendLine("    }");
            sb.AppendLine();

            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// IDからマップのプレハブを取得");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    public static GameObject GetMapPrefab(int id)");
            sb.AppendLine("    {");
            sb.AppendLine("        if (_mapDefinitions.TryGetValue(id, out var definition))");
            sb.AppendLine("        {");
            sb.AppendLine("            return LoadPrefabSafely(definition.prefabPath);");
            sb.AppendLine("        }");
            sb.AppendLine("        return null;");
            sb.AppendLine("    }");
            sb.AppendLine();

            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// 管理名からマップのプレハブを取得");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    public static GameObject GetMapPrefabByName(string name)");
            sb.AppendLine("    {");
            sb.AppendLine("        foreach (var kvp in _mapDefinitions)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (kvp.Value.name == name)");
            sb.AppendLine("            {");
            sb.AppendLine("                return LoadPrefabSafely(kvp.Value.prefabPath);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("        return null;");
            sb.AppendLine("    }");
            sb.AppendLine();

            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// 表示名からマップのプレハブを取得");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    public static GameObject GetMapPrefabByDisplayName(string displayName)");
            sb.AppendLine("    {");
            sb.AppendLine("        foreach (var kvp in _mapDefinitions)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (kvp.Value.displayName == displayName)");
            sb.AppendLine("            {");
            sb.AppendLine("                return LoadPrefabSafely(kvp.Value.prefabPath);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("        return null;");
            sb.AppendLine("    }");
            sb.AppendLine();

            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// 全マップのIDリストを取得");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    public static IEnumerable<int> GetAllMapIds()");
            sb.AppendLine("    {");
            sb.AppendLine("        return _mapDefinitions.Keys;");
            sb.AppendLine("    }");
            sb.AppendLine();

            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// 全マップデータを取得");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    public static IEnumerable<MapData> GetAllMapDatas()");
            sb.AppendLine("    {");
            sb.AppendLine("        foreach (var kvp in _mapDefinitions)");
            sb.AppendLine("        {");
            sb.AppendLine("            var def = kvp.Value;");
            sb.AppendLine("            yield return CreateMapData(kvp.Key, def.name, def.displayName, def.prefabPath);");
            sb.AppendLine("        }");
            sb.AppendLine("    }");

            sb.AppendLine("}");

            // ファイル出力
            SaveToFile(sb.ToString());
        }

        /// <summary>
        /// プレハブ生成メソッド
        /// </summary>
        private string CreatePrefab(string prefabName)
        {
            try
            {
                var normalizedPath = NormalizePath(_prefabOutputPath);
                CreateDirectoryIfNotExists(normalizedPath);

                string fullPath = Path.Combine(normalizedPath, $"Map_{prefabName}Prefab.prefab");

                if (File.Exists(fullPath))
                {
                    Debug.Log($"Prefab already exists: {fullPath}");
                    return fullPath;
                }

                GameObject emptyObject = new GameObject($"Map_{prefabName}Prefab");
                
                // TODO: 必要に合わせてプレハブの初期状態をここで設定する
                // SpriteRendererのオブジェクトをつけるなど。マップ生成Editorを作るタイミングで検討

                GameObject prefabAsset = PrefabUtility.SaveAsPrefabAsset(emptyObject, fullPath);
                DestroyImmediate(emptyObject);

                AssetDatabase.Refresh();

                if (prefabAsset != null)
                {
                    Debug.Log($"Prefab created successfully: {fullPath}");
                    return fullPath;
                }
                else
                {
                    Debug.LogError($"Failed to create prefab: {fullPath}");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error creating prefab '{prefabName}': {e.Message}");
                return null;
            }
        }
        
        private static string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return "Assets";

            path = path.Replace('\\', '/');

            if (!path.StartsWith("Assets/"))
            {
                if (path.StartsWith("/"))
                    path = "Assets" + path;
                else
                    path = "Assets/" + path;
            }

            return path;
        }

        private static void CreateDirectoryIfNotExists(string path)
        {
            if (!AssetDatabase.IsValidFolder(path))
            {
                string[] folders = path.Split('/');
                string currentPath = folders[0];

                for (int i = 1; i < folders.Length; i++)
                {
                    string newPath = currentPath + "/" + folders[i];
                    if (!AssetDatabase.IsValidFolder(newPath))
                    {
                        AssetDatabase.CreateFolder(currentPath, folders[i]);
                    }
                    currentPath = newPath;
                }
                
                AssetDatabase.Refresh();
            }
        }
    }
}