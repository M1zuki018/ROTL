using CryStar.Attribute;
using UnityEngine;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// Win_View
    /// </summary>
    public class WinView : MonoBehaviour
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
        public void SetText(string name, int experience)
        {
            _textBox.SetText($"{name}の勝利\n経験値{experience}を手に入れた");
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