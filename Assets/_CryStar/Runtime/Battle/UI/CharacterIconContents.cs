using CryStar.CommandBattle.Data;
using CryStar.Utility;
using CryStar.Utility.Enum;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CryStar.CommandBattle.UI
{
    /// <summary>
    /// バトルのキャラクターのUIを管理するクラス
    /// </summary>
    public class CharacterIconContents : MonoBehaviour
    {
        /// <summary>
        /// 背景
        /// </summary>
        [SerializeField] 
        private Image _background;
        
        /// <summary>
        /// キャラクターアイコン
        /// </summary>
        [SerializeField]
        private CustomImage _icon;
        
        /// <summary>
        /// HPバー
        /// </summary>
        [SerializeField]
        private Slider _hpSlider;

        /// <summary>
        /// のこりHP量を示すText
        /// </summary>
        [SerializeField] 
        private CustomText _hpAmount;
        
        /// <summary>
        /// 最大HP量を示すText
        /// </summary>
        [SerializeField]
        private CustomText _hpMaxValue;
        
        /// <summary>
        /// スキルポイントバー
        /// </summary>
        [SerializeField]
        private Slider _spSlider;
        
        /// <summary>
        /// のこりスキルポイント量を示すText
        /// </summary>
        [SerializeField]
        private CustomText _spAmount;
        
        /// <summary>
        /// 最大SP量を示すText
        /// </summary>
        [SerializeField]
        private CustomText _spMaxValue;
        
        /// <summary>
        /// ダメージテキストのオブジェクトプールの参照
        /// </summary>
        private DamageTextPool _damageTextPool;

        /// <summary>
        /// 自身のRectTransform
        /// </summary>
        private RectTransform RectTransform => _background.rectTransform;
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(DamageTextPool damageTextPool, BattleUnitData unitData)
        {
            _damageTextPool = damageTextPool;
            
            // Viewの初期化
            SetHpSlider(unitData.CurrentHp, unitData.UserData.MaxHp);
            SetSpSlider(unitData.CurrentSp, unitData.UserData.MaxSp);
        }

        /// <summary>
        /// アイコンを差し替える
        /// </summary>
        public async UniTask SetIcon(string spritePath)
        {
            await _icon.ChangeSpriteAsync(spritePath);
        }
        
        /// <summary>
        /// HPバーを更新する
        /// </summary>
        public void SetHpSlider(int value, int maxValue)
        {
            if (_hpSlider != null)
            {
                _hpSlider.maxValue = maxValue;
                
                // 0以下にならないようにしてvalueに代入
                _hpSlider.value = Mathf.Max(value, 0);
                
                // テキスト表示も更新
                _hpAmount.text = $"{value}";
                _hpMaxValue.text = $" /{maxValue}"; 
            }
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
            damageText.transform.position = _background.transform.position;
            damageText.SetText(value.ToString());
            
            await UniTask.Delay(200); // TODO: 仮置き。ここでアニメーションをする

            _damageTextPool.Release(damageText);
        }

        /// <summary>
        /// スキルポイントバーを更新する
        /// </summary>
        public void SetSpSlider(int value, int maxValue)
        {
            if (_spSlider != null)
            {
                _spSlider.maxValue = maxValue;
                _spSlider.value = Mathf.Max(value, 0);
                
                // テキスト表示も更新
                _spAmount.text = $"{value}";
                _spMaxValue.text = $" /{maxValue}";
            }
        }

        /// <summary>
        /// アイコンを非表示にする
        /// </summary>
        public void Hide()
        {
            _icon.enabled = false;
        }
    }
}
