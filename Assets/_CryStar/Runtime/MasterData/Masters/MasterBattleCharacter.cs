using System.Collections.Generic;
using CryStar.Utility;
using UnityEngine;

namespace CryStar.MasterData
{
    /// <summary>
    /// バトルで使用する形式のキャラクターマスターデータ
    /// </summary>
    public class MasterBattleCharacter : AddressableJsonMasterBase<int, BattleCharacterInfo>
    {
        public override LoadPriority Priority => LoadPriority.Resident;
        protected override string AddressOrLabel => MasterDataAddresses.CHARACTER_BATTLE;
        
        protected override void LoadFromJson(string json)
        {
            var charJson = JsonUtility.FromJson<BattleCharacterJson>(json);
            
            // Jsonから読み込むことができたデータで辞書を作成
            _data = new Dictionary<int, BattleCharacterInfo>(charJson.characters.Count);
            foreach (var character in charJson.characters)
            {
                _data[character.id] = character;
            }
    
            LogUtility.Verbose($"[{typeof(MasterBattleCharacter)}] Loaded {_data.Count} characters");
        }
        
        /// <summary>
        /// キャラクター名
        /// </summary>
        public static string GetName(int id)
        {
            var master = MasterDataManager.Instance.Get<MasterBattleCharacter>();
            return master?.Get(id)?.name;
        }
    
        /// <summary>
        /// キャラクターカラー
        /// </summary>
        public static Color GetColor(int id)
        {
            var master = MasterDataManager.Instance.Get<MasterBattleCharacter>();
            return master?.Get(id)?.characterColor ?? Color.white;
        }
    
        /// <summary>
        /// アイコン素材のAddressableのPath
        /// </summary>
        public static string GetIconPath(int id)
        {
            var master = MasterDataManager.Instance.Get<MasterBattleCharacter>();
            return master?.Get(id)?.iconPath;
        }
    }   
}