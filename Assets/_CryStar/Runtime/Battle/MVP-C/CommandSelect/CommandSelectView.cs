using System;
using CryStar.Attribute;
using CryStar.Utility;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// CommandSelect_View
    /// </summary>
    public class CommandSelectView : MonoBehaviour
    {
        [Header("画像")]
        [SerializeField, HighlightIfNull] private CustomImage _characterPreview;

        [SerializeField] private CustomImage _selectorEffect;
        
        [Header("ボタン")]
        [SerializeField, HighlightIfNull] private Button _attack;
        [SerializeField, HighlightIfNull] private Button _idea;
        [SerializeField, HighlightIfNull] private Button _item;
        [SerializeField, HighlightIfNull] private Button _guard;
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(Action onAttack, Action onIdea, Action onItem, Action onGuard)
        {
            _attack.onClick.SafeReplaceListener(() => onAttack?.Invoke());
            _idea.onClick.SafeReplaceListener(() => onIdea?.Invoke());
            _item.onClick.SafeReplaceListener(() => onItem?.Invoke());
            _guard.onClick.SafeReplaceListener(() => onGuard?.Invoke());
        }

        /// <summary>
        /// キャラクターの画像を設定する
        /// </summary>
        public async UniTask SetCharacterPreview(string iconPath)
        {
            await _characterPreview.ChangeSpriteAsync(iconPath);
        }

        /// <summary>
        /// セレクターUIのキャラクターカラーのぼかし素材のカラーを設定する
        /// </summary>
        public void SetEffectColor(Color characterColor)
        {
            if (_selectorEffect != null)
            {
                _selectorEffect.color = characterColor;
            }
        }

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            _attack.onClick.SafeRemoveAllListeners();
            _idea.onClick.SafeRemoveAllListeners();
            _item.onClick.SafeRemoveAllListeners();
            _guard.onClick.SafeRemoveAllListeners();
        }
    }
}