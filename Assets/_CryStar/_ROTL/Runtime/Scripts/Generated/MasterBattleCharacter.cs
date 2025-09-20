using System.Collections.Generic;

public static class MasterBattleCharacter
{
    private static readonly Dictionary<int, string> _name = new Dictionary<int, string>()
    {
        { 1, "ステージ" },
        { 2, "フロース" }
    };
    
    private static readonly Dictionary<int, string> _iconPath = new Dictionary<int, string>()
    {
        { 1, "Assets/AssetStoreTools/Images/Battle/Character/stage.png" },
        { 2, "Assets/AssetStoreTools/Images/Battle/Character/flos.png" }
    };
    
    public static string GetName(int id) => _name[id];
    public static string GetIconPath(int id) => _iconPath[id];
}
