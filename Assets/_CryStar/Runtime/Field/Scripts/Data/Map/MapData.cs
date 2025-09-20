using UnityEngine;

namespace CryStar.Field.Data
{
    /// <summary>
    /// マップデータ
    /// </summary>
    public class MapData
    {
        #region Private Fields

        private int _id;
        private string _name;
        private string _displayName;
        private GameObject _prefab;

        #endregion

        /// <summary>
        /// マップID
        /// </summary>
        public int Id => _id;
        
        /// <summary>
        /// 変数名
        /// </summary>
        public string Name => _name;
        
        /// <summary>
        /// 表示名
        /// </summary>
        public string DisplayName => _displayName;
        
        /// <summary>
        /// プレハブ
        /// </summary>
        public GameObject Prefab => _prefab;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MapData(int id, string name, string displayName, GameObject prefab)
        {
            _id = id;
            _name = name;
            _displayName = displayName;
            _prefab = prefab;
        }

        /// <summary>
        /// 文字列表現を取得
        /// </summary>
        public override string ToString()
        {
            return $"Map[{_id}] : {_displayName} ({_name})]";
        }
    }
}
