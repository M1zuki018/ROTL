#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace CryStar.Core.Editor
{
    /// <summary>
    /// ゲームの実行を停止したときにServiceLocatorに登録されているクラスを全て登録解除する
    /// </summary>
    [InitializeOnLoad]
    public static class ServiceLocatorCleaner
    {
        static ServiceLocatorCleaner()
        {
            // 重複登録を避けるため、一度解除してから登録する
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        /// <summary>
        /// 実行停止時に全サービスの解除を行うメソッド
        /// </summary>
        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                // プレイモード終了直前に全解除
                ServiceLocator.ClearAllServicesEverywhere();
                Debug.Log("[ServiceLocator] 全サービスを解除しました");
            }
        }
    }
}

#endif