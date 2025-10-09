using System.Collections.Generic;
using CryStar.Utility;
using UnityEngine;

namespace CryStar.MasterData
{
    /// <summary>
    /// キャラクターの育成データマスター
    /// </summary>
    public class MasterGrowthCharacter : AddressableLabelMaster<int, CharacterGrowthData>
    {
        public override LoadPriority Priority => LoadPriority.Cached;
        protected override string Label => MasterDataAddresses.CHARACTER_GROWTH;

        protected override void LoadFromJson(string json)
        {
            var growthJson = JsonUtility.FromJson<CharacterGrowthJson>(json);
            _data = new Dictionary<int, CharacterGrowthData>(growthJson.characters.Count);

            foreach (var character in growthJson.characters)
            {
                _data[character.characterId] = character;
            }

            LogUtility.Verbose($"[{typeof(MasterBattleCharacter)}] Loaded {_data.Count} characters");
        }

        /// <summary>
        /// HPを取得する
        /// データが取得できなかった場合は、すぐに死亡しないように0ではなく1を返却
        /// </summary>
        public static int GetHp(int characterId, int level)
        {
            var master = MasterDataManager.Instance.Get<MasterGrowthCharacter>();
            var growthData = master._data[characterId];
            return growthData != null ? growthData.hp[level] : 1;
        }

        /// <summary>
        /// SPを取得する
        /// データが取得できなかった場合は0を返却
        /// </summary>
        public static int GetSp(int characterId, int level)
        {
            var master = MasterDataManager.Instance.Get<MasterGrowthCharacter>();
            var growthData = master._data[characterId];
            return growthData != null ? growthData.sp[level] : 0;
        }

        /// <summary>
        /// Attack 攻撃力を取得する
        /// データが取得できなかった場合は1を返却
        /// </summary>
        public static int GetAttack(int characterId, int level)
        {
            var master = MasterDataManager.Instance.Get<MasterGrowthCharacter>();
            var growthData = master._data[characterId];
            return growthData != null ? growthData.attack[level] : 1;
        }

        /// <summary>
        /// 防御力を取得する
        /// データが取得できなかった場合は0を返却
        /// </summary>
        public static int GetDefense(int characterId, int level)
        {
            var master = MasterDataManager.Instance.Get<MasterGrowthCharacter>();
            var growthData = master._data[characterId];
            return growthData != null ? growthData.defense[level] : 0;
        }

        /// <summary>
        /// スキル倍率を取得する
        /// データが取得できなかった場合は1.0を返却
        /// </summary>
        public static float GetSkillMultiplier(int characterId, int level)
        {
            var master = MasterDataManager.Instance.Get<MasterGrowthCharacter>();
            var growthData = master._data[characterId];
            return growthData != null ? growthData.skillMultiplier[level] : 1.0f;
        }

        /// <summary>
        /// 状態異常耐性を取得する
        /// データが取得できなかった場合は0を返却
        /// </summary>
        public static int GetStatusResistance(int characterId, int level)
        {
            var master = MasterDataManager.Instance.Get<MasterGrowthCharacter>();
            var growthData = master._data[characterId];
            return growthData != null ? growthData.statusResistance[level] : 0;
        }

        /// <summary>
        /// 攻撃速度を取得する
        /// データが取得できなかった場合は0を返却
        /// </summary>
        public static int GetSpeed(int characterId, int level)
        {
            var master = MasterDataManager.Instance.Get<MasterGrowthCharacter>();
            var growthData = master._data[characterId];
            return growthData != null ? growthData.speed[level] : 0;
        }

        /// <summary>
        /// 回避速度を取得する
        /// データが取得できなかった場合は0を返却
        /// </summary>
        public static int GetDodgeSpeed(int characterId, int level)
        {
            var master = MasterDataManager.Instance.Get<MasterGrowthCharacter>();
            var growthData = master._data[characterId];
            return growthData != null ? growthData.dodgeSpeed[level] : 0;
        }

        /// <summary>
        /// 防御無視を取得する
        /// データが取得できなかった場合は0を返却
        /// </summary>
        public static int GetArmorPenetration(int characterId, int level)
        {
            var master = MasterDataManager.Instance.Get<MasterGrowthCharacter>();
            var growthData = master._data[characterId];
            return growthData != null ? growthData.armorPenetration[level] : 0;
        }

        /// <summary>
        /// クリティカル率を取得する
        /// データが取得できなかった場合は0を返却
        /// </summary>
        public static int GetCriticalRate(int characterId, int level)
        {
            var master = MasterDataManager.Instance.Get<MasterGrowthCharacter>();
            var growthData = master._data[characterId];
            return growthData != null ? growthData.criticalRate[level] : 0;
        }

        /// <summary>
        /// クリティカルダメージを取得する
        /// データが取得できなかった場合は100を返却（等倍）
        /// </summary>
        public static int GetCriticalDamage(int characterId, int level)
        {
            var master = MasterDataManager.Instance.Get<MasterGrowthCharacter>();
            var growthData = master._data[characterId];
            return growthData != null ? growthData.criticalDamage[level] : 100;
        }

        /// <summary>
        /// 登録されているキャラクター数を取得する
        /// </summary>
        public static int RegisteredCharacterCount()
        {
            var master = MasterDataManager.Instance.Get<MasterGrowthCharacter>();
            return master._data.Count;
        }
    }
}