using System;
using CryStar.Utility;
using CryStar.Utility.Enum;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CryStar.CommandBattle.Data
{
    /// <summary>
    /// バトル時に使用するキャラクターデータ
    /// </summary>
    public class BattleUnitData
    {
        public CharacterStatus UserData { get; set; }
        public int CurrentHp { get; set; }
        public int CurrentWill { get; set; }
        public int CurrentSp { get; set; }
        public int Attack { get; set; }
        public float SkillMultiplier { get; set; }
        public int Defense { get; set; }
        public int SkillDefense { get; set; }
        public int Speed { get; set; }
        public int DodgeSpeed { get; set; }
        public int ArmorPenetration { get; set; }
        
        /// <summary>
        /// ガード中
        /// </summary>
        public bool IsGuarding { get; set; }
        
        /// <summary>
        /// 名前
        /// </summary>
        public string Name => UserData.Name;
        
        /// <summary>
        /// 生存しているか
        /// </summary>
        public bool IsAlive => CurrentHp > 0;
        
        /// <summary>
        /// HP変動を通知するコールバック
        /// </summary>
        public event Action<int, int, int> OnHpChanged;
        
        /// <summary>
        /// スキルポイント変動を通知するコールバック
        /// </summary>
        public event Action<int, int> OnSpChanged;

        /// <summary>
        /// 死亡通知
        /// </summary>
        public event Action OnDeath;

        /// <summary>
        /// 回避成功
        /// </summary>
        public event Action OnSuccessDodge;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">キャラクターID</param>
        public BattleUnitData(int id)
        {
            // TODO: idを元にデータを検索

            UserData = new CharacterStatus(id);
            
            if (UserData == null)
            {
                LogUtility.Error($"{id} のBattleUnitUserDataがありません", LogCategory.Gameplay);
                return;
            }
            
            CurrentHp = UserData.CurrentHp;
            CurrentWill = UserData.StatusResistance;
            CurrentSp = UserData.CurrentSp;
            Attack = UserData.Attack;
            SkillMultiplier = UserData.SkillMultiplier;
            Defense = UserData.Defense;
            Speed = UserData.Speed;
            DodgeSpeed = UserData.DodgeSpeed;
            ArmorPenetration = UserData.ArmorPenetration;
        }

        /// <summary>
        /// ダメージを受ける
        /// </summary>
        public void TakeDamage(int damage)
        {
            if (TryDodge())
            {
                // 回避成功したらイベント発火してリターン
                OnSuccessDodge?.Invoke();
                LogUtility.Info($"{Name} 回避成功！", LogCategory.Gameplay);
                return;
            }
            
            // 最小値は0、最大値はMaxHPにおさまるように調整
            var value = Mathf.Max(0, CurrentHp - damage);
            CurrentHp = Mathf.Min(value, UserData.MaxHp);
            OnHpChanged?.Invoke(CurrentHp, UserData.MaxHp, damage);

            if (CurrentHp <= 0)
            {
                // 現在HPが0以下になったときには死亡イベントを呼び出す
                OnDeath?.Invoke();
            }
        }

        /// <summary>
        /// SPを消費する
        /// </summary>
        public void ConsumedSp(int amount)
        {
            // 最小値は0、最大値はMaxSPにおさまるように調整
            var value = Mathf.Max(0, CurrentSp - amount);
            CurrentHp = Mathf.Min(value, UserData.MaxSp);
            OnSpChanged?.Invoke(CurrentSp, UserData.MaxSp);
        }

        /// <summary>
        /// 回避チェック
        /// </summary>
        private bool TryDodge()
        {
            // 乱数生成
            float randomFactor = Random.Range(0f, 100f);
            // 生成した乱数が回避速度以下なら成功
            return randomFactor <= (DodgeSpeed + 100) * 0.01f;
        }

        /// <summary>
        /// 状態異常チェック
        /// </summary>
        private bool CheckAbnormalCondition()
        {
            float randomFactor = Random.Range(0f, 100f);
            return randomFactor <= (CurrentWill + 100) * 0.01f;
        }
    }
}
