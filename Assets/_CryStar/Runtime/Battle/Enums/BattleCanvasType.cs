namespace CryStar.CommandBattle.Enums
{
    /// <summary>
    /// Battleの状態の列挙型
    /// </summary>
    public enum BattlePhaseType
    {
        Battle,
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
