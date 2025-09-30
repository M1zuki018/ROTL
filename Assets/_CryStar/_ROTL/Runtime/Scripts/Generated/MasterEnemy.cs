using System.Collections.Generic;
using CryStar.PerProject;

public static class MasterEnemy
{
    private static readonly Dictionary<int, BattleTargetData> _battleTargets = new Dictionary<int, BattleTargetData>()
    {
        {
            // フロース
            2, new BattleTargetData(TargetType.Bind, RaceType.Human, AttackType.Technical, AttackType.Precision,
                "30", "???", "Lydiance Kingdom Knights Order", "Skirmisher Captain")
        },
    };
    
    public static BattleTargetData GetBattleTarget(int battleId)
    {
        return _battleTargets[battleId];
    }
}
