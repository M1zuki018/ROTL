using CryStar.Core.ReactiveExtensions;
using iCON.Enums;

/// <summary>
/// 依存性を低減させるためのGameManager用のインターフェース
/// </summary>
public interface IGameManager
{
    public ReadOnlyReactiveProperty<InGameStateType> CurrentGameStateProp { get; }
    // GameSettings Settings { get; }
    // bool IsFirstLoad { get; }
    // int VictoryPoints { get; }
    // BattleModeData GetGameModeData();
    // void SetGameMode(GameModeEnum mode);
    // void SetGameState(GameStateEnum gameState);
    // int SetVictoryPoints(int points);
}