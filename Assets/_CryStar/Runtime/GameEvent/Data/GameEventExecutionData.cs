using CryStar.GameEvent.Enums;

namespace CryStar.GameEvent.Data
{
    /// <summary>
    /// ゲームイベントの実行用データ
    /// </summary>
    public class GameEventExecutionData
    {
        #region Private Field

        private int _executionIndex = 0;
        
        private int _id;
        private ExecutionType _executionType;
        private GameEventData[] _eventDataArray;

        #endregion
        
        /// <summary>
        /// 現在実行中のゲームイベントのIndex
        /// </summary>
        public int ExecutionIndex => _executionIndex;
        
        /// <summary>
        /// ゲームイベントの実行タイプ
        /// </summary>
        public ExecutionType ExecutionType => _executionType;
        
        /// <summary>
        /// イベントデータの配列
        /// </summary>
        public GameEventData[] EventDataArray => _eventDataArray;
        
        /// <summary>
        /// 現在実行中のゲームイベントのイベントの種類
        /// </summary>
        public GameEventType EventType => _eventDataArray[ExecutionIndex].EventType;
        
        /// <summary>
        /// 現在実行中のゲームイベントのパラメーターデータクラス
        /// </summary>
        public GameEventParameters Parameters => _eventDataArray[ExecutionIndex].Parameters;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameEventExecutionData(ExecutionType executionType, GameEventData[] eventDataArray)
        {
            _executionType = executionType;
            _eventDataArray = eventDataArray;
        }
    }
}
