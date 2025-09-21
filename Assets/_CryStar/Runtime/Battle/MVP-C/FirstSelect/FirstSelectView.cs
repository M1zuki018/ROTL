using System;
using System.Collections.Generic;
using CryStar.Attribute;
using CryStar.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
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
        /// デフォルトのテキスト色
        /// </summary>
        [SerializeField]
        private Color _defaultTextColor = Color.white;
        
        /// <summary>
        /// 選択中のテキストの色
        /// </summary>
        [SerializeField]
        private Color _selectedTextColor = Color.black;

        [Header("アニメーション設定")]
        [SerializeField] private float _appearDelay = 0.1f;
        [SerializeField] private float _appearDuration = 0.6f;
        [SerializeField] private float _selectScaleAmount = 1.1f;
        [SerializeField] private float _selectDuration = 0.2f;
        [SerializeField] private float _bounceAmount = 0.05f;
        [SerializeField] private float _pulseDuration = 2.0f;
        [SerializeField] private float _fillDuration = 0.4f;
        [SerializeField] private float _submitDuration = 0.15f;
        
        /// <summary>
        /// 現在選択中のボタン
        /// </summary>
        private CustomButton _currentSelectedButton;
        
        /// <summary>
        /// ボタンのリスト（管理用）
        /// </summary>
        private List<CustomButton> _buttons;
        
        /// <summary>
        /// ボタンの初期スケール
        /// </summary>
        private Vector3 _originalScale;

        /// <summary>
        /// 現在実行中のTweenを管理
        /// </summary>
        private Dictionary<CustomButton, Sequence> _activeTweens = new Dictionary<CustomButton, Sequence>();
        
        /// <summary>
        /// 選択中のImageの参照を管理
        /// </summary>
        private Dictionary<CustomButton, Image> _overlayMapping = new Dictionary<CustomButton, Image>();
        
        /// <summary>
        /// 各ボタンのCanvasGroup
        /// </summary>
        private List<CanvasGroup> _canvasGroups = new List<CanvasGroup>();
            
        #region Life cycle

        private void Awake()
        {
            // ボタンリストを初期化
            _buttons = new List<CustomButton> { _battle, _organization, _escape };

            // ボタンのデフォルトのスケールを保存
            _originalScale = _buttons[0].transform.localScale;
            
            for (int i = 0; i < _buttons.Count; i++)
            {
                var button = _buttons[i];
                
                // キャンバスグループを追加もしくは取得
                var canvasGroup = button.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    _canvasGroups.Add(button.gameObject.AddComponent<CanvasGroup>());
                }
                
                // 各ボタンの最初の子オブジェクトから選択画像を取得
                if (button.transform.childCount > 0)
                {
                    var selectedImage = button.transform.GetChild(0).GetComponent<Image>();
                    if (selectedImage != null)
                    {
                        // 対応するボタンとのkvpで辞書に登録
                        _overlayMapping[button] = selectedImage;
                        
                        // 初期状態は非表示になるようにしておく 
                        selectedImage.fillAmount = 0f;
                    }
                }
            }

            // マウスのホバーイベントを登録する
            EventTriggerResister();
        }

        private void OnDestroy()
        {
            // 全てのTweenを停止
            foreach (var tween in _activeTweens.Values)
            {
                tween?.Kill();
            }
        }

        #endregion
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(Action startAction, Action organizationAction, Action escapeAction)
        {
            // イベント登録
            _battle.onClick.SafeReplaceListener(() => PlayClickAnimation(_battle, () => startAction?.Invoke()));
            _organization.onClick.SafeReplaceListener(() => PlayClickAnimation(_organization, () => organizationAction?.Invoke()));
            _escape.onClick.SafeReplaceListener(() => PlayClickAnimation(_escape, () => escapeAction?.Invoke()));
            
            // ビューのリセットを行う
            ViewReset();
            
            // 登場アニメーションを再生
            PlayAppearAnimation();
        }

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            // 全てのTweenを停止
            foreach (var tween in _activeTweens.Values)
            {
                tween?.Kill();
            }

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
        /// 登場アニメーションを再生
        /// </summary>
        private void PlayAppearAnimation()
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                var button = _buttons[i];
                var canvasGroup = _canvasGroups[i];
                
                // 初期状態を設定
                button.transform.localScale = Vector3.zero;
                canvasGroup.alpha = 0f;

                // 遅延付きで登場アニメーションを行いたいため待つ時間を計算
                float delay = i * _appearDelay;
                
                // シーケンス作成
                var sequence = DOTween.Sequence();
                sequence.SetDelay(delay);
                
                // スケールと透明度を同時にアニメーション
                sequence.Append(button.transform.DOScale(_originalScale * 1.2f, _appearDuration * 0.7f)
                    .SetEase(Ease.OutBack));
                sequence.Join(canvasGroup.DOFade(1f, _appearDuration * 0.5f)
                    .SetEase(Ease.OutQuad));
                
                // 元のサイズに
                sequence.Append(button.transform.DOScale(_originalScale, _appearDuration * 0.3f)
                    .SetEase(Ease.OutQuad));

                _activeTweens[button] = sequence;
            }

            // アニメーションが完了したら最初のボタンを選択状態にする
            DOVirtual.DelayedCall(_buttons.Count * _appearDelay + _appearDuration * 0.7f, () => SelectButton(_battle));
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

            // 前の選択を解除する
            if (_currentSelectedButton != null)
            {
                DeselectButton(_currentSelectedButton);
            }
            
            // 新しいボタンを選択状態にする
            _currentSelectedButton = selectedButton;
            if (_currentSelectedButton != null)
            {
                SelectButtonWithAnimation(_currentSelectedButton);
            }
        }

        /// <summary>
        /// ボタンの選択解除アニメーション
        /// </summary>
        private void DeselectButton(CustomButton button)
        {
            // 前のTweenを停止
            _activeTweens[button]?.Kill(true);

            // テキスト色を元に戻す
            button.ChangeTextColor(_defaultTextColor);

            var overlay = _overlayMapping[button];
            var sequence = DOTween.Sequence();

            // オーバーレイのFillを0にする
            if (overlay != null)
            {
                sequence.Append(overlay.DOFillAmount(0f, _fillDuration * 0.5f)
                    .SetEase(Ease.OutQuad));
            }

            // スケールを元に戻す
            sequence.Join(button.transform.DOScale(_originalScale, _selectDuration)
                .SetEase(Ease.OutQuad));

            _activeTweens[button] = sequence;
        }

        /// <summary>
        /// ボタンの選択アニメーション
        /// </summary>
        private void SelectButtonWithAnimation(CustomButton button)
        {
            // 前のTweenを停止
            // NOTE: ボタンの登場中に選択されるとアニメーションが中断されてCanvasGroupなどが
            // 意図しない挙動で止まってしまうので、引数にtrueを渡して演出スキップという形にする
            _activeTweens[button]?.Kill(true);

            // テキスト色を変更
            button.ChangeTextColor(_selectedTextColor);

            var targetScale = _originalScale * _selectScaleAmount;
            var overlay = _overlayMapping[button];
            
            var sequence = DOTween.Sequence();
            
            // 少し大きくする
            sequence.Append(button.transform.DOScale(targetScale, _selectDuration)
                .SetEase(Ease.OutBack, 1.2f));
                
            // オーバーレイのFillを操作
            if (overlay != null)
            {
                sequence.Join(overlay.DOFillAmount(1f, _fillDuration)
                    .SetDelay(_selectDuration * 0.3f)
                    .SetEase(Ease.OutCirc));
            }
            
            // シーケンスが完了したら継続的な軽いパルスアニメーションを呼び出す
            sequence.AppendCallback(() => StartPulseEffect(button));

            _activeTweens[button] = sequence;
        }

        /// <summary>
        /// 選択中ボタンのパルス効果
        /// </summary>
        private void StartPulseEffect(CustomButton button)
        {
            if (_currentSelectedButton != button)
            {
                // 現在も選択中のボタンであればスキップ
                return;
            }

            var originalScale = _originalScale * _selectScaleAmount;
            var overlay = _overlayMapping[button];

            var pulseSequence = DOTween.Sequence();
            
            // スケールのパルス
            pulseSequence.Append(button.transform.DOScale(originalScale * 1.02f, _pulseDuration * 0.5f)
                .SetEase(Ease.InOutSine));
            pulseSequence.Append(button.transform.DOScale(originalScale, _pulseDuration * 0.5f)
                .SetEase(Ease.InOutSine));
                
            // オーバーレイの軽い透明度変化を行う
            if (overlay != null)
            {
                pulseSequence.Join(overlay.DOFade(0.8f, _pulseDuration * 0.5f)
                    .SetEase(Ease.InOutSine));
                pulseSequence.Join(overlay.DOFade(1f, _pulseDuration * 0.5f)
                    .SetDelay(_pulseDuration * 0.5f)
                    .SetEase(Ease.InOutSine));
            }
            
            pulseSequence.SetLoops(-1, LoopType.Restart);

            _activeTweens[button] = pulseSequence;
        }

        /// <summary>
        /// クリック時のアニメーション
        /// </summary>
        private void PlayClickAnimation(CustomButton button, Action callback)
        {
            // 現在のTweenを停止
            _activeTweens[button]?.Kill();
            
            var sequence = DOTween.Sequence();
            
            // 押し込まれる効果
            sequence.Append(button.transform.DOScale(_originalScale * 0.95f, _submitDuration * 0.5f)
                .SetEase(Ease.InQuad));
            
            // 元に戻りながら少し大きくなる
            sequence.Append(button.transform.DOScale(_originalScale * 1.05f, _submitDuration * 0.3f)
                .SetEase(Ease.OutBack));
            
            // 最終的に元のサイズに
            sequence.Append(button.transform.DOScale(_originalScale, _submitDuration * 0.1f)
                .SetEase(Ease.InQuad));
            
            // コールバック実行
            sequence.AppendCallback(() => callback?.Invoke());

            _activeTweens[button] = sequence;
        }

        /// <summary>
        /// 表示のリセットを行うメソッド
        /// </summary>
        private void ViewReset()
        {
            if (_currentSelectedButton != null)
            {
                // テキストの色を戻す
                _currentSelectedButton.ChangeTextColor(_defaultTextColor);

                // fillを0に戻しておく
                _overlayMapping[_currentSelectedButton].fillAmount = 0f;
            }
            
            // 何も選択していない状態とする
            _currentSelectedButton = null;
        }

        #endregion
    }
}