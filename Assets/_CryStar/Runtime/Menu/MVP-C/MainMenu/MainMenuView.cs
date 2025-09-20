using System;
using CryStar.Attribute;
using CryStar.Utility;
using UnityEngine;

namespace CryStar.Menu
{
    /// <summary>
    /// MainMenu_View
    /// </summary>
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField, HighlightIfNull] private CustomButton _statusButton;
        [SerializeField, HighlightIfNull] private CustomButton _itemButton;
        [SerializeField, HighlightIfNull] private CustomButton _backTitleButton;
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(Action onStatus, Action onItem, Action onBackTitle)
        {
            _statusButton.onClick.SafeReplaceListener(() => onStatus?.Invoke());
            _itemButton.onClick.SafeReplaceListener(() => onItem?.Invoke());
            _backTitleButton.onClick.SafeReplaceListener(() => onBackTitle?.Invoke());
        }

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            _statusButton.onClick.SafeRemoveAllListeners();
            _itemButton.onClick.SafeRemoveAllListeners();
            _backTitleButton.onClick.SafeRemoveAllListeners();
        }
    }
}