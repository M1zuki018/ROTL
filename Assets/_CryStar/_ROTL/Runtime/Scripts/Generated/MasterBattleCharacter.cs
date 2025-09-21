using System.Collections.Generic;
using UnityEngine;

public static class MasterBattleCharacter
{
    private static readonly Dictionary<int, string> _name = new Dictionary<int, string>()
    {
        { 1, "Stage" },
        { 2, "Flos" },
        { 3, "Speaker"}
    };
    
    private static readonly Dictionary<int, string> _iconPath = new Dictionary<int, string>()
    {
        { 1, "Assets/AssetStoreTools/Images/Battle/Character/Stage_I.png" },
        { 2, "Assets/AssetStoreTools/Images/Battle/Character/flos.png" },
        { 3, "Assets/AssetStoreTools/Images/Battle/Character/Speaker_I.png" }
    };

    private static readonly Dictionary<int, Color> _characterColor = new Dictionary<int, Color>()
    {
        { 1, new Color32(185, 0, 0, 255) },
        { 2, new Color32(193, 68, 205, 255)},
        { 3, new Color32(10, 86, 230, 255) },
    };
    
    public static string GetName(int id) => _name[id];
    public static string GetIconPath(int id) => _iconPath[id];
    public static Color GetCharacterColor(int id) => _characterColor[id];
}
