using CryStar.GameEvent.Data;
using CryStar.GameEvent.Enums;
using Cysharp.Threading.Tasks;

namespace CryStar.GameEvent.Execution
{
    /// <summary>
    /// GameEventHandlerが継承すべきインターフェース
    /// </summary>
    public interface IGameEventHandler
    {
        /// <summary>
        /// このハンドラが担当するゲームイベントの種類
        /// </summary>
        GameEventType SupportedGameEventType { get; }

        /// <summary>
        /// ゲームイベントを実行する
        /// </summary>
        UniTask HandleGameEvent(GameEventParameters parameters);
    }
}
