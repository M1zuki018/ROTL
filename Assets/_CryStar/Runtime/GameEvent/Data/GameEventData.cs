using CryStar.GameEvent.Enums;

namespace CryStar.GameEvent.Data
{
    /// <summary>
    /// ゲームイベント実行に必要なゲームイベントデータの最小単位
    /// </summary>
    public class GameEventData
    {
        private GameEventType _eventType;
        private GameEventParameters _parameters;
        
        /// <summary>
        /// ゲームイベントの種類
        /// </summary>
        public GameEventType EventType => _eventType;
        
        /// <summary>
        /// パラメーター
        /// パラメーターが存在しない場合はnullが渡される
        /// </summary>
        public GameEventParameters Parameters => _parameters;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameEventData(GameEventType eventType, GameEventParameters parameters = null)
        {
            _eventType = eventType;
            _parameters = parameters;
        }
    }
}