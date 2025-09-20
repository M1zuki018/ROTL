using System;
using CryStar.Attribute;
using CryStar.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// FirstSelect_View
    /// </summary>
    public class FirstSelectView : MonoBehaviour
    {
        /// <summary>
        /// たたかうボタン
        /// </summary>
        [SerializeField, HighlightIfNull] 
        private Button _battle;
        
        /// <summary>
        /// にげるボタン
        /// </summary>
        [SerializeField, HighlightIfNull] 
        private Button _escape;
    
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(Action startAction, Action escapeAction)
        {
            // イベント登録
            _battle.onClick.SafeReplaceListener(() => startAction?.Invoke());
            _escape.onClick.SafeReplaceListener(() => escapeAction?.Invoke());
        }

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            _battle.onClick.SafeRemoveAllListeners();
            _escape.onClick.SafeRemoveAllListeners();
        }
    }
}
