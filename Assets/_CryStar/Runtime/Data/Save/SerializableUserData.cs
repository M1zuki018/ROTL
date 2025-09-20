using System;
using System.Collections.Generic;
using CryStar.Data.User;

namespace CryStar.Data
{
    /// <summary>
    /// Jsonファイルに変換するためのシリアライズ可能なユーザーデータクラス
    /// </summary>
    [Serializable]
    public class SerializableUserData
    {
        /// <summary>
        /// ユーザーID
        /// </summary>
        public int UserId;
    
        /// <summary>
        /// 最終セーブ時間
        /// </summary>
        public long LastSaveTime;
    
        /// <summary>
        /// フィールドデータ
        /// </summary>
        public FieldUserData FieldData;
    
        /// <summary>
        /// ストーリーデータ
        /// </summary>
        public StoryUserData StoryData;
    
        /// <summary>
        /// バトル・フィールド用のキャラクターデータ
        /// </summary>
        public List<InGameCharacterData> CharacterData = new List<InGameCharacterData>();

        /// <summary>
        /// ゲームイベントデータ
        /// </summary>
        public GameEventUserData GameEventData;
        
        /// <summary>
        /// アイテムイベントリのデータ
        /// </summary>
        public InventoryUserData InventoryData;
    }
}