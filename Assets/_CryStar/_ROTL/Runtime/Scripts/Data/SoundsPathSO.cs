using System.Collections.Generic;
using UnityEngine;

namespace CryStar.PerProject
{
    /// <summary>
    /// サウンド素材の入れ替えを楽にするための、Addressableのパス指定サポートクラス
    /// </summary>
    [CreateAssetMenu(fileName = "SoundsPathSO", menuName = "ScriptableObjects/SoundsPathSO")]
    public class SoundsPathSO : ScriptableObject
    {
        [SerializeField] private List<SoundPathData> _soundPaths;

        // ランタイム用のキャッシュ
        private Dictionary<SoundsPathType, string> _pathCache;

        /// <summary>
        /// 指定されたサウンドのAddressablePathを取得する
        /// </summary>
        public string GetPath(SoundsPathType pathType)
        {
            if (_pathCache == null)
            {
                //キャッシュが作られていなかったら作成
                BuildPathCache();
            }

            return _pathCache.TryGetValue(pathType, out string path) ? path : null;
        }

        #region Private Method

        /// <summary>
        /// キャッシュを構築する
        /// </summary>
        private void BuildPathCache()
        {
            _pathCache = new Dictionary<SoundsPathType, string>();

            foreach (var soundPath in _soundPaths)
            {
                if (soundPath.IsValid())
                {
                    if (_pathCache.ContainsKey(soundPath.PathType))
                    {
                        Debug.LogWarning(
                            $"[SoundsPathSO] Duplicate path type detected: {soundPath.PathType}. Using first occurrence.",
                            this);
                        continue;
                    }

                    _pathCache[soundPath.PathType] = soundPath.Path;
                }
            }
        }

        #endregion
    }
}