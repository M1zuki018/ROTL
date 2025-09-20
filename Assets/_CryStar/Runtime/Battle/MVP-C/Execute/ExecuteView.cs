using CryStar.Attribute;
using UnityEngine;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// Execute_View
    /// </summary>
    public class ExecuteView : MonoBehaviour
    {
        [SerializeField, HighlightIfNull] private CustomText _textBox;
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            // 引数にActionを羅列する
        }

        /// <summary>
        /// テキストを設定
        /// </summary>
        public void SetText(string text)
        {
            _textBox.SetText(text);
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