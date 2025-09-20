namespace CryStar.GameEvent.Data
{
    /// <summary>
    /// ゲームイベントの開始時のイベントと終了時のイベントを持った最大のデータ
    /// </summary>
    public class GameEventSequenceData
    {
        private bool _playOnStart;
        private GameEventExecutionData _startEvent;
        private GameEventExecutionData _endEvent;
        
        /// <summary>
        /// ゲーム開始時に自動再生するか
        /// </summary>
        public bool PlayOnStart => _playOnStart;
        
        /// <summary>
        /// 開始時のイベント
        /// </summary>
        public GameEventExecutionData StartEvent => _startEvent;
        
        /// <summary>
        /// 終了時のイベント
        /// 終了時のイベントがない場合はnullが渡される
        /// </summary>
        public GameEventExecutionData EndEvent => _endEvent;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameEventSequenceData(bool playOnStart, GameEventExecutionData startEvent, GameEventExecutionData endEvent = null)
        {
            _playOnStart = playOnStart;
            _startEvent = startEvent;
            _endEvent = endEvent;
        }
    }
}
