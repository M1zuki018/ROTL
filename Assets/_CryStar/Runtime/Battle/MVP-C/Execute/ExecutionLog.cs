using DG.Tweening;
using UnityEngine;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// コマンド実行ログを管理するクラス
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class ExecutionLog : MonoBehaviour
    {
        /// <summary>
        /// CanvasGroup
        /// </summary>
        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        /// <summary>
        /// ログ用のテキストコンポーネント
        /// </summary>
        [SerializeField] 
        private CustomText _text;
        
        /// <summary>
        /// 表示アニメーションのスピード
        /// </summary>
        [SerializeField]
        private float _animDuration = 0.2f;
        
        /// <summary>
        /// 初期位置
        /// </summary>
        private Vector3 _initialPosition;
        
        /// <summary>
        /// 移動後の位置
        /// </summary>
        private Vector3 MovedPosition => _initialPosition + Vector3.left * 200f;

        #region Life cycle

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            if (_canvasGroup == null)
            {
                // 自身についているCanvasGroupを取得
                _canvasGroup = GetComponent<CanvasGroup>();
            }
            
            if (_text == null)
            {
                // 子オブジェクトから取得
                _text = GetComponentInChildren<CustomText>();
            }
            
            // 初期位置を保存
            //_initialPosition = transform.localPosition;
            
            // 透明度は0の状態で初期化
            _canvasGroup.alpha = 0;
            
            gameObject.SetActive(false);
        }

        #endregion

        /// <summary>
        /// テキストを設定する
        /// </summary>
        public void SetText(string text)
        {
            _text.text = text;
        }

        /// <summary>
        /// 表示
        /// </summary>
        public Tween Show()
        {
            // 左側にずらした状態にしておく
            transform.localPosition = MovedPosition;
            
            // 表示
            gameObject.SetActive(true);
            
            var sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(1f, _animDuration));
            //sequence.Join(transform.DOMove(transform.localPosition, _animDuration));
            
            return sequence;
        }

        /// <summary>
        /// 非表示
        /// </summary>
        public Tween Hide()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(0f, _animDuration));
            //sequence.Join(transform.DOMove(MovedPosition, _animDuration));

            // 非表示
            sequence.OnComplete(() => gameObject.SetActive(false));
            
            return sequence;
        }
    }
}