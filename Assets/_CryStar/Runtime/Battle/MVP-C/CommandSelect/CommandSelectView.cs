using System;
using CryStar.Attribute;
using CryStar.Utility;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// CommandSelect_View
    /// </summary>
    public class CommandSelectView : MonoBehaviour
    {
        [Header("画像")]
        [SerializeField, HighlightIfNull] private CustomImage _characterPreview;
        [SerializeField] private CustomImage _selectorEffect;
        [SerializeField] private CustomImage _glowEffect;
        
        [Header("ボタン")]
        [SerializeField, HighlightIfNull] private Button _attack;
        [SerializeField, HighlightIfNull] private Button _idea;
        [SerializeField, HighlightIfNull] private Button _item;
        [SerializeField, HighlightIfNull] private Button _guard;
        [SerializeField, HighlightIfNull] private Button _back;
        
        [Header("アニメーションの設定")]
        [SerializeField] private float _entranceDuration = 1.0f;
        [SerializeField] private float _buttonInterval = 0.1f;
        [SerializeField] private float _breathDuration = 3.0f;
        [SerializeField] private float _breathIntensity = 0.05f;
        [SerializeField] private float _effectPulseDuration = 2.0f;
        [SerializeField] private float _hoverDuration = 0.3f;

        [Header("CanvasGroup")]
        [SerializeField] private CanvasGroup _selectorCanvasGroup; // セレクターのキャンバスグループ
        [SerializeField] private CanvasGroup _characterCanvasGroup; // キャラクタープレビュー全体
        
        // アニメーション制御
        private Button[] _allButtons;
        private Vector3 _buttonInitialScale;
        private Vector3[] _buttonInitialPosition;
        private Vector3 _characterInitialPosition;
        private CanvasGroup[] _buttonCanvasGroups;
        private float _glowDefaultIntensity;
        
        private Tween _breathTween;
        private Tween _effectPulseTween;
        
        private bool _isAnimating = false;

        private Action _onIdea;
        private Action _onItem;

        #region Life cycle

        private void Awake()
        {
            // ボタンの初期化
            InitializeButtons();
            
            // ボタンのホバーアニメーションの準備
            SetupHoverEffects();
            
            // 初期状態を非表示に設定
            SetInitialState();
        }
        
        private void OnDestroy()
        {
            Exit();
        }

        #endregion

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(Action onAttack, Action onIdea, Action onItem, Action onGuard, Action onBack)
        {
            // ボタンイベント設定（アニメーション付き）
            _attack.onClick.SafeReplaceListener(() => OnButtonClicked(_attack, () => onAttack?.Invoke()));
            _idea.onClick.SafeReplaceListener(() => OnButtonClicked(_idea, () => onIdea?.Invoke()));
            _item.onClick.SafeReplaceListener(() => OnButtonClicked(_item, () => onItem?.Invoke()));
            _guard.onClick.SafeReplaceListener(() => OnButtonClicked(_guard, () => onGuard?.Invoke()));
            _back.onClick.SafeReplaceListener(() => OnButtonClicked(_back, () => onBack?.Invoke()));
            
            // パネル表示のために保存しておく
            _onIdea = onIdea;
            _onItem = onItem;
        }
        
        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            // アニメーション停止
            _breathTween?.Kill();
            _effectPulseTween?.Kill();
            
            // ボタンイベント削除
            _attack?.onClick.SafeRemoveAllListeners();
            _idea?.onClick.SafeRemoveAllListeners();
            _item?.onClick.SafeRemoveAllListeners();
            _guard?.onClick.SafeRemoveAllListeners();
            _back?.onClick.SafeRemoveAllListeners();
            
            // DOTweenアニメーション停止
            transform.DOKill();
            if (_characterPreview != null) _characterPreview.transform.DOKill();
            if (_selectorEffect != null) _selectorEffect.transform.DOKill();
            foreach (var button in _allButtons)
            {
                if (button != null) button.transform.DOKill();
            }
        }

        /// <summary>
        /// キャラクターの画像を設定する
        /// </summary>
        public async UniTask SetCharacterPreview(string iconPath)
        {
            if (_selectorEffect != null)
            {
                await _characterPreview.ChangeSpriteAsync(iconPath);
            }
        }

        /// <summary>
        /// セレクターUIのキャラクターカラーのぼかし素材のカラーを設定する
        /// </summary>
        public void SetEffectColor(Color characterColor)
        {
            if (_selectorEffect != null)
            {
                _selectorEffect.color = characterColor;
            }
        }

        /// <summary>
        /// アニメーション再生
        /// </summary>
        public void CallEntranceAnimation()
        {
            if (_isAnimating) return;
            _isAnimating = true;
            
            PlayEntranceAnimation();
        }

        /// <summary>
        /// 退場アニメーションを再生
        /// </summary>
        public void CallExitAnimation(Action cancelAction)
        {
            // 退場アニメーションを再生したあとにコールバックを呼び出す
            PlayExitAnimation().OnComplete(() =>
            {
                _isAnimating = false;
                cancelAction?.Invoke();
            });
        }

        #region Private Method

        #region 初期化

        /// <summary>
        /// ボタンの初期化
        /// </summary>
        private void InitializeButtons()
        {
            // 実行用の配列を作成
            _allButtons = new Button[] { _attack, _idea, _item, _guard, _back };
            
            // 初期スケールを保存
            _buttonInitialScale = _attack.transform.localScale;
            
            // 初期位置とキャンバスグループを保存
            _buttonInitialPosition = new Vector3[_allButtons.Length];
            _buttonCanvasGroups = new CanvasGroup[_allButtons.Length];
            
            for (int i = 0; i < _allButtons.Length; i++)
            {
                if (_allButtons[i] != null)
                {
                    _buttonInitialPosition[i] = _allButtons[i].transform.localPosition;
                    
                    // キャンバスグループがなかったら付ける
                    if (!_allButtons[i].TryGetComponent(out _buttonCanvasGroups[i]))
                    {
                        _buttonCanvasGroups[i] = _allButtons[i].gameObject.AddComponent<CanvasGroup>();
                    }
                }
            }
        }

        /// <summary>
        /// 初期状態の設定
        /// </summary>
        private void SetInitialState()
        {
            // セレクター全体を非表示
            if (_selectorCanvasGroup != null)
            {
                _selectorCanvasGroup.alpha = 0f;
            }
            
            // キャラクタープレビューを非表示
            if (_characterCanvasGroup != null)
            {
                _characterCanvasGroup.alpha = 0f;
                _characterInitialPosition = _characterCanvasGroup.transform.localPosition;
                _characterCanvasGroup.transform.localPosition = _characterInitialPosition + Vector3.left * 200f;
                _characterCanvasGroup.transform.localScale = Vector3.one * 0.8f;
            }
            
            // ボタンを左側に移動させて非表示
            for (int i = 0; i < _allButtons.Length; i++)
            {
                if (_allButtons[i] != null)
                {
                    // 左側にずらす
                    var buttonTransform = _allButtons[i].transform;
                    buttonTransform.localPosition = _buttonInitialPosition[i] + Vector3.left * 100f;
                    buttonTransform.localScale = _buttonInitialScale * 0.8f;
                    
                    // キャンバスグループ非表示
                    _buttonCanvasGroups[i].alpha = 0f;
                }
            }
            
            // エフェクトを非表示
            if (_selectorEffect != null)
            {
                // 透明度0にする
                _selectorEffect.Hide();
                _selectorEffect.transform.localScale = Vector3.one * 1.2f;
            }

            if (_glowEffect != null)
            {
                // アルファ値を保存しておく
                _glowDefaultIntensity = _glowEffect.color.a;
                _glowEffect.Hide();
            }
        }
        
        #endregion
        
        /// <summary>
        /// エントランスアニメーション
        /// </summary>
        private void PlayEntranceAnimation()
        {
            var sequence = DOTween.Sequence();
            
            // キャラクタープレビューのフェードイン
            if (_characterCanvasGroup != null)
            {
                sequence.Append(_characterCanvasGroup.DOFade(1f, _entranceDuration).SetEase(Ease.OutCubic));
                sequence.Join(_characterCanvasGroup.transform.DOLocalMove(_characterInitialPosition, _entranceDuration).SetEase(Ease.OutCubic));
                sequence.Join(_characterCanvasGroup.transform.DOScale(Vector3.one, _entranceDuration).SetEase(Ease.OutBack));
            }
            
            // セレクター全体のフェードイン
            if (_selectorCanvasGroup != null)
            {
                sequence.Join(_selectorCanvasGroup.DOFade(1f, _entranceDuration).SetEase(Ease.OutCubic));
            }
            
            // エフェクトのフェードイン
            if (_selectorEffect != null)
            {
                var effectCanvasGroup = _selectorEffect.GetComponent<CanvasGroup>();
                if (effectCanvasGroup != null)
                {
                    sequence.Join(effectCanvasGroup.DOFade(0.8f, _entranceDuration).SetEase(Ease.OutCubic));
                    sequence.Join(_selectorEffect.transform.DOScale(Vector3.one, _entranceDuration).SetEase(Ease.OutBack));
                }
            }
            
            // ボタンを順番にアニメーション
            for (int i = 0; i < _allButtons.Length; i++)
            {
                if (_allButtons[i] == null) continue;
                
                int index = i;
                var button = _allButtons[index];
                var buttonCanvasGroup = button.GetComponent<CanvasGroup>();
                
                // 遅延時間を計算
                float delay = _buttonInterval * index * 0.5f;
                
                // フェードイン
                sequence.Insert(delay, buttonCanvasGroup.DOFade(1f, _entranceDuration * 0.4f).SetEase(Ease.OutCubic));
                
                // スライドイン（左から）
                sequence.Insert(delay, button.transform.DOLocalMove(_buttonInitialPosition[index], _entranceDuration * 0.6f)
                    .SetEase(Ease.OutBack));
                
                // スケールアニメーション
                sequence.Insert(delay, button.transform.DOScale(_buttonInitialScale, _entranceDuration * 0.6f)
                    .SetEase(Ease.OutBack));
            }
            
            // アニメーション完了後の処理
            sequence.OnComplete(() =>
            {
                _isAnimating = false;
                StartBreathAnimation();
                StartEffectPulseAnimation();
            });
            
            sequence.Play();
        }

        /// <summary>
        /// 呼吸アニメーション
        /// </summary>
        private void StartBreathAnimation()
        {
            if (_characterPreview == null)
            {
                // キャラクタープレビューがなければ処理を行う必要はないので早期リターン
                return;
            }
            
            // 既に動いているTweenがあったらキル
            _breathTween?.Kill();
            
            var originalScale = _characterPreview.transform.localScale;
            var breathScale = originalScale * (1f + _breathIntensity);
            
            _breathTween = _characterPreview.transform.DOScale(breathScale, _breathDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            
            _breathTween.Play();
        }

        /// <summary>
        /// セレクターのUIにかけているエフェクトのパルスアニメーション開始
        /// </summary>
        private void StartEffectPulseAnimation()
        {
            if (_selectorEffect == null)
            {
                // 設定されていなければ処理を行う必要はないので早期リターン
                return;
            }
            
            // 既にTweenが登録されていたら一度キル
            _effectPulseTween?.Kill();
            
            _effectPulseTween = _selectorEffect.DOFade(0.3f, _effectPulseDuration * 0.5f).SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
 
            _effectPulseTween.Play();
        }

        /// <summary>
        /// 退場アニメーション
        /// </summary>
        private Tween PlayExitAnimation()
        {
            _isAnimating = true;
            
            // 継続アニメーション停止
            _breathTween?.Kill();
            _effectPulseTween?.Kill();
            
            var sequence = DOTween.Sequence();
            
            // ボタンを逆順でフェードアウト
            for (int i = _allButtons.Length - 1; i >= 0; i--)
            {
                if (_allButtons[i] == null) continue;
                
                var button = _allButtons[i];
                float delay = (_allButtons.Length - 1 - i) * _buttonInterval * 0.1f;
                
                sequence.Insert(delay, _buttonCanvasGroups[i].DOFade(0f, 0.1f).SetEase(Ease.OutCubic));
                sequence.Insert(delay, button.transform.DOLocalMoveX(_buttonInitialPosition[i].x - 200f, 0.1f)
                    .SetEase(Ease.InBack));
            }
            
            // エフェクトとキャラクタープレビューをフェードアウト
            if (_selectorEffect != null)
            {
                sequence.Join(_selectorEffect.DOFade(0f, 0.1f).SetEase(Ease.OutCubic));
            }
            
            if (_characterCanvasGroup != null)
            {
                sequence.Append(_characterCanvasGroup.DOFade(0f, 0.1f).SetEase(Ease.OutCubic));
                sequence.Join(_characterCanvasGroup.transform.DOLocalMove(_characterInitialPosition + Vector3.left * 100f, 0.1f).SetEase(Ease.OutCubic));
                sequence.Join(_characterCanvasGroup.transform.DOScale(Vector3.one * 0.8f, 0.1f).SetEase(Ease.InBack));
            }
            
            // セレクター全体をフェードアウト
            if (_selectorCanvasGroup != null)
            {
                sequence.Join(_selectorCanvasGroup.DOFade(0f, 0.1f).SetEase(Ease.OutCubic));
            }
            
            return sequence;
        }

        #region ボタンのエフェクト・アニメーション

        /// <summary>
        /// ホバーエフェクトの設定
        /// </summary>
        private void SetupHoverEffects()
        {
            for (int i = 0; i < _allButtons.Length; i++)
            {
                var button = _allButtons[i];
                if (button == null) continue;
                
                // EventTriggerを追加
                var eventTrigger = button.GetComponent<EventTrigger>();
                if (eventTrigger == null)
                {
                    eventTrigger = button.gameObject.AddComponent<EventTrigger>();
                }

                // ホバー開始
                var pointerEnter = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerEnter
                };
                pointerEnter.callback.AddListener(_ => OnButtonHover(button, true));
                eventTrigger.triggers.Add(pointerEnter);

                // ホバー終了
                var pointerExit = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerExit
                };
                pointerExit.callback.AddListener(_ => OnButtonHover(button, false));
                eventTrigger.triggers.Add(pointerExit);
            }
        }

        /// <summary>
        /// ボタンホバー時の処理
        /// </summary>
        private void OnButtonHover(Button button, bool isHovering)
        {
            if (button == null || _isAnimating) return;
            
            button.transform.DOKill();
            
            Vector3 targetScale = _buttonInitialScale;
            
            if (isHovering)
            {
                targetScale *= 1.1f;
                
                // ホバー時のパンチエフェクト
                var punchSequence = DOTween.Sequence();
                punchSequence.Append(button.transform.DOScale(targetScale, _hoverDuration).SetEase(Ease.OutCubic));
                punchSequence.OnComplete(() =>
                {
                    // パネルを開くものはその処理を行う
                    if (button == _idea)
                    {
                        _onIdea?.Invoke();
                    }
                    else if(button == _item)
                    {
                        _onItem?.Invoke();
                    }
                });
                punchSequence.Play();
                
                // ホバー時のグローエフェクトの座標移動・表示させる
                _glowEffect.transform.position = button.transform.position;
                _glowEffect.DOFade(_glowDefaultIntensity, 0.2f).SetEase(Ease.OutCubic);

                
            }
            else
            {
                button.transform.DOScale(targetScale, _hoverDuration).SetEase(Ease.OutCubic);
                
                // グロー効果を非表示
                _glowEffect.DOFade(0f, 0.2f).SetEase(Ease.OutCubic);
            }
        }

        /// <summary>
        /// ボタンクリック時の処理
        /// </summary>
        private void OnButtonClicked(Button clickedButton, Action callback)
        {
            if (_isAnimating)
            {
                return;
            }
            
            // クリックエフェクト
            if (clickedButton != null)
            {
                var clickSequence = DOTween.Sequence();
                clickSequence.Append(clickedButton.transform.DOScale(_buttonInitialScale * 0.9f, 0.1f).SetEase(Ease.OutCubic));
                clickSequence.Append(clickedButton.transform.DOScale(_buttonInitialScale, 0.2f).SetEase(Ease.OutBack));
                clickSequence.Play();
            }

            if (clickedButton == _idea || clickedButton == _item)
            {
                // スキルかアイテムの場合はパネルを非表示にしたくないのですぐにコールバックを呼ぶ
                _isAnimating = false;
                callback?.Invoke();
                return;
            }

            // 退場演出を再生
            PlayExitAnimation().OnComplete(() =>
            {
                _isAnimating = false;
                callback?.Invoke();
            });
        }
        
        #endregion
        
        #endregion
    }
}