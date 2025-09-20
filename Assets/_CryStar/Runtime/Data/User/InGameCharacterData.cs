using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryStar.Data.User
{
    /// <summary>
    /// フィールド、バトルで利用するキャラクターデータ
    /// </summary>
    [Serializable]
    public class InGameCharacterData
    {
        [SerializeField] private int _characterID;
        
        /// <summary>
        /// キャラクターのID
        /// </summary>
        public int CharacterID => _characterID;

        /// <summary>
        /// レベル
        /// </summary>
        public int Level = 1;
        
        /// <summary>
        /// 経験値
        /// </summary>
        public int Experience = 0;
        
        // フィールドでの現在値（持ち越し用）
        // NOTE: -1は未設定
        
        /// <summary>
        /// 現在のHPの減少量
        /// </summary>
        public int DecreaseHp = 0;
        
        /// <summary>
        /// 現在のSPの減少量
        /// </summary>
        public int DecreaseSp = 0;
        
        // 装備やアイテムによる補正値
        
        /// <summary>
        /// HP補正値
        /// </summary>
        public int BonusHp = 0;
        
        /// <summary>
        /// SP補正値
        /// </summary>
        public int BonusSp = 0;
        
        /// <summary>
        /// 攻撃力補正値
        /// </summary>
        public int BonusAttack = 0;
        
        /// <summary>
        /// 防御力補正値
        /// </summary>
        public int BonusDefense = 0;

        /// <summary>
        /// スキル倍率（float）
        /// </summary>
        public float BonusSkillMultiplier = 0f;
        
        /// <summary>
        /// 状態異常耐性
        /// </summary>
        public int BonusStatusResistance = 0;

        /// <summary>
        /// 攻撃速度
        /// </summary>
        public int BonusSpeed = 0;
        
        /// <summary>
        /// 回避速度
        /// </summary>
        public int BonusDodgeSpeed = 0;
        
        /// <summary>
        /// 防御無視
        /// </summary>
        public int BonusArmorPenetration = 0;

        /// <summary>
        /// クリティカル率
        /// </summary>
        public int BonusCriticalRate = 0;
        
        /// <summary>
        /// クリティカルダメージ
        /// </summary>
        public int BonusCriticalDamage = 0;
        
        /// <summary>
        /// IdeaのIDリスト
        /// </summary>
        public List<int> IdeaIdList = new List<int>();
        
        // TODO: 装備データの変数も作成する

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InGameCharacterData(int characterID)
        {
            _characterID = characterID;
        }
    }
}