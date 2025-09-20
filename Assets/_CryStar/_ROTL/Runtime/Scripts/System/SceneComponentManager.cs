using UnityEngine;
using UnityEngine.EventSystems;

namespace iCON.System
{
    /// <summary>
    /// シーン遷移時に前のシーンのAudioListenerとEventSystemを無効化する
    ///
    /// NOTE: シーン遷移をAdditiveで行っているため、古いシーンと新しいシーンが存在する瞬間がある
    /// そのときにAudioListenerとEventSystemが二重で存在するために警告が出るので、その対策を行っている
    /// </summary>
    public class SceneComponentManager : MonoBehaviour
    {
        /// <summary>
        /// 現在登録中のインスタンス
        /// </summary>
        private static SceneComponentManager _currentInstance;
        
        [SerializeField] private AudioListener _audioListener;
        [SerializeField] private EventSystem _eventSystem;

        #region Life cycle

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            if (_currentInstance != null)
            {
                // 前のインスタンスがあれば、そのインスタンスに登録されているAudioListenerとEventSystemを無効化
                // NOTE: 古いシーンがある場合はこの処理が実行される
                _currentInstance.EnableComponents(false);
            }

            // インスタンスの書き換え・新しいシーンのコンポーネントを有効化する
            _currentInstance = this;
            EnableComponents(true);
        }
        
        /// <summary>
        /// Destroy
        /// </summary>
        private void OnDestroy()
        {
            if (_currentInstance == this)
            {
                // インスタンスに設定されているものが自分ならnullを代入してクリーンアップ
                _currentInstance = null;
            }
        }

        #endregion
        
        /// <summary>
        /// コンポーネントの有効/無効を切り替える
        /// </summary>
        private void EnableComponents(bool isEnable)
        {
            if (_audioListener != null) _audioListener.enabled = isEnable;
            if (_eventSystem != null) _eventSystem.enabled = isEnable;
        }
    }
}