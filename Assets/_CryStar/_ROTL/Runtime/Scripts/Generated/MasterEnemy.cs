using System.Collections.Generic;
using CryStar.PerProject;

public static class MasterEnemy
{
    private static readonly Dictionary<int, BattleTargetData> _battleTargets = new Dictionary<int, BattleTargetData>()
    {
        {
            // フロース
            2, new BattleTargetData(TargetType.Bind, RaceType.Human, AttackType.Technical, AttackType.Precision,
                "30", "___Longed to serve the king in blind faith, but I must be the one to rescue him",
                "Skirmisher Captain", "Lydiance Kingdom Knights Order")
        },
    };
    
    public static BattleTargetData GetBattleTarget(int battleId)
    {
        return _battleTargets[battleId];
    }
}
