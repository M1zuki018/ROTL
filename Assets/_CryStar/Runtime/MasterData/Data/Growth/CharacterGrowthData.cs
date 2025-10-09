using System;

/// <summary>
/// MasterGrowthCharacter
/// </summary>
[Serializable]
public class CharacterGrowthData
{
    public int characterId;
    public int[] hp;
    public int[] sp;
    public int[] attack;
    public int[] defense;
    public float[] skillMultiplier;
    public int[] statusResistance;
    public int[] speed;
    public int[] dodgeSpeed;
    public int[] armorPenetration;
    public int[] criticalRate;
    public int[] criticalDamage;
}