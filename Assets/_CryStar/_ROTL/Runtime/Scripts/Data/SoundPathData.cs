using System;
using UnityEngine;

namespace CryStar.PerProject
{
    /// <summary>
    /// サウンドの種類とAddressablePathのデータクラス
    /// </summary>
    [Serializable]
    public class SoundPathData
    {
        [SerializeField] 
        private SoundsPathType _pathType;
    
        [SerializeField] 
        private string _path;

        /// <summary>
        /// サウンドの種類
        /// </summary>
        public SoundsPathType PathType => _pathType;
    
        /// <summary>
        /// Path
        /// </summary>
        public string Path => _path;
    
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(_path);
        }
    }
}

