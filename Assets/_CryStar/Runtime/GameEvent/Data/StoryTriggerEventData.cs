namespace CryStar.GameEvent.Data
{
    /// <summary>
    /// ストーリー終了時にトリガーしたいイベントを処理するためのデータクラス
    /// </summary>
    public class StoryEndTriggerEventData
    {
        private int _eventId;
        private float _delayDuration;
        
        /// <summary>
        /// 呼び出したいイベントID
        /// </summary>
        public int EventId => _eventId;
        
        /// <summary>
        /// ストーリー終了後からイベント実行までの遅延秒数
        /// </summary>
        public float DelayDuration => _delayDuration;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StoryEndTriggerEventData(int eventId, float delayDuration)
        {
            _eventId = eventId;
            _delayDuration = delayDuration;
        }
    }
}
