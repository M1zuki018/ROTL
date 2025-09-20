using System;
using System.Collections.Generic;
using System.IO;
using CryStar.Data;
using CryStar.Data.User;
using CryStar.Utility;
using UnityEngine;

namespace CryStar.Core.SaveData
{
    /// <summary>
    /// JSON保存・読み込みを行うセーブマネージャー
    /// </summary>
    public static class JsonSaveManager
    {
        private static readonly string SaveDirectoryPath = Path.Combine(Application.persistentDataPath, "SaveData");
        private static readonly string SaveFileExtension = ".json";
        
        /// <summary>
        /// UserDataContainerをJSONファイルに保存
        /// </summary>
        public static bool SaveToJson(UserDataContainer userData, int slotIndex)
        {
            try
            {
                // セーブディレクトリが存在しない場合作成する
                EnsureSaveDirectoryExists();
                
                var serializableData = ConvertToSerializable(userData);
                string jsonString = JsonUtility.ToJson(serializableData, true);
                string filePath = GetSaveFilePath(slotIndex);
                
                File.WriteAllText(filePath, jsonString);
                
                LogUtility.Info($"セーブデータをJSONファイルに保存しました: {filePath}");
                return true;
            }
            catch (Exception e)
            {
                LogUtility.Error($"JSONファイル保存に失敗しました: {e.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// JSONファイルからUserDataContainerを読み込み
        /// </summary>
        public static UserDataContainer LoadFromJson(int slotIndex)
        {
            try
            {
                string filePath = GetSaveFilePath(slotIndex);
                
                if (!File.Exists(filePath))
                {
                    LogUtility.Warning($"セーブファイルが存在しません: {filePath}");
                    return null;
                }
                
                string jsonString = File.ReadAllText(filePath);
                var serializableData = JsonUtility.FromJson<SerializableUserData>(jsonString);
                
                var userData = ConvertFromSerializable(serializableData);
                
                LogUtility.Info($"セーブデータをJSONファイルから読み込みました: {filePath}");
                return userData;
            }
            catch (Exception e)
            {
                LogUtility.Error($"JSONファイル読み込みに失敗しました: {e.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// セーブファイルが存在するかチェック
        /// </summary>
        public static bool SaveFileExists(int slotIndex)
        {
            string filePath = GetSaveFilePath(slotIndex);
            return File.Exists(filePath);
        }
        
        /// <summary>
        /// セーブファイルを削除
        /// </summary>
        public static bool DeleteSaveFile(int slotIndex)
        {
            try
            {
                string filePath = GetSaveFilePath(slotIndex);
                
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    LogUtility.Info($"セーブファイルを削除しました: {filePath}");
                    return true;
                }
                
                LogUtility.Warning($"削除対象のセーブファイルが存在しません: {filePath}");
                return false;
            }
            catch (Exception e)
            {
                LogUtility.Error($"セーブファイル削除に失敗しました: {e.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// 全てのセーブファイルを削除
        /// </summary>
        public static void DeleteAllSaveFiles()
        {
            try
            {
                if (Directory.Exists(SaveDirectoryPath))
                {
                    string[] saveFiles = Directory.GetFiles(SaveDirectoryPath, "*" + SaveFileExtension);
                    
                    foreach (string file in saveFiles)
                    {
                        File.Delete(file);
                    }
                    
                    LogUtility.Info($"全てのセーブファイルを削除しました。削除数: {saveFiles.Length}");
                }
            }
            catch (Exception e)
            {
                LogUtility.Error($"全セーブファイル削除に失敗しました: {e.Message}");
            }
        }
        
        /// <summary>
        /// 存在するセーブスロットのリストを取得
        /// </summary>
        public static List<int> GetExistingSaveSlots()
        {
            var existingSlots = new List<int>();
            
            try
            {
                if (!Directory.Exists(SaveDirectoryPath))
                    return existingSlots;
                
                string[] saveFiles = Directory.GetFiles(SaveDirectoryPath, "*" + SaveFileExtension);
                
                foreach (string filePath in saveFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    if (fileName.StartsWith("save_slot_"))
                    {
                        string slotNumberStr = fileName.Substring("save_slot_".Length);
                        if (int.TryParse(slotNumberStr, out int slotNumber))
                        {
                            existingSlots.Add(slotNumber);
                        }
                    }
                }
                
                existingSlots.Sort();
            }
            catch (Exception e)
            {
                LogUtility.Error($"セーブスロット取得に失敗しました: {e.Message}");
            }
            
            return existingSlots;
        }
        
        /// <summary>
        /// セーブディレクトリが存在しない場合作成する
        /// </summary>
        private static void EnsureSaveDirectoryExists()
        {
            if (!Directory.Exists(SaveDirectoryPath))
            {
                Directory.CreateDirectory(SaveDirectoryPath);
                LogUtility.Info($"セーブディレクトリを作成しました: {SaveDirectoryPath}");
            }
        }
        
        /// <summary>
        /// セーブファイルのパスを取得
        /// </summary>
        private static string GetSaveFilePath(int slotIndex)
        {
            return Path.Combine(SaveDirectoryPath, $"save_slot_{slotIndex:D2}{SaveFileExtension}");
        }
        
        /// <summary>
        /// UserDataContainerをシリアライズ可能な形式に変換
        /// </summary>
        private static SerializableUserData ConvertToSerializable(UserDataContainer userData)
        {
            var serializableData = new SerializableUserData
            {
                UserId = userData.FieldUserData.UserId,
                LastSaveTime = userData.FieldUserData.LastSaveTime
            };
            
            // FieldData変換
            serializableData.FieldData = userData.FieldUserData;
            
            // StoryData変換
            serializableData.StoryData = userData.StoryUserData;
            
            // CharacterData変換
            serializableData.CharacterData = userData.CharacterUserData.GetAllCharacterUserData();
            
            // GameEventData変換
            serializableData.GameEventData = userData.GameEventUserData;
            
            // InventoryData変換
            serializableData.InventoryData = userData.InventoryUserData;
            
            return serializableData;
        }
        
        /// <summary>
        /// シリアライズ可能な形式からUserDataContainerに変換
        /// </summary>
        private static UserDataContainer ConvertFromSerializable(SerializableUserData serializableData)
        {
            var userData = new UserDataContainer(serializableData.UserId);
            
            // FieldDataの復元
            userData.FieldUserData.TransitionMap(serializableData.FieldData.LastMapId);
            userData.FieldUserData.SetLastTranslation(
                serializableData.FieldData.LastPosition,
                serializableData.FieldData.DirectionType
            );
            
            // StoryDataの復元
            userData.StoryUserData.SetRestorationData(serializableData.StoryData.DataList);
            
            // CharacterDataの復元
            userData.CharacterUserData.SetCharacterUserData(serializableData.CharacterData);
            
            // GameEventDataの復元
            userData.GameEventUserData.SetRestorationData(serializableData.GameEventData.DataList);
            
            // InventoryDataの復元
            userData.InventoryUserData.SetRestorationData(serializableData.InventoryData.DataList);
            
            return userData;
        }
    }
}