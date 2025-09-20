using System;
using CryStar.Attribute;
using CryStar.Utility;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace iCON.UI
{
    /// <summary>
    /// CanvasController_QuitConfirm
    /// </summary>
    public class CanvasController_QuitConfirm : WindowBase
    {
        /// <summary>
        /// ゲーム終了ボタンを押したときのコールバック
        /// </summary>
        public event Action OnYesButtonClicked;
        
        /// <summary>
        /// キャンセルボタンを押したときのコールバック
        /// </summary>
        public event Action OnNoButtonClicked;
        
        [SerializeField, HighlightIfNull] private CustomButton _yesButton;
        [SerializeField, HighlightIfNull] private CustomButton _noButton;
                
        public override UniTask OnAwake()
        {
            // イベント登録
            _yesButton.onClick.SafeAddListener(() => OnYesButtonClicked?.Invoke());
            _noButton.onClick.SafeAddListener(() => OnNoButtonClicked?.Invoke());
            return base.OnAwake();
        }
        
        private void OnDestroy()
        {
            _yesButton.onClick.SafeRemoveAllListeners();
            _noButton.onClick.SafeRemoveAllListeners();
        }
    }
}