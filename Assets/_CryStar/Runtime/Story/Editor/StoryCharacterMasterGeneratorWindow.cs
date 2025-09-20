using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryStar.Story.Enums;
using UnityEditor;
using UnityEngine;

/// <summary>
/// StoryCharacterMasterの辞書を生成するウィンドウ
/// </summary>
public class StoryCharacterMasterGeneratorWindow : BaseMasterGeneratorWindow
{
    private const int MIN_COLUMN = 8;
    
    [MenuItem("Tools/Story Character Master Generator")]
    public static void ShowWindow()
    {
        GetWindow<StoryCharacterMasterGeneratorWindow>("Story Character Master Generator");
    }
    
    protected override string GetWindowTitle() => "Story Character Master Generator";
    
    protected override void GenerateClass(IList<IList<object>> data)
    {
        var sb = new StringBuilder();
        
        // クラス定義の開始
        sb.AppendLine("// ============================================================================");
        sb.AppendLine("// AUTO GENERATED - DO NOT MODIFY");
        sb.AppendLine($"// Generated at: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine("// ============================================================================");
        sb.AppendLine();
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine("using CryStar.Story.Data;");
        sb.AppendLine("using CryStar.Story.Enums;");
        sb.AppendLine("using iCON.Story.Data;");
        sb.AppendLine();
        sb.AppendLine("/// <summary>");
        sb.AppendLine("/// ストーリーキャラクター情報の定数クラス");
        sb.AppendLine("/// </summary>");
        sb.AppendLine($"public static class {_className}");
        sb.AppendLine("{");
        
        // データ辞書の生成
        sb.AppendLine("    private static readonly Dictionary<int, BattleCharacterData> _characterData = new Dictionary<int, BattleCharacterData>");
        sb.AppendLine("    {");

        foreach (var row in data)
        {
            if (row.Count < 5) continue; // 最低限の列数チェック
            
            var id = int.Parse(row[0].ToString());
            var fullName = row[1].ToString();
            var displayName = row[2].ToString();
            var colorString = row[3].ToString();
            var textSpeed = float.Parse(row[4].ToString());
            
            // Color解析（#8B0000形式を想定）
            sb.AppendLine($"        {{");
            sb.AppendLine($"            {id}, new BattleCharacterData({id}, \"{fullName}\", \"{displayName}\", ");
            
            if (ColorUtility.TryParseHtmlString(colorString, out Color color))
            {
                sb.AppendLine($"                new Color({color.r:F3}f, {color.g:F3}f, {color.b:F3}f, {color.a:F3}f), {textSpeed:F2}f,");
            }
            else
            {
                sb.AppendLine($"                Color.white, {textSpeed:F2}f,");
            }
            
            sb.AppendLine("                    new CharacterBasePathData(");

            for (int i = 0; i < 3; i++)
            {
                var columnIndex = i + 5;
                string path = null;
    
                // 配列の境界チェック
                if (row.Count > columnIndex)
                {
                    path = row[columnIndex].ToString();
                }

                if (i != 2)
                {
                    // パスが空ならnull、そうでなければパスを書き込む
                    if (!string.IsNullOrEmpty(path))
                    {
                        sb.AppendLine($"                        \"{path}\",");
                    }
                    else
                    {
                        sb.AppendLine($"                        null,");
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(path))
                    {
                        sb.AppendLine($"                        \"{path}\"");
                    }
                    else
                    {
                        sb.AppendLine($"                        null");
                    }
                }
            }
            sb.AppendLine("                ),");
            
            sb.AppendLine($"                new Dictionary<FacialExpressionType, string>");
            sb.AppendLine($"                {{");
            
            // 表情パスの追加
            var expressionTypes = System.Enum.GetValues(typeof(FacialExpressionType))
                .Cast<FacialExpressionType>()
                .ToArray();
            
            for (int i = 0; i < expressionTypes.Length; i++)
            {
                var columnIndex = i + MIN_COLUMN;
                if (row.Count > columnIndex && !string.IsNullOrEmpty(row[columnIndex].ToString()))
                {
                    sb.AppendLine($"                    {{ FacialExpressionType.{expressionTypes[i]}, \"{row[columnIndex]}\" }},");
                }
            }
            
            sb.AppendLine($"                }})");
            sb.AppendLine($"        }},");
        }
        
        sb.AppendLine("    };");
        sb.AppendLine();
        
        // メソッド群の追加
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// IDからキャラクターデータを取得");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    public static BattleCharacterData GetCharacter(int id)");
        sb.AppendLine("    {");
        sb.AppendLine("        return _characterData.GetValueOrDefault(id, null);");
        sb.AppendLine("    }");
        sb.AppendLine();
        
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// フルネームからキャラクターデータを取得");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    public static BattleCharacterData GetCharacterByName(string fullName)");
        sb.AppendLine("    {");
        sb.AppendLine("        foreach (var kvp in _characterData)");
        sb.AppendLine("        {");
        sb.AppendLine("            if (kvp.Value.FullName == fullName)");
        sb.AppendLine("                return kvp.Value;");
        sb.AppendLine("        }");
        sb.AppendLine("        return null;");
        sb.AppendLine("    }");
        sb.AppendLine();
        
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// キャラクターの立ち絵ベース素材のパスを取得");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    public static CharacterBasePathData GetCharacterBasePath(int characterId)");
        sb.AppendLine("    {");
        sb.AppendLine("        var character = GetCharacter(characterId);");
        sb.AppendLine("        return character.BasePathData;");
        sb.AppendLine("    }");
        sb.AppendLine();
        
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// キャラクターの表情パスを取得");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    public static string GetExpressionPath(int characterId, FacialExpressionType expression)");
        sb.AppendLine("    {");
        sb.AppendLine("        var character = GetCharacter(characterId);");
        sb.AppendLine("        if (character.HasExpression(expression))");
        sb.AppendLine("        {");
        sb.AppendLine("            return character.GetExpressionPath(expression);");
        sb.AppendLine("        }");
        sb.AppendLine("        return null;");
        sb.AppendLine("    }");
        sb.AppendLine();
        
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// 全キャラクターのIDリストを取得");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    public static IEnumerable<int> GetAllCharacterIds()");
        sb.AppendLine("    {");
        sb.AppendLine("        return _characterData.Keys;");
        sb.AppendLine("    }");
        sb.AppendLine();
        
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// 全キャラクターデータを取得");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    public static IEnumerable<BattleCharacterData> GetAllCharacters()");
        sb.AppendLine("    {");
        sb.AppendLine("        return _characterData.Values;");
        sb.AppendLine("    }");
        
        sb.AppendLine("}");
        
        // ファイル出力
        SaveToFile(sb.ToString());
    }
}