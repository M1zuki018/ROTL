using CryStar.GameEvent.Data;
using CryStar.GameEvent.Enums;
using CryStar.Story.Factory;
using Cysharp.Threading.Tasks;
using iCON.System;

namespace CryStar.GameEvent.Execution
{
    /// <summary>
    /// ゲームイベントを処理するための基底クラス
    /// NOTE: このクラスを継承することで、新しい種類のイベントを処理するハンドラを定義できます
    /// </summary>
    public abstract class GameEventHandlerBase : IHandlerBase, IGameEventHandler
    {
        /// <summary>
        /// このハンドラが担当するゲームイベントの種類
        /// </summary>
        public abstract GameEventType SupportedGameEventType { get; }
        protected InGameManager InGameManager { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameEventHandlerBase(InGameManager inGameManager)
        {
            InGameManager = inGameManager;
        }

        /// <summary>
        /// ゲームイベントを実行する
        /// NOTE: 継承クラスで具体的な処理を実装する必要があります
        /// </summary>
        public abstract UniTask HandleGameEvent(GameEventParameters parameters);
    }
}
