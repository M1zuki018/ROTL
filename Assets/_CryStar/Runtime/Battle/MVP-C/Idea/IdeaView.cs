using System;
using CryStar.Attribute;
using CryStar.CommandBattle.UI;
using CryStar.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// Idea_View
    /// </summary>
    public class IdeaView : MonoBehaviour, IPointerExitHandler
    {
        /// <summary>
        /// Ideaを選択したときのコールバック
        /// </summary>
        private event Action<int> _onIdeaSelected;

        /// <summary>
        /// 画面を閉じるコールバック
        /// </summary>
        private Action _onClose;
        
        [SerializeField, HighlightIfNull] private IdeaContents _commandIdeaContents;
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(Action<int> onIdeaSelected, Action onClose)
        {
            _onIdeaSelected += onIdeaSelected;
            _onClose = onClose;
            
            // キャンバスを表示
            CanvasSetActive(_commandIdeaContents.CanvasGroup, true);
            // 既存のリスナーをクリーンアップしてから新しいリスナーを登録
            CleanupCommandButtonListeners();
            SetupCommandButtonListeners();
        }
        
        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            CleanupButtonListeners();
            
            _onIdeaSelected = null;
            _onClose = null;
            
            CanvasSetActive(_commandIdeaContents.CanvasGroup, false);
        }
        
        /// <summary>
        /// ターゲットから出た場合にセレクターを閉じる処理を呼ぶ
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            _onClose?.Invoke();
        }

        /// <summary>
        /// コマンドボタンのリスナーを設定
        /// </summary>
        private void SetupCommandButtonListeners()
        {
            for (int i = 0; i < _commandIdeaContents.IdeaButtons.Count; i++)
            {
                int index = i;
                _commandIdeaContents.IdeaButtons[i].onClick.SafeReplaceListener(() => _onIdeaSelected?.Invoke(index));
            }
        }
        
        /// <summary>
        /// コマンドボタンのリスナーをクリーンアップ
        /// </summary>
        private void CleanupCommandButtonListeners()
        {
            for (int i = 0; i < _commandIdeaContents.IdeaButtons.Count; i++)
            {
                _commandIdeaContents.IdeaButtons[i].onClick.SafeRemoveAllListeners();
            }
        }
        
        /// <summary>
        /// 全てのボタンリスナーをクリーンアップ
        /// </summary>
        private void CleanupButtonListeners()
        {
            CleanupCommandButtonListeners();
        }

        /// <summary>
        /// CanvasGroupの表示/非表示を切り替える
        /// </summary>
        private void CanvasSetActive(CanvasGroup canvasGroup, bool isActive)
        {
            canvasGroup.alpha = isActive ? 1 : 0;
            canvasGroup.interactable = isActive;
            canvasGroup.blocksRaycasts = isActive;
        }
    }
}