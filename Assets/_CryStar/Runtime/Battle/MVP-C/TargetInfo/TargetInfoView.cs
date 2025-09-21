using System;
using CryStar.Utility;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// TargetInfo_View
    /// </summary>
    public class TargetInfoView : MonoBehaviour
    {
        #region Field
        
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
        /// パネルを表示するアニメーションにかける時間
        /// </summary>
        [Header("アニメーションの設定")][SerializeField]
        private float _entranceDuration = 0.8f;
        
        /// <summary>
        /// パネルを非表示にするアニメーションにかける時間
        /// </summary>
        [SerializeField]
        private float _exitDuration = 0.5f;
        
        /// <summary>
        /// キャラクターの浮遊アニメーションの時間
        /// </summary>
        [SerializeField]
        private float _floatDuration = 2.0f;
        
        /// <summary>
        /// キャラクターの浮遊アニメーションの振幅
        /// </summary>
        [SerializeField]
        private float _floatAmplitude = 10f;

        // 初期位置とスケールを保存
        private Vector3 _characterInitialPosition;
        private Vector3 _characterInitialScale;
        private Vector3 _upperLeftInitialPosition;
        private Vector3 _infoInitialPosition;
        private Vector3 _nameInitialPosition;
        
        // アニメーション用Sequence
        private DG.Tweening.Sequence _entranceSequence;
        private Tween _floatTween;
        private DG.Tweening.Sequence _exitSequence;

        #endregion

        #region Life cycle

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            // 初期位置とスケールを保存
            SaveInitialTransforms();
            // 初期状態を設定（非表示）
            SetInitialHiddenState();
        }

        /// <summary>
        /// Destroy
        /// </summary>
        private void OnDestroy()
        {
            Exit();
        }

        #endregion
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(Action transitionAction)
        {
            // パネルを閉じるときのアニメーションを登録
            _cover.onClick.SafeReplaceListener(() => PlayExitAnimation(() => transitionAction?.Invoke()));
            
            
        }

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            // 全てのアニメーションを停止
            _entranceSequence?.Kill();
            _floatTween?.Kill();
            _exitSequence?.Kill();
            
            _cover.onClick.SafeRemoveAllListeners();
        }

        /// <summary>
        /// エントランスアニメーションを開始する
        /// NOTE: UIを更新し終わったあとに開始したい
        /// </summary>
        public void PlayAnimation()
        {
            PlayEntranceAnimation();
        }
        
        /// <summary>
        /// キャラクターの画像を設定する
        /// </summary>
        public async UniTask SetCharacterPreview(string iconPath)
        {
            await _character.ChangeSpriteAsync(iconPath);
        }

        #region Animation

        /// <summary>
        /// 初期のTransform情報を保存
        /// </summary>
        private void SaveInitialTransforms()
        {
            // キャラクターの画像
            _characterInitialPosition = _character.transform.localPosition;
            _characterInitialScale = _character.transform.localScale;
            
            // 左上のテキスト
            _upperLeftInitialPosition = _upperLeftCanvasGourp.transform.localPosition;

            // 左下の敵の情報コンテンツ
            _infoInitialPosition = _infomation.transform.localPosition;

            // 右側の名前などのコンテンツ
            _nameInitialPosition = _name.transform.localPosition;
        }

        /// <summary>
        /// 初期状態を非表示に設定
        /// </summary>
        private void SetInitialHiddenState()
        {
            // CanvasGroupを透明に
            _upperLeftCanvasGourp.alpha = 0f;
            _characterCanvasGroup.alpha = 0f;
            _infomation.CanvasGroup.alpha = 0f;
            _name.CanvasGroup.alpha = 0f;
            
            // キャラクターを小さくして右に配置
            _character.transform.localScale = Vector3.zero;
            _character.transform.localPosition = _characterInitialPosition + Vector3.right * 200f;
            
            // 左上UIを左上に移動
            _upperLeftCanvasGourp.transform.localPosition = _upperLeftInitialPosition + Vector3.up * 100f + Vector3.left * 100f;
            
            // 情報UIを下に移動
            _infomation.transform.localPosition = _infoInitialPosition + Vector3.down * 300f;
            
            // 名前UIを上に移動
            _name.transform.localPosition = _nameInitialPosition + Vector3.up * 50f;
        }

        /// <summary>
        /// エントランスアニメーションを再生
        /// </summary>
        private void PlayEntranceAnimation()
        {
            // 念のため最初にキルしておく
            _entranceSequence?.Kill();
            _entranceSequence = DOTween.Sequence();
            
            // キャラクター登場（スケールアップ + 位置移動 + フェードイン）
            _entranceSequence.Append(
                DOTween.To(() => _character.transform.localScale, x => _character.transform.localScale = x, _characterInitialScale, _entranceDuration * 0.4f)
                    .SetEase(Ease.OutBack, 1.2f)
            );
            
            _entranceSequence.Join(
                _character.transform.DOLocalMove(_characterInitialPosition, _entranceDuration * 0.4f)
                    .SetEase(Ease.OutCubic)
            );
            
            _entranceSequence.Join(
                _characterCanvasGroup.DOFade(1f, _entranceDuration * 0.3f)
            );

            // 左上UI（位置移動 + フェードイン）
            _entranceSequence.Append(
                _upperLeftCanvasGourp.transform.DOLocalMove(_upperLeftInitialPosition, _entranceDuration * 0.3f)
                    .SetEase(Ease.OutCubic)
            );
            
            _entranceSequence.Join(
                _upperLeftCanvasGourp.DOFade(1f, _entranceDuration * 0.25f)
            );

            // 情報UI
            // アニメーション全体の半分のタイミングで遅れて登場する
            _entranceSequence.Insert(_entranceDuration * 0.5f,
                _infomation.transform.DOLocalMove(_infoInitialPosition, _entranceDuration * 0.3f)
                    .SetEase(Ease.OutCubic)
            );
            
            _entranceSequence.Insert(_entranceDuration * 0.5f,
                _infomation.CanvasGroup.DOFade(1f, _entranceDuration * 0.3f)
            );

            // 名前UI
            // 情報UIの少し後に登場
            _entranceSequence.Insert(_entranceDuration * 0.6f,
                _name.transform.DOLocalMove(_nameInitialPosition, _entranceDuration * 0.3f)
                    .SetEase(Ease.OutCubic)
            );
            
            _entranceSequence.Insert(_entranceDuration * 0.6f,
                _name.CanvasGroup.DOFade(1f, _entranceDuration * 0.3f)
            );

            // アニメーション完了後にアイドルアニメーション開始（キャラクターがふよふよする）
            _entranceSequence.OnComplete(StartFloatingAnimation);
        }

        /// <summary>
        /// キャラクターの浮遊アニメーション開始（ずっと繰り返す）
        /// </summary>
        private void StartFloatingAnimation()
        {
            // 既存のシーケンスがあればキル
            _floatTween?.Kill();
            
            _floatTween = _character.transform.DOLocalMoveY(
                _characterInitialPosition.y + _floatAmplitude, _floatDuration
            )
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
        }

        /// <summary>
        /// エグジットアニメーションを再生
        /// エントランスアニメーションの逆向きを再生する
        /// </summary>
        private void PlayExitAnimation(Action onComplete)
        {
            // 進行中のアニメーションがあれば停止
            _entranceSequence?.Kill();
            _floatTween?.Kill();
            _exitSequence?.Kill();

            _exitSequence = DOTween.Sequence();

            // 名前UIを上に移動してフェードアウト
            _exitSequence.Append(
                _name.transform.DOLocalMove(_nameInitialPosition + Vector3.up * 50f, _exitDuration * 0.3f)
                    .SetEase(Ease.InCubic)
            );
            
            _exitSequence.Join(
                _name.CanvasGroup.DOFade(0f, _exitDuration * 0.3f)
            );

            // 情報UI
            _exitSequence.Join(
                _infomation.transform.DOLocalMove(_infoInitialPosition + Vector3.down * 100f, _exitDuration * 0.4f)
                    .SetEase(Ease.InCubic)
            );
            
            _exitSequence.Join(
                _infomation.CanvasGroup.DOFade(0f, _exitDuration * 0.4f)
            );

            // 左上UI
            _exitSequence.Join(
                _upperLeftCanvasGourp.DOFade(0f, _exitDuration * 0.4f)
            );
            
            _exitSequence.Join(
                _upperLeftCanvasGourp.transform.DOLocalMove(_upperLeftInitialPosition + Vector3.up * 100f + Vector3.left * 100f, _exitDuration * 0.4f)
                    .SetEase(Ease.InCubic)
            );

            // キャラクターのアニメーション
            _exitSequence.Append(
                DOTween.To(() => _character.transform.localScale, x => _character.transform.localScale = x, Vector3.zero, _exitDuration * 0.4f)
                    .SetEase(Ease.OutQuint, 1.2f)
            );
            
            _exitSequence.Join(
                _character.transform.DOLocalMove(_characterInitialPosition + Vector3.right * 200f, _exitDuration * 0.4f)
                    .SetEase(Ease.InCubic)
            );
            
            _exitSequence.Join(
                _characterCanvasGroup.DOFade(0f, _exitDuration * 0.3f)
            );

            // アニメーション完了時のコールバック
            _exitSequence.OnComplete(() => onComplete?.Invoke());
        }
        
        #endregion
    }
}