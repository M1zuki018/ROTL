using System;
using CryStar.Attribute;
using CryStar.Utility;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace iCON.UI
{
    /// <summary>
    /// CanvasController_Title
    /// </summary>
    public class CanvasController_Title : WindowBase
    {
        /// <summary>
        /// ゲームを最初から始めるボタンを押したときのコールバック
        /// </summary>
        public event Action OnNewGameButtonClicked;
        
        /// <summary>
        /// ゲームを続きから始めるボタンを押したときのコールバック
        /// </summary>
        public event Action OnLoadGameButtonClicked;
        
        /// <summary>
        /// 設定画面を開くボタンを押したときのコールバック
        /// </summary>
        public event Action OnConfigButtonClicked;
        
        /// <summary>
        /// ゲーム終了ボタンを押したときのコールバック
        /// </summary>
        public event Action OnQuitButtonClicked;
        
        [Header("ボタンの参照")]
        [SerializeField, HighlightIfNull] private CustomButton _newGameButton;
        [SerializeField, HighlightIfNull] private CustomButton _loadGameButton;
        [SerializeField, HighlightIfNull] private CustomButton _configButton;
        [SerializeField, HighlightIfNull] private CustomButton _quitButton;
        
        [Header("色の設定")]
        [SerializeField] private Color _clickedColor = Color.cyan;
        
        public override UniTask OnAwake()
        {
            // イベント登録
            _newGameButton.onClick.SafeAddListener(HandleNewGameButtonClicked);
            _loadGameButton.onClick.SafeAddListener(HandleLoadGameButtonClicked);
            _configButton.onClick.SafeAddListener(HandleConfigButtonClicked);
            _quitButton.onClick.SafeAddListener(HandleQuitButtonClicked);

            // ボタンを確実に有効化する
            ButtonEnabled(_newGameButton, true);
            ButtonEnabled(_loadGameButton, true);
            ButtonEnabled(_configButton, true);
            ButtonEnabled(_quitButton, true);
           
            return base.OnAwake();
        }

        /// <summary>
        /// ゲームを最初から始めるボタンを押したときの処理
        /// </summary>
        private void HandleNewGameButtonClicked()
        {
            OnNewGameButtonClicked?.Invoke();
        }

        /// <summary>
        /// ゲームを続きから始めるボタンを押したときの処理
        /// </summary>
        private void HandleLoadGameButtonClicked()
        {
            GameStartButtonClicked(_loadGameButton);
            OnLoadGameButtonClicked?.Invoke();
        }

        /// <summary>
        /// 設定画面を開くボタンを押したときの処理
        /// </summary>
        private void HandleConfigButtonClicked()
        {
            // 特に二度押し対策は行っていない
            OnConfigButtonClicked?.Invoke();
        }

        /// <summary>
        /// ゲーム終了ボタンを押したときの処理
        /// </summary>
        private void HandleQuitButtonClicked()
        {
            // 特に二度押し対策は行っていない
            OnQuitButtonClicked?.Invoke();
        }
        
        /// <summary>
        /// nullチェックをしてボタンの有効/無効を切り替える
        /// </summary>
        private void ButtonEnabled(CustomButton button, bool isEnabled)
        {
            if (button != null)
            {
                button.enabled = isEnabled;
            }
        }

        /// <summary>
        /// ゲーム開始ボタンが押されたときの処理（二度押し対策とボタンの色変更）
        /// </summary>
        private void GameStartButtonClicked(CustomButton button)
        {
            ButtonEnabled(_newGameButton, false);
            ButtonEnabled(_loadGameButton, false);
            ButtonEnabled(_configButton, false);
            ButtonEnabled(_quitButton, false);
            
            // ボタンの色を変更する
            button.image.color = _clickedColor;
        }
        
        /// <summary>
        /// Destroy
        /// </summary>
        private void OnDestroy()
        {
            _newGameButton.onClick?.SafeRemoveAllListeners();
            _loadGameButton.onClick?.SafeRemoveAllListeners();
            _configButton.onClick?.SafeRemoveAllListeners();
            _quitButton.onClick?.SafeRemoveAllListeners();
        }
    }
}