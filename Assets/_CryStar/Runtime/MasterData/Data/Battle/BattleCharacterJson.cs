using System;
using System.Collections.Generic;

/// <summary>
/// MasterBattleCharacter
/// </summary>
[Serializable]
public class BattleCharacterJson
{
    public string version;
    public List<BattleCharacterInfo> characters;
}