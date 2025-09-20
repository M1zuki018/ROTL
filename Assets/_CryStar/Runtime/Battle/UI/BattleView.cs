using System.Collections.Generic;
using CryStar.CommandBattle.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CryStar.CommandBattle.UI
{
    /// <summary>
    /// バトルのベースとなるUIを管理するクラス
    /// </summary>
    public class BattleView : MonoBehaviour
    {
        /// <summary>
        /// キャラクターアイコンのPrefab
        /// TODO: 管理方法は検討中
        /// </summary>
        [SerializeField]
        private CharacterIconContents _unitIconPrefab;
        private List<CharacterIconContents> _icons;
        
        [SerializeField]
        private Transform _unitIconParent;

        /// <summary>
        /// エネミーアイコンのPrefab
        /// </summary>
        [SerializeField] 
        private EnemyIconContents _enemyIconPrefab;
        private List<EnemyIconContents> _enemyIcons;

        [SerializeField] 
        private Transform _enemyIconParent;
         
        /// <summary>
        /// ダメージテキストのオブジェクトプールを管理するクラス
        /// </summary>
        [SerializeField]
        private DamageTextPool _damageTextPool;
        
        /// <summary>
        /// キャラクターのアイコンを設定する
        /// </summary>
        public async UniTask SetupIcons(IReadOnlyList<BattleUnitData> unitData, IReadOnlyList<BattleUnitData> enemyData)
        {
            // リストを新規作成
            _icons = new List<CharacterIconContents>(unitData.Count);
            
            for (int i = 0; i < unitData.Count; i++)
            {
                var icon = Instantiate(_unitIconPrefab, _unitIconParent);
                _icons.Add(icon);
                icon.Setup(_damageTextPool);
                await icon.SetIcon(unitData[i].UserData.IconPath);
            }
            
            SubscribeToUnitEvents(unitData);

            _enemyIcons = new List<EnemyIconContents>(enemyData.Count);
            
            for (int i = 0; i < enemyData.Count; i++)
            {
                var icon = Instantiate(_enemyIconPrefab, _enemyIconParent);
                _enemyIcons.Add(icon);
                icon.Setup(_damageTextPool);
                await icon.SetIcon(enemyData[i].UserData.IconPath);
            }
            
            SubscribeToEnemyEvents(enemyData);
        }
        
        /// <summary>
        /// キャラクターのHP・SP変動アクションを購読する
        /// </summary>
        private void SubscribeToUnitEvents(IReadOnlyList<BattleUnitData> unitData)
        {
            // 味方ユニットのイベント購読
            for (int i = 0; i < unitData.Count; i++)
            {
                var unit = unitData[i];
                var index = i; // ローカル変数でキャプチャ
                
                unit.OnHpChanged += (currentHp, maxHp, damage) => UpdatePlayer(index, currentHp, maxHp, damage);
                //unit.OnSpChanged += (newSp) => UpdatePlayer(index);
                unit.OnDeath += () => DisEnableIcon(index, true);
            }
        }
        
        /// <summary>
        /// エネミーのHP変動アクションを購読する
        /// </summary>
        private void SubscribeToEnemyEvents(IReadOnlyList<BattleUnitData> enemyData)
        {
            // エネミーのイベント購読
            for (int i = 0; i < enemyData.Count; i++)
            {
                var unit = enemyData[i];
                var index = i; // ローカル変数でキャプチャ
                
                unit.OnHpChanged += (currentHp, maxHp, damage) => UpdateEnemy(index, damage);
                unit.OnDeath += () => DisEnableIcon(index, false);
            }
        }

        /// <summary>
        /// ダメージを受けた時のUI更新
        /// </summary>
        private void UpdatePlayer(int index, int value, int maxValue, int damage)
        {
            // HPバーを更新
            _icons[index].SetHpSlider(value, maxValue);
            
            // ダメージ量の表記を行う
            _icons[index].SetDamageText(damage).Forget();
        }

        /// <summary>
        /// 敵がダメージを受けた時のUI更新
        /// </summary>
        private void UpdateEnemy(int index, int damage)
        {
            _enemyIcons[index].SetDamageText(damage).Forget();
        }

        /// <summary>
        /// 死亡時にアイコンを非表示にする
        /// </summary>
        private void DisEnableIcon(int index, bool isPlayer)
        {
            if (isPlayer)
            {
                _icons[index].Hide();
            }
            else
            {
                _enemyIcons[index].Hide();
            }
        }
    }
}
