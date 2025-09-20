using CryStar.GameEvent.Enums;
using CryStar.Story.Factory;

namespace CryStar.GameEvent.Attributes
{
    /// <summary>
    /// ゲームイベントハンドラを自動登録するための属性
    /// </summary>
    public class GameEventHandlerAttribute : System.Attribute, IHandlerAttribute<GameEventType>
    {
        /// <summary>
        /// ゲームイベントの種類
        /// </summary>
        public GameEventType HandlerType { get; }
        
        /// <summary>
        /// ハンドラーの優先度（低い値ほど優先される デフォルト: 0）
        /// </summary>
        public int Priority { get; set; } = 0;
        
        /// <summary>
        /// このハンドラーが有効かどうか（デフォルト: true）
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// ゲームイベントハンドラ属性の初期化
        /// </summary>
        public GameEventHandlerAttribute(GameEventType handlerType)
        {
            HandlerType = handlerType;
        }
    }
}
