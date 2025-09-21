using System;
using CryStar.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// TargetInfo_View
    /// </summary>
    public class TargetInfoView : MonoBehaviour
    {
        /// <summary>
        /// カバー
        /// </summary>
        [SerializeField] 
        private Button _cover;
        
        /// <summary>
        /// ターゲット情報を表示するUI群を管理するクラス
        /// </summary>
        [SerializeField] 
        private UIContents_TargetInfo _infomation;
        
        /// <summary>
        /// ターゲットの名前を表示するUI群を管理するクラス
        /// </summary>
        [SerializeField]
        private UIContents_TargetName _name;

        /// <summary>
        /// 左上の敵の分析テキストの参照
        /// </summary>
        [SerializeField] 
        private CustomText _analysis;

        /// <summary>
        /// 左上のUI群のCanvasGroup
        /// </summary>
        [SerializeField] 
        private CanvasGroup _upperLeftCanvasGourp;

        /// <summary>
        /// キャラクターの立ち絵を表示するコンポーネントの参照
        /// </summary>
        [SerializeField] 
        private CustomImage _character;
        
        /// <summary>
        /// キャラクターの立ち絵を表示するUI群のCanvasGroup
        /// </summary>
        [SerializeField]
        private CanvasGroup _characterCanvasGroup;
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(Action transitionAction)
        {
            // イベント登録
            _cover.onClick.SafeReplaceListener(() => transitionAction?.Invoke());
        }

        // TODO: 実装

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            _cover.onClick.SafeRemoveAllListeners();
        }
    }
}