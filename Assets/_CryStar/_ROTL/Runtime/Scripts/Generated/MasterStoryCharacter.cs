// ============================================================================
// AUTO GENERATED - DO NOT MODIFY
// Generated at: 2025-08-03 16:44:17
// ============================================================================

using System.Collections.Generic;
using UnityEngine;
using CryStar.Story.Data;
using CryStar.Story.Enums;
using iCON.Story.Data;

/// <summary>
/// ストーリーキャラクター情報の定数クラス
/// </summary>
public static class MasterStoryCharacter
{
    private static readonly Dictionary<int, CharacterData> _characterData = new Dictionary<int, CharacterData>
    {
        {
            1, new CharacterData(1, "六ノ花雪紀乃", "ロクノハナ ユキノ", 
                new Color(1.000f, 0.000f, 0.000f, 1.000f), 0.06f,
                    new CharacterBasePathData(
                        "Assets/AssetStoreTools/Images/Characters/Yukino/Yukino_hair.png",
                        "Assets/AssetStoreTools/Images/Characters/Yukino/Yukino_body.png",
                        "Assets/AssetStoreTools/Images/Characters/Yukino/Yukino_rearHair.png"
                ),
                new Dictionary<FacialExpressionType, string>
                {
                    { FacialExpressionType.Default, "Assets/AssetStoreTools/Images/Characters/Yukino/Yukino_face_default.png" },
                    { FacialExpressionType.Nervous, "Assets/AssetStoreTools/Images/Characters/Yukino/Yukino_face_nervous.png" },
                    { FacialExpressionType.Panicked, "Assets/AssetStoreTools/Images/Characters/Yukino/Yukino_face_panicked.png" },
                })
        },
        {
            2, new CharacterData(2, "蘇芳縢", "スオウ カガル", 
                new Color(0.208f, 0.110f, 0.459f, 1.000f), 0.06f,
                    new CharacterBasePathData(
                        "Assets/AssetStoreTools/Images/Characters/Kagaru/Kagaru_hair.png",
                        "Assets/AssetStoreTools/Images/Characters/Kagaru/Kagaru_body.png",
                        null
                ),
                new Dictionary<FacialExpressionType, string>
                {
                    { FacialExpressionType.Default, "Assets/AssetStoreTools/Images/Characters/Kagaru/Kagaru_face_default.png" },
                    { FacialExpressionType.Surprised, "Assets/AssetStoreTools/Images/Characters/Kagaru/Kagaru_face_surprised.png" },
                    { FacialExpressionType.Panicked, "Assets/AssetStoreTools/Images/Characters/Kagaru/Kagaru_face_panicked.png" },
                })
        },
        {
            3, new CharacterData(3, "鳥嶋 霖", "トリシマ リン", 
                new Color(0.110f, 0.271f, 0.529f, 1.000f), 0.07f,
                    new CharacterBasePathData(
                        null,
                        null,
                        null
                ),
                new Dictionary<FacialExpressionType, string>
                {
                })
        },
        {
            4, new CharacterData(4, "穂鷹 時優", "ホダカ シユウ", 
                new Color(0.651f, 0.302f, 0.475f, 1.000f), 0.05f,
                    new CharacterBasePathData(
                        null,
                        null,
                        null
                ),
                new Dictionary<FacialExpressionType, string>
                {
                })
        },
        {
            5, new CharacterData(5, "鳥嶋 俊也", "トリシマ シュンヤ", 
                new Color(0.271f, 0.506f, 0.557f, 1.000f), 0.06f,
                    new CharacterBasePathData(
                        null,
                        null,
                        null
                ),
                new Dictionary<FacialExpressionType, string>
                {
                })
        },
        {
            6, new CharacterData(6, "ヴェルデ母", "???", 
                new Color(1.000f, 0.427f, 0.004f, 1.000f), 0.07f,
                    new CharacterBasePathData(
                        null,
                        null,
                        null
                ),
                new Dictionary<FacialExpressionType, string>
                {
                })
        },
    };

    /// <summary>
    /// IDからキャラクターデータを取得
    /// </summary>
    public static CharacterData GetCharacter(int id)
    {
        return _characterData.GetValueOrDefault(id, null);
    }

    /// <summary>
    /// フルネームからキャラクターデータを取得
    /// </summary>
    public static CharacterData GetCharacterByName(string fullName)
    {
        foreach (var kvp in _characterData)
        {
            if (kvp.Value.FullName == fullName)
                return kvp.Value;
        }
        return null;
    }

    /// <summary>
    /// キャラクターの立ち絵ベース素材のパスを取得
    /// </summary>
    public static CharacterBasePathData GetCharacterBasePath(int characterId)
    {
        var character = GetCharacter(characterId);
        return character.BasePathData;
    }

    /// <summary>
    /// キャラクターの表情パスを取得
    /// </summary>
    public static string GetExpressionPath(int characterId, FacialExpressionType expression)
    {
        var character = GetCharacter(characterId);
        if (character.HasExpression(expression))
        {
            return character.GetExpressionPath(expression);
        }
        return null;
    }

    /// <summary>
    /// 全キャラクターのIDリストを取得
    /// </summary>
    public static IEnumerable<int> GetAllCharacterIds()
    {
        return _characterData.Keys;
    }

    /// <summary>
    /// 全キャラクターデータを取得
    /// </summary>
    public static IEnumerable<CharacterData> GetAllCharacters()
    {
        return _characterData.Values;
    }
}
