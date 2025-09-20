using System.Collections.Generic;
using CryStar.Core;
using CryStar.Core.UserData;
using CryStar.Data.User;
using UnityEngine;

namespace CryStar.CommandBattle.Data
{
    /// <summary>
    /// キャラクターのステータス
    /// </summary>
    public class CharacterStatus
    {
        private int _characterID;
        private InGameCharacterData _data;
        private UserDataManager _userDataManager;

        /// <summary>
        /// キャラクターユーザーデータ
        /// </summary>
        private CharacterUserData CharacterUserData => _userDataManager.CurrentUserData.CharacterUserData;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CharacterStatus(int characterID)
        {
            _characterID = characterID;

            _userDataManager = ServiceLocator.GetGlobal<UserDataManager>();
            _data = CharacterUserData.GetCharacterUserData(_characterID);

            // TODO: テスト用。プレイヤー側のキャラクターのレベルだけ変更する
            if (_characterID == 1)
            {
                _data.Level = 50;
            }
        }
        
        /// <summary>
        /// キャラクターID
        /// </summary>
        public int CharacterID => _characterID;

        /// <summary>
        /// 名前
        /// </summary>
        public string Name => MasterBattleCharacter.GetName(_characterID);
        
        /// <summary>
        /// アイコンのPath
        /// </summary>
        public string IconPath => MasterBattleCharacter.GetIconPath(_characterID);
        
        /// <summary>
        /// 最大HP
        /// </summary>
        public int MaxHp => MasterCharacter.GetHp(_characterID, _data.Level) + _data.BonusHp;
        
        /// <summary>
        /// 最大SP
        /// </summary>
        public int MaxSp => MasterCharacter.GetSp(_characterID, _data.Level) + _data.BonusSp;

        /// <summary>
        /// 現在のHP
        /// </summary>
        public int CurrentHp => Mathf.Max(MaxHp - _data.DecreaseHp, 0);
        
        /// <summary>
        /// 現在のSP
        /// </summary>
        public int CurrentSp => Mathf.Max(MaxSp - _data.DecreaseSp, 0);
        
        /// <summary>
        /// 経験値
        /// </summary>
        public int Experience => _data.Experience;
        
        /// <summary>
        /// 攻撃力
        /// </summary>
        public int Attack => MasterCharacter.GetAttack(_characterID, _data.Level) + _data.BonusAttack;
        
        /// <summary>
        /// 防御力
        /// </summary>
        public int Defense => MasterCharacter.GetDefense(_characterID, _data.Level) + _data.BonusDefense;
        
        /// <summary>
        /// スキル倍率
        /// </summary>
        public float SkillMultiplier => MasterCharacter.GetSkillMultiplier(_characterID, _data.Level) + _data.BonusSkillMultiplier;
        
        /// <summary>
        /// 状態異常耐性
        /// </summary>
        public int StatusResistance => MasterCharacter.GetStatusResistance(_characterID, _data.Level) + _data.BonusStatusResistance;
        
        /// <summary>
        /// 攻撃速度
        /// </summary>
        public int Speed => MasterCharacter.GetSpeed(_characterID, _data.Level) + _data.BonusSpeed;
        
        /// <summary>
        /// 回避速度
        /// </summary>
        public int DodgeSpeed => MasterCharacter.GetDodgeSpeed(_characterID, _data.Level) + _data.BonusDodgeSpeed;

        /// <summary>
        /// 防御無視
        /// </summary>
        public int ArmorPenetration => MasterCharacter.GetArmorPenetration(_characterID, _data.Level) +
                                       _data.BonusArmorPenetration;
        
        /// <summary>
        /// クリティカル率
        /// </summary>
        public int CriticalRate => MasterCharacter.GetCriticalRate(_characterID, _data.Level) + _data.BonusCriticalRate;
        
        /// <summary>
        /// クリティカルダメージ
        /// </summary>
        public int CriticalDamage => MasterCharacter.GetCriticalDamage(_characterID, _data.Level) + _data.BonusCriticalDamage;
        
        /// <summary>
        /// IdeaのIDリスト
        /// </summary>
        public List<int> IdeaIdList => _data.IdeaIdList;
    }
}