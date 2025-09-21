using System;
using System.Collections.Generic;
using CryStar.Attribute;
using CryStar.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

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
        private CustomButton _battle;

        /// <summary>
        /// 編成ボタン
        /// </summary>
        [SerializeField, HighlightIfNull] 
        private CustomButton _organization;
        
        /// <summary>
        /// にげるボタン
        /// </summary>
        [SerializeField, HighlightIfNull] 
        private CustomButton _escape;
        
        /// <summary>
        /// デフォルトのボタン画像
        /// </summary>
        [Header("ボタンのバリエーションの設定")] [SerializeField]
        private Sprite _defaultSprite;
        
        /// <summary>
        /// 選択中のボタン画像
        /// </summary>
        [SerializeField]
        private Sprite _selectedSprite;
        
        /// <summary>
        /// デフォルトのテキスト色
        /// </summary>
        [SerializeField]
        private Color _defaultTextColor = Color.white;
        
        /// <summary>
        /// 選択中のテキストの色
        /// </summary>
        [SerializeField]
        private Color _selectedTextColor = Color.black;
        
        /// <summary>
        /// 現在選択中のボタン
        /// </summary>
        private CustomButton _currentSelectedButton;
        
        /// <summary>
        /// ボタンのリスト（管理用）
        /// </summary>
        private List<CustomButton> _buttons;

        #region Life cycle

        private void Awake()
        {
            // ボタンリストを初期化
            _buttons = new List<CustomButton> { _battle, _organization, _escape };

            // マウスのホバーイベントを登録する
            EventTriggerResister();
        }

        #endregion
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(Action startAction, Action organizationAction, Action escapeAction)
        {
            // イベント登録
            _battle.onClick.SafeReplaceListener(() => startAction?.Invoke());
            _organization.onClick.SafeReplaceListener(() => organizationAction?.Invoke());
            _escape.onClick.SafeReplaceListener(() => escapeAction?.Invoke());
            
            SelectButton(_battle);
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

        #region Private Methods

        /// <summary>
        /// マウスのホバーイベントを登録する
        /// </summary>
        private void EventTriggerResister()
        {
            foreach (var button in _buttons)
            {
                // EventTriggerコンポーネントを追加（存在しない場合）
                EventTrigger trigger = button.GetComponent<EventTrigger>();
                if (trigger == null)
                {
                    trigger = button.gameObject.AddComponent<EventTrigger>();
                }

                // ボタンにマウスカーソルがのったときのイベントを追加する
                EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerEnter
                };
                pointerEnterEntry.callback.AddListener(_ => SelectButton(button));
                trigger.triggers.Add(pointerEnterEntry);
            }
        }
        
        /// <summary>
        /// ボタンを選択状態にする
        /// </summary>
        private void SelectButton(CustomButton selectedButton)
        {
            if (_currentSelectedButton == selectedButton)
            {
                // 現在選択中のボタンだったら処理をスキップ
                return;
            }

            if (_currentSelectedButton != null)
            {
                // 前の選択を解除する
                // スプライト変更とテキストの色を元に戻す
                _currentSelectedButton.image.sprite = _defaultSprite;    
                _currentSelectedButton.ChangeTextColor(_defaultTextColor);
            }
            
            // 新しいボタンを選択状態にする
            _currentSelectedButton = selectedButton;
            if (_currentSelectedButton != null)
            {
                _currentSelectedButton.image.sprite = _selectedSprite;
                _currentSelectedButton.ChangeTextColor(_selectedTextColor);
            }
        }

        #endregion
    }
}
