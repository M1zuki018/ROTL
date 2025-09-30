using System;
using CryStar.PerProject;
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
        [SerializeField] private Button _cover;

        /// <summary>
        /// ターゲット情報を表示するUI群を管理するクラス
        /// </summary>
        [SerializeField] private UIContents_TargetInfo _infomation;

        /// <summary>
        /// ターゲットの名前を表示するUI群を管理するクラス
        /// </summary>
        [SerializeField] private UIContents_TargetName _name;

        /// <summary>
        /// 左上の敵の分析テキストの参照
        /// </summary>
        [SerializeField] private CustomText _analysis;

        /// <summary>
        /// 左上のUI群のCanvasGroup
        /// </summary>
        [SerializeField] private CanvasGroup _upperLeftCanvasGourp;

        /// <summary>
        /// キャラクターの立ち絵を表示するコンポーネントの参照
        /// </summary>
        [SerializeField] private CustomImage _character;

        /// <summary>
        /// キャラクターの立ち絵を表示するUI群のCanvasGroup
        /// </summary>
        [SerializeField] private CanvasGroup _characterCanvasGroup;

        /// <summary>
        /// パネルを表示するアニメーションにかける時間
        /// </summary>
        [Header("アニメーションの設定")] [SerializeField]
        private float _entranceDuration = 0.8f;

        /// <summary>
        /// パネルを非表示にするアニメーションにかける時間
        /// </summary>
        [SerializeField] private float _exitDuration = 0.5f;

        /// <summary>
        /// キャラクターの浮遊アニメーションの時間
        /// </summary>
        [SerializeField] private float _floatDuration = 2.0f;

        /// <summary>
        /// キャラクターの浮遊アニメーションの振幅
        /// </summary>
        [SerializeField] private float _floatAmplitude = 10f;

        // 初期位置とスケールを保存
        private Vector3 _characterInitialPosition;
        private Vector3 _characterInitialScale;
        private Vector3 _upperLeftInitialPosition;
        private Vector3 _infoInitialPosition;
        private Vector3 _nameInitialPosition;

        // アニメーション用Sequence
        private Sequence _entranceSequence;
        private Tween _floatTween;
        private Sequence _exitSequence;

        // Canvas Scaler のスケール係数
        private float _canvasScaleFactor = 1f;
        private Canvas _canvas;

        #endregion

        #region Life cycle

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            // Canvas Scaler の情報を取得
            InitializeCanvasScaler();
        }

        /// <summary>
        /// Start - Canvas Scaler の計算が完了してから実行
        /// </summary>
        private void Start()
        {
            // Canvas Scaler の計算完了後にスケール係数を更新
            UpdateCanvasScaleFactor();
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
        /// Canvas Scaler の初期化
        /// </summary>
        private void InitializeCanvasScaler()
        {
            _canvas = GetComponentInParent<Canvas>();
            if (_canvas == null)
            {
                Debug.LogWarning("Canvas が見つかりません。Canvas Scaler の補正が無効になります。");
            }
        }

        /// <summary>
        /// Canvas Scaler のスケール係数を更新
        /// </summary>
        private void UpdateCanvasScaleFactor()
        {
            if (_canvas != null && _canvas.renderMode != RenderMode.WorldSpace)
            {
                var canvasScaler = _canvas.GetComponent<CanvasScaler>();
                if (canvasScaler != null && canvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
                {
                    // Canvas Scaler の実際のスケール係数を取得
                    var referenceResolution = canvasScaler.referenceResolution;
                    var screenSize = new Vector2(Screen.width, Screen.height);
                    
                    float scaleFactorX = screenSize.x / referenceResolution.x;
                    float scaleFactorY = screenSize.y / referenceResolution.y;
                    
                    switch (canvasScaler.screenMatchMode)
                    {
                        case CanvasScaler.ScreenMatchMode.MatchWidthOrHeight:
                            _canvasScaleFactor = Mathf.Lerp(scaleFactorX, scaleFactorY, canvasScaler.matchWidthOrHeight);
                            break;
                        case CanvasScaler.ScreenMatchMode.Expand:
                            _canvasScaleFactor = Mathf.Min(scaleFactorX, scaleFactorY);
                            break;
                        case CanvasScaler.ScreenMatchMode.Shrink:
                            _canvasScaleFactor = Mathf.Max(scaleFactorX, scaleFactorY);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Canvas Scaler を考慮したオフセット値を取得
        /// </summary>
        private Vector3 GetScaledOffset(Vector3 offset)
        {
            return offset * _canvasScaleFactor;
        }

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(Action transitionAction)
        {
            // パネルを閉じるときのアニメーションを登録
            _cover.onClick.SafeReplaceListener(() => HandleCoverClicked(() => transitionAction?.Invoke()));
        }

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            // 全てのアニメーションを停止
            _entranceSequence?.Kill(true);
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
            // アニメーション開始時にスケール係数を更新
            UpdateCanvasScaleFactor();
            PlayEntranceAnimation();
        }

        /// <summary>
        /// キャラクターの画像を設定する
        /// </summary>
        public async UniTask SetCharacterPreview(string iconPath)
        {
            await _character.ChangeSpriteAsync(iconPath);
        }

        /// <summary>
        /// キャラクター名を設定する
        /// </summary>
        public void SetTargetName(string name)
        {
            _name.SetTargetName(name);
        }

        /// <summary>
        /// ターゲット情報を設定する
        /// </summary>
        public void SetTargetInfo(RaceType raceType, AttackType attackType, AttackType weaknessType,
            string recommendedLevel, string additionalExplanation)
        {
            _infomation.SetFirstLine(raceType, attackType, weaknessType);
            _infomation.SetSecondLine(recommendedLevel);
            _infomation.SetThirdLine(additionalExplanation);
        }
        
        /// <summary>
        ///  カバーボタンがおされたときの処理
        /// </summary>
        private void HandleCoverClicked(Action transitionAction)
        {
            // 登場アニメーション中かつ、浮遊アニメーションがまだ未再生であればアニメーションをスキップする
            // NOTE: 一度登場アニメーションが終わると浮遊アニメーションが始まるためこのように条件を付けている
            if (_entranceSequence != null && _floatTween == null)
            {
                // エントランスアニメーションを完了させた状態でキル
                _entranceSequence.Kill(true);
                return;
            }

            // 登場アニメーションが終了済み
            // 浮遊アニメーション再生中
            // 退場アニメーション未再生の場合、退場アニメーションと次の画面遷移処理を呼び出す
            if (_floatTween != null && _exitSequence == null)
            {
                PlayExitAnimation(() => transitionAction?.Invoke());
            }
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

            // キャラクターを小さくして右に配置（Canvas Scaler を考慮）
            _character.transform.localScale = Vector3.zero;
            _character.transform.localPosition = _characterInitialPosition + GetScaledOffset(Vector3.right * 200f);

            // 左上UIを左上に移動（Canvas Scaler を考慮）
            _upperLeftCanvasGourp.transform.localPosition =
                _upperLeftInitialPosition + GetScaledOffset(Vector3.up * 100f + Vector3.left * 100f);

            // 情報UIを下に移動（Canvas Scaler を考慮）
            _infomation.transform.localPosition = _infoInitialPosition + GetScaledOffset(Vector3.down * 300f);

            // 名前UIを上に移動（Canvas Scaler を考慮）
            _name.transform.localPosition = _nameInitialPosition + GetScaledOffset(Vector3.up * 50f);
        }

        /// <summary>
        /// エントランスアニメーションを再生
        /// </summary>
        private void PlayEntranceAnimation()
        {
            // 念のため最初にキルしておく
            _entranceSequence?.Kill();
            _entranceSequence = DOTween.Sequence();

            // キャラクター登場（0秒から開始）
            _entranceSequence.Insert(0f,
                DOTween.To(() => _character.transform.localScale, x => _character.transform.localScale = x,
                        _characterInitialScale, _entranceDuration * 0.6f)
                    .SetEase(Ease.OutBack, 1.2f)
            );

            _entranceSequence.Insert(0f,
                _character.transform.DOLocalMove(_characterInitialPosition, _entranceDuration * 0.6f)
                    .SetEase(Ease.OutCubic)
            );

            _entranceSequence.Insert(0f,
                _characterCanvasGroup.DOFade(1f, _entranceDuration * 0.3f)
            );

            // 左上UI（キャラクターの少し後）
            _entranceSequence.Insert(_entranceDuration * 0.2f,
                _upperLeftCanvasGourp.transform.DOLocalMove(_upperLeftInitialPosition, _entranceDuration * 0.3f)
                    .SetEase(Ease.OutCubic)
            );

            _entranceSequence.Insert(_entranceDuration * 0.2f,
                _upperLeftCanvasGourp.DOFade(1f, _entranceDuration * 0.25f)
            );

            // 情報UI
            _entranceSequence.Insert(_entranceDuration * 0.3f,
                _infomation.transform.DOLocalMove(_infoInitialPosition, _entranceDuration * 0.3f)
                    .SetEase(Ease.OutCubic)
            );

            _entranceSequence.Insert(_entranceDuration * 0.3f,
                _infomation.CanvasGroup.DOFade(1f, _entranceDuration * 0.3f)
            );

            // 名前UI
            _entranceSequence.Insert(_entranceDuration * 0.4f,
                _name.transform.DOLocalMove(_nameInitialPosition, _entranceDuration * 0.3f)
                    .SetEase(Ease.OutCubic)
            );

            _entranceSequence.Insert(_entranceDuration * 0.4f,
                _name.CanvasGroup.DOFade(1f, _entranceDuration * 0.3f)
            );
            
            // アニメーション完了後にアイドルアニメーション開始
            _entranceSequence.OnComplete(StartFloatingAnimation);
        }

        /// <summary>
        /// キャラクターの浮遊アニメーション開始（ずっと繰り返す）
        /// </summary>
        private void StartFloatingAnimation()
        {
            // 既存のシーケンスがあればキル
            _floatTween?.Kill();
            
            // Canvas Scaler を考慮した振幅を計算
            float scaledAmplitude = _floatAmplitude * _canvasScaleFactor;
            
            _floatTween = _character.transform.DOLocalMoveY(
                _characterInitialPosition.y + scaledAmplitude, _floatDuration
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

            // 名前UIを上に移動してフェードアウト（Canvas Scaler を考慮）
            _exitSequence.Append(
                _name.transform.DOLocalMove(_nameInitialPosition + GetScaledOffset(Vector3.up * 50f), _exitDuration * 0.3f)
                    .SetEase(Ease.InCubic)
            );
            
            _exitSequence.Join(
                _name.CanvasGroup.DOFade(0f, _exitDuration * 0.3f)
            );

            // 情報UI（Canvas Scaler を考慮）
            _exitSequence.Join(
                _infomation.transform.DOLocalMove(_infoInitialPosition + GetScaledOffset(Vector3.down * 100f), _exitDuration * 0.4f)
                    .SetEase(Ease.InCubic)
            );
            
            _exitSequence.Join(
                _infomation.CanvasGroup.DOFade(0f, _exitDuration * 0.4f)
            );

            // 左上UI（Canvas Scaler を考慮）
            _exitSequence.Join(
                _upperLeftCanvasGourp.DOFade(0f, _exitDuration * 0.4f)
            );
            
            _exitSequence.Join(
                _upperLeftCanvasGourp.transform.DOLocalMove(_upperLeftInitialPosition + GetScaledOffset(Vector3.up * 100f + Vector3.left * 100f), _exitDuration * 0.4f)
                    .SetEase(Ease.InCubic)
            );

            // キャラクターのアニメーション（Canvas Scaler を考慮）
            _exitSequence.Append(
                DOTween.To(() => _character.transform.localScale, x => _character.transform.localScale = x, Vector3.zero, _exitDuration * 0.4f)
                    .SetEase(Ease.OutQuint, 1.2f)
            );
            
            _exitSequence.Join(
                _character.transform.DOLocalMove(_characterInitialPosition + GetScaledOffset(Vector3.right * 200f), _exitDuration * 0.4f)
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