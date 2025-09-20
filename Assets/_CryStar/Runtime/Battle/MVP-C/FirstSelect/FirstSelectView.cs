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
        /// 編成ボタン
        /// </summary>
        [SerializeField, HighlightIfNull] 
        private Button _organization;
        
        /// <summary>
        /// にげるボタン
        /// </summary>
        [SerializeField, HighlightIfNull] 
        private Button _escape;
    
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(Action startAction, Action organizationAction, Action escapeAction)
        {
            // イベント登録
            _battle.onClick.SafeReplaceListener(() => startAction?.Invoke());
            _organization.onClick.SafeReplaceListener(() => organizationAction?.Invoke());
            _escape.onClick.SafeReplaceListener(() => escapeAction?.Invoke());
        }

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            _battle.onClick.SafeRemoveAllListeners();
            _organization.onClick.SafeRemoveAllListeners();
            _escape.onClick.SafeRemoveAllListeners();
        }
    }
}
