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
        private float _showDuration = 0.3f;
        
        /// <summary>
        /// 初期位置
        /// </summary>
        private float _initialPositionX;
        
        /// <summary>
        /// 開始位置のオフセット
        /// </summary>
        [SerializeField]
        private float _startOffset = -100f;

        /// <summary>
        /// 表示アニメーションのシーケンス
        /// </summary>
        private Sequence _showSequence;
        
        /// <summary>
        /// 非表示アニメーションのシーケンス
        /// </summary>
        private Sequence _hideSequence;
        
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
            _initialPositionX = transform.localPosition.x;
            
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
            // 出現位置を設定
            var pos = transform.localPosition;
            pos.x = _initialPositionX + _startOffset;
            transform.localPosition = pos;
            
            // 念のためキルしておく
            _showSequence?.Kill();
            _hideSequence?.Kill(true);
            _showSequence = DOTween.Sequence();
            
            // 表示
            gameObject.SetActive(true);
            
            // フェードインしながら定位置へスライド
            _showSequence.Append(_canvasGroup.DOFade(1f, _showDuration).SetEase(Ease.OutQuad));
            _showSequence.Join(transform.DOLocalMoveX(_initialPositionX, _showDuration).SetEase(Ease.OutCubic));
            
            return _showSequence;
        }

        /// <summary>
        /// 非表示
        /// </summary>
        public Tween Hide()
        {
            // 念のためキルしておく
            _hideSequence?.Kill();
            _hideSequence = DOTween.Sequence();
            
            // フェードアウトしながら左側へスライド
            _hideSequence.Append(_canvasGroup.DOFade(0f, _showDuration).SetEase(Ease.InQuad));
            _hideSequence.Join(transform.DOLocalMoveX(_initialPositionX + _startOffset, _showDuration).SetEase(Ease.InCubic));
            
            return _hideSequence;
        }

        public void Exit()
        {
            // テキストを空にする
            _text.text = "";

            // 非表示処理を呼び出し
            Hide();

            Dispose();
        }
        
        private void Dispose()
        {
            _showSequence?.Kill(true);
            _hideSequence?.Kill(true);
            _showSequence = null;
            _hideSequence = null;
        }
    }
}