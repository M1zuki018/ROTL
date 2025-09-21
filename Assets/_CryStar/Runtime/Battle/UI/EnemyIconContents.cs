using CryStar.Utility;
using CryStar.Utility.Enum;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CryStar.CommandBattle.UI
{
    /// <summary>
    /// バトルの敵キャラクターのUIを管理するクラス
    /// </summary>
    public class EnemyIconContents : MonoBehaviour
    {
        /// <summary>
        /// キャラクターアイコン
        /// </summary>
        [SerializeField] 
        private CustomImage _icon;
        
        /// <summary>
        /// ダメージテキストの表示位置
        /// </summary>
        [SerializeField]
        private Vector3 _viewPosition = Vector3.one;
        
        /// <summary>
        /// 浮遊アニメーションの時間
        /// </summary>
        [Header("アニメーションの設定")] [SerializeField]
        private float _floatDuration = 2.0f;
        
        /// <summary>
        /// 浮遊アニメーションの振幅
        /// </summary>
        [SerializeField]
        private float _floatAmplitude = 10f;
        
        /// <summary>
        /// ダメージテキストのオブジェクトプールの参照
        /// </summary>
        private DamageTextPool _damageTextPool;

        /// <summary>
        /// 浮遊アニメーション
        /// </summary>
        private Tween _floatTween;
        
        /// <summary>
        /// 初期位置
        /// </summary>
        private Vector3 _initialPosition;

        private void Awake()
        {
            _initialPosition = transform.localPosition;
        }
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(DamageTextPool damageTextPool)
        {
            _damageTextPool = damageTextPool;
            
            // 浮遊アニメーションを開始
            StartFloatingAnimation();
        }
        
        /// <summary>
        /// アイコンを差し替える
        /// </summary>
        public async UniTask SetIcon(string spritePath)
        {
            await _icon.ChangeSpriteAsync(spritePath);
        }
        
        /// <summary>
        /// ダメージ量のテキストを表示する
        /// </summary>
        public async UniTask SetDamageText(int value)
        {
            if (_damageTextPool == null)
            {
                LogUtility.Warning("DamageTextPool が null です", LogCategory.UI, this);
                return;
            }

            // ダメージ量のテキストオブジェクトをオブジェクトプールから取得
            var damageText = _damageTextPool.Get();
            
            // 位置を調整し表示を変更
            damageText.rectTransform.localPosition = _viewPosition;
            damageText.SetText(value.ToString());
            
            await UniTask.Delay(500); // TODO: 仮置き。ここでアニメーションをする

            _damageTextPool.Release(damageText);
        }
        
        /// <summary>
        /// アイコンを非表示にする
        /// </summary>
        public void Hide()
        {
            _icon.enabled = false;
        }
        
        /// <summary>
        /// キャラクターの浮遊アニメーション開始（ずっと繰り返す）
        /// </summary>
        private void StartFloatingAnimation()
        {
            // 既存のシーケンスがあればキル
            _floatTween?.Kill();
            
            _floatTween = transform.DOLocalMoveY(
                    _initialPosition.y + _floatAmplitude, _floatDuration
                )
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}
