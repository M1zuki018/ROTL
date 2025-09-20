using CryStar.Attribute;
using UnityEngine;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// Lose_View
    /// </summary>
    public class LoseView : MonoBehaviour
    {
        [SerializeField, HighlightIfNull] private CustomText _textBox;
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            // メッセージを表示
            _textBox.SetText($"負けてしまった...");
        }

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            // テキストはリセットして空にしておく
            _textBox.SetText("");
        }
    }
}