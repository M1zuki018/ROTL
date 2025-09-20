using System.Collections.Generic;

public static class MasterCharacter
{
    private static readonly Dictionary<int, int[]> _hp = new Dictionary<int, int[]>()
    {
        {
            1, new int[]
            {
                200, 250, 260, 270, 280, 290, 300, 310, 320, 330, 340, 350, 360, 370, 380, 390,
                400, 410, 420, 430, 440, 460, 480, 500, 520, 540, 560, 580, 600, 620, 640, 660,
                680, 700, 720, 740, 760, 780, 800, 820, 840, 870, 900, 930, 960, 990, 1020, 1050,
                1080, 1110, 1140, 1170, 1200, 1230, 1260, 1290, 1320, 1350, 1380, 1410, 1440, 1475,
                1510, 1545, 1580, 1615, 1650, 1685, 1720, 1755, 1790, 1825, 1860, 1895, 1930, 1965,
                2000, 2035, 2070, 2105, 2140, 2180, 2220, 2260, 2300, 2340, 2380, 2420, 2460, 2500,
                2540, 2580, 2620, 2660, 2700, 2740, 2780, 2820, 2860, 2900
            }
        },
        {
            2, new int[] { 200, 250 }
        },
    };

    private static readonly Dictionary<int, int[]> _sp = new Dictionary<int, int[]>()
    {
        {
            1, new int[]
            {
                0, 60, 60, 60, 60, 60, 70, 70, 70, 70, 70, 80, 80, 80, 80, 80,
                90, 90, 90, 90, 90, 110, 115, 120, 125, 130, 135, 140, 145, 150, 155, 160,
                165, 170, 175, 180, 185, 190, 195, 200, 205, 210, 215, 220, 225, 230, 235, 240,
                245, 250, 255, 260, 265, 270, 275, 280, 285, 290, 295, 300, 305, 310, 315,
                320, 325, 330, 335, 340, 345, 350, 355, 360, 365, 370, 375, 380, 385, 390,
                395, 400, 405, 410, 415, 420, 425, 430, 435, 440, 445, 450, 455, 460, 465,
                470, 475, 480, 485, 490, 495, 500
            }
        },
        {
            2, new int[] { 0, 30 }
        },
    };

    private static readonly Dictionary<int, int[]> _attack = new Dictionary<int, int[]>()
    {
        {
            1, new int[]
            {
                0, 10, 13, 16, 19, 22, 25, 28, 31, 34, 37, 42, 47, 52, 57, 62,
                67, 72, 77, 82, 87, 92, 97, 102, 107, 112, 117, 122, 127, 132, 137, 142,
                147, 152, 157, 162, 167, 172, 177, 182, 187, 192, 197, 202, 207, 212, 217, 222,
                227, 232, 237, 244, 251, 258, 265, 272, 279, 286, 293, 300, 307, 314, 321,
                328, 335, 342, 349, 356, 363, 370, 377, 384, 391, 398, 405, 412, 419, 426,
                433, 440, 447, 454, 461, 468, 475, 482, 489, 496, 503, 510, 517, 524, 531,
                538, 545, 552, 559, 566, 573, 580
            }
        },
        {
            2, new int[] { 0, 5 }
        },
    };

    private static readonly Dictionary<int, int[]> _defense = new Dictionary<int, int[]>()
    {
        {
            1, new int[]
            {
                0, 5, 5, 5, 5, 5, 5, 8, 11, 14, 17, 20, 23, 26, 29, 32,
                35, 38, 41, 44, 47, 50, 53, 56, 59, 62, 65, 70, 75, 80, 85, 90,
                95, 100, 105, 110, 115, 120, 125, 130, 135, 140, 145, 150, 155, 160, 165, 170,
                175, 180, 185, 190, 195, 200, 205, 210, 215, 222, 229, 236, 243, 250, 257,
                264, 271, 278, 285, 292, 299, 306, 313, 320, 327, 334, 341, 348, 355, 362,
                369, 376, 383, 390, 397, 404, 411, 418, 425, 432, 439, 446, 453, 460, 467,
                474, 481, 488
            }
        },
        {
            2, new int[] { 0, 3 }
        },
    };
    
    private static readonly Dictionary<int, float[]> _skillMultiplier = new Dictionary<int, float[]>()
    {
        {
            1, CreateConstantFloatArray(1.0f, 100)
        },
        {
            2, CreateConstantFloatArray(1.0f, 2)
        },
    };

    private static readonly Dictionary<int, int[]> _statusResistances = new Dictionary<int, int[]>()
    {
        {
            1, CreateConstantIntArray(5, 100)
        },
        {
            2, CreateConstantIntArray(5, 2)
        },
    };
    
    private static readonly Dictionary<int, int[]> _speed = new Dictionary<int, int[]>()
    {
        {
            1, CreateConstantIntArray(30, 100)
        },
        {
            2, CreateConstantIntArray(30, 2)
        },
    };

    private static readonly Dictionary<int, int[]> _dodgeSpeed = new Dictionary<int, int[]>()
    {
        {
            1, CreateConstantIntArray(5, 100)
        },
        {
            2, CreateConstantIntArray(5, 2)
        },
    };

    private static readonly Dictionary<int, int[]> _armorPenetration = new Dictionary<int, int[]>()
    {
        {
            1, CreateConstantIntArray(5, 100)
        },
        {
            2, CreateConstantIntArray(5, 2)
        },
    };

    private static readonly Dictionary<int, int[]> _criticalRate = new Dictionary<int, int[]>()
    {
        {
            1, CreateConstantIntArray(5, 100)
        },
        {
            2, CreateConstantIntArray(5, 2)
        },
    };

    private static readonly Dictionary<int, int[]> _criticalDamage = new Dictionary<int, int[]>()
    {
        {
            1, CreateConstantIntArray(100, 100)
        },
        {
            2, CreateConstantIntArray(100, 2)
        },
    };
    

    public static int GetHp(int characterId, int level) => _hp[characterId][level];
    public static int GetSp(int characterId, int level) => _sp[characterId][level];
    public static int GetAttack(int characterId, int level) => _attack[characterId][level];
    public static int GetDefense(int characterId, int level) => _defense[characterId][level];
    public static float GetSkillMultiplier(int characterId, int level) => _skillMultiplier[characterId][level];
    public static int GetStatusResistance(int characterId, int level) => _statusResistances[characterId][level];
    public static int GetSpeed(int characterId, int level) => _speed[characterId][level];
    public static int GetDodgeSpeed(int characterId, int level) => _dodgeSpeed[characterId][level];
    public static int GetArmorPenetration(int characterId, int level) => _armorPenetration[characterId][level];
    public static int GetCriticalRate(int characterId, int level) => _criticalRate[characterId][level];
    public static int GetCriticalDamage(int characterId, int level) => _criticalDamage[characterId][level];
    
    /// <summary>
    /// 登録されているキャラクター数を取得する
    /// </summary>
    public static int RegisteredCharacterCount => _hp.Count;
    
    #region ヘルパーメソッド

    private static int[] CreateConstantIntArray(int value, int length)
    {
        int[] array = new int[length];
        for (int i = 0; i < length; i++)
            array[i] = value;
        return array;
    }

    private static float[] CreateConstantFloatArray(float value, int length)
    {
        float[] array = new float[length];
        for (int i = 0; i < length; i++)
            array[i] = value;
        return array;
    }

    #endregion
}
