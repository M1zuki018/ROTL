using System;
using CryStar.Attribute;
using CryStar.Utility;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace iCON.UI
{
    /// <summary>
    /// CanvasController_PlayStyleSelect
    /// </summary>
    public class CanvasController_PlayStyleSelect : WindowBase
    {
        /// <summary>
        /// ストーリーモードでPlay
        /// </summary>
        public event Action OnStoryModeButtonClicked;
        
        /// <summary>
        /// バトルモードでPlay
        /// </summary>
        public event Action OnBattleModeButtonClicked;
        
        [SerializeField, HighlightIfNull] private CustomButton _storyModeButton;
        [SerializeField, HighlightIfNull] private CustomButton _battleModeButton;
        [SerializeField, HighlightIfNull] private CustomText _descriptionText;

        [Header("文言キーの指定")]
        [SerializeField] private string _descriptionKey = "TITLE_PLAYSTYLE_DESCRIPTION";
        [SerializeField] private string _storyTimeRequiedKey = "STORY_TIME_REQUIRED";
        [SerializeField] private string _battleTimeRequiedKey = "BATTLE_TIME_REQUIRED";
        [SerializeField] private string _forThoseStoryKey = "FOR_THOSE_STORY";
        [SerializeField] private string _forThoseBattleKey = "FOR_THOSE_BATTLE";
        
        /// <summary>
        /// Awake
        /// </summary>
        public override UniTask OnAwake()
        {
            // イベント登録
            _storyModeButton.onClick.SafeAddListener(HandleStoryModeButtonClicked);
            _battleModeButton.onClick.SafeAddListener(HandleBattleModeButtonClicked);
            
            ChangeDescriptionText(true);
            
            // ボタンを全て有効化しておく
            AllButtonsEnabled(true);
            
            return base.OnAwake();
        }

        /// <summary>
        /// ストーリーモードのボタンを押した時の処理
        /// </summary>
        private void HandleStoryModeButtonClicked()
        {
            OnStoryModeButtonClicked?.Invoke();
            AllButtonsEnabled(false);
        }

        /// <summary>
        /// バトルモードのボタンを押した時の処理
        /// </summary>
        private void HandleBattleModeButtonClicked()
        {
            OnBattleModeButtonClicked?.Invoke();
            AllButtonsEnabled(false);
        }
        
        /// <summary>
        /// カーソルが合っているときに説明文章を変更する
        /// </summary>
        private void ChangeDescriptionText(bool isStoryMode)
        {
            // TODO: カーソルが合っている時を判定する
            if (_descriptionText == null)
            {
                // テキストコンポーネントが設定されていなければ早期リターン
                return;
            }
            
            // フォーマットの代入に利用する文言キーを取得する
            // NOTE: isStoryMode = trueならストーリーモードのキーを取得する
            var arg1 = isStoryMode ? _storyTimeRequiedKey : _battleTimeRequiedKey; 
            var arg2 = isStoryMode ? _forThoseStoryKey : _forThoseBattleKey;
            
            // フォーマットを使ってテキストを設定する
            _descriptionText.SetWordingTextFormat(_descriptionKey, arg1, arg2);   
        }

        /// <summary>
        /// 全てのボタンのアクティブ状態を変更する
        /// </summary>
        private void AllButtonsEnabled(bool enabled)
        {
            ButtonEnabled(_storyModeButton, enabled);
            ButtonEnabled(_battleModeButton, enabled);
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
        /// Destroy
        /// </summary>
        private void OnDestroy()
        {
            _storyModeButton.onClick.SafeRemoveAllListeners();
            _battleModeButton.onClick.SafeRemoveAllListeners();
        }
    }
}