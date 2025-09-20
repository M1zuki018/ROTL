namespace CryStar.GameEvent.Data
{
    /// <summary>
    /// GameEventのパラメーター用のデータクラス
    /// </summary>
    public class GameEventParameters
    {
        private int _intParam;
        private string _stringParam;
        private int[] _intArrayParam;
        
        /// <summary>
        /// Int型パラメーター
        /// </summary>
        public int IntParam => _intParam;
        
        /// <summary>
        /// String型パラメーター
        /// </summary>
        public string StringParam => _stringParam;
        
        /// <summary>
        /// Int配列型パラメーター
        /// </summary>
        public int[] IntArrayParam => _intArrayParam;

        public GameEventParameters(int intParam = -1, string stringParam = null, int[] intArrayParam = null)
        {
            _intParam = intParam;
            _stringParam = stringParam;
            _intArrayParam = intArrayParam;
        }
    }
}
