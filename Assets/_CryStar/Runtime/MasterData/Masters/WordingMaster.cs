using System.Collections.Generic;
using UnityEngine;

namespace CryStar.MasterData
{
    /// <summary>
    /// 文言キーのマスターデータ
    /// </summary>
    public class WordingMaster : AddressableJsonMasterBase<string, string>
    {
        public override LoadPriority Priority => LoadPriority.Resident;
        protected override string AddressOrLabel => MasterDataAddresses.WORDING;

        protected override void LoadFromJson(string json)
        {
            var wordingData = JsonUtility.FromJson<WordingData>(json);
            _data = new Dictionary<string, string>(wordingData.data.Count);

            foreach (var entry in wordingData.data)
            {
                _data[entry.key] = entry.value;
            }

            Debug.Log($"[WordingMaster] Loaded {_data.Count} entries (version: {wordingData.version})");
        }

        /// <summary>
        /// 文字列を取得する
        /// </summary>
        public static string GetText(string key)
        {
            var master = MasterDataManager.Instance.Get<WordingMaster>();
            return master?.Get(key) ?? $"[{key}]";
        }

        /// <summary>
        /// フォーマット形式の文字列を取得する
        /// </summary>
        public static string GetFormat(string key, params object[] args)
        {
            return string.Format(GetText(key), args);
        }
    }
}