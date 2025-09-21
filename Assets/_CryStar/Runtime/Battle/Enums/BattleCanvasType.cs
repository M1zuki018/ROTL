namespace CryStar.CommandBattle.Enums
{
    /// <summary>
    /// Battleの状態の列挙型
    /// </summary>
    public enum BattlePhaseType
    {
        Battle,
        TargetInfo,
        FirstSelect,
        Organization,
        TryEscape,
        CommandSelect,
        Execute,
        Win,
        Lose,
        Idea,
    }
}
