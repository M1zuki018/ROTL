namespace CryStar.CommandBattle.Enums
{
    /// <summary>
    /// Battleの状態の列挙型
    /// </summary>
    public enum BattlePhaseType
    {
        Battle,
        FirstSelect,
        TryEscape,
        CommandSelect,
        Execute,
        Win,
        Lose,
        Idea,
    }
}
