using System;
using System.Collections.Generic;
using System.Linq;
using CryStar.Core.Enums;
using CryStar.Core.SaveData;
using CryStar.Data;
using CryStar.Data.User;
using CryStar.Utility;
using UnityEditor;
using UnityEngine;

namespace CryStar.Core.UserData
{
    /// <summary>
    /// ユーザーデータ管理クラス
    /// </summary>
    [DefaultExecutionOrder(-998)]
    public class UserDataManager : MonoBehaviour
    {
        /// <summary>
        /// セーブ実行時に呼び出されるイベント
        /// </summary>
        public event Action OnExecuteSaveEvent;
        
        /// <summary>
        /// セーブデータ変更時イベント
        /// </summary>
        public event Action<UserDataContainer> OnUserDataChanged;
        
        /// <summary>
        /// セーブ完了時イベント
        /// </summary>
        public event Action<int> OnSaveCompleted;
        
        /// <summary>
        /// ロード完了時イベント
        /// </summary>
        public event Action<int> OnLoadCompleted;
        
        [SerializeField] private bool _autoSaveEnabled = true;
        [SerializeField] private float _autoSaveInterval = 300f; // 5分間隔
        [SerializeField] private int _maxSaveSlots = 10;
        
        private static int _userId = -1;
        private float _autoSaveTimer = 0f;
        
        /// <summary>
        /// 現在選択中のセーブスロット
        /// </summary>
        private int _currentSaveSlot = 0;
        
        /// <summary>
        /// 現在使用中のユーザーデータ
        /// </summary>
        private UserDataContainer _currentUserData;
        
        /// <summary>
        /// 保存されている全てのユーザーデータ
        /// </summary>
        private List<UserDataContainer> _userDataContainers = new List<UserDataContainer>();
        
        /// <summary>
        /// 現在使用中のユーザーデータ
        /// </summary>
        public UserDataContainer CurrentUserData => _currentUserData;
        
        /// <summary>
        /// 現在のセーブスロット
        /// </summary>
        public int CurrentSaveSlot => _currentSaveSlot;
        
        /// <summary>
        /// 自動保存が有効か
        /// </summary>
        public bool AutoSaveEnabled => _autoSaveEnabled;

        private void Awake()
        {
            // GlobalServiceに既に自身が登録されているかチェックする
            if (ServiceLocator.IsRegisteredGlobal<UserDataManager>())
            {
                // 既に登録されていた場合は、登録されているInstanceが自分ではない場合、オブジェクトを削除する
                if (ServiceLocator.GetGlobal<UserDataManager>() != this)
                {
                    Destroy(gameObject);
                }
                
                // 初期化済みなので早期return
                return;
            }
            
            // グローバルサービスに登録しておく
            ServiceLocator.Register(this, ServiceType.Global);
            DontDestroyOnLoad(gameObject);

            // ユーザーIDを初期化する
            InitializeUserId();

            
            // セーブデータ読み込み
            LoadExistingSaveData();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F11))
            {
                SaveCurrentUserData();
            }
            // 自動保存処理
            if (_autoSaveEnabled && _currentUserData != null)
            {
                _autoSaveTimer += Time.deltaTime;
                
                if (_autoSaveTimer >= _autoSaveInterval)
                {
                    SaveCurrentUserData();
                    _autoSaveTimer = 0f;
                }
            }
        }

        /// <summary>
        /// ユーザーIDの初期化を行う
        /// </summary>
        private void InitializeUserId()
        {
            if (_userId < 0)
            {
                // UserIdが0以下 = 未設定の場合、GUIDを元にユーザーIDを生成する
                var guid = Guid.NewGuid();
                _userId = Math.Abs(guid.GetHashCode());
                LogUtility.Info($"UserIDを生成しました ID:{_userId}");
            }
        }
        
        /// <summary>
        /// 既存のセーブデータを読み込む
        /// </summary>
        private void LoadExistingSaveData()
        {
            var existingSlots = JsonSaveManager.GetExistingSaveSlots();
            
            if (existingSlots.Count > 0)
            {
                // 最新のセーブスロットを選択
                int latestSlot = existingSlots.Last();
                LoadUserData(latestSlot);
            }
            else
            {
                // セーブデータが存在しない場合は新規作成
                CreateNewUserData();
            }
        }
        
        /// <summary>
        /// 指定スロットからユーザーデータを読み込む
        /// </summary>
        public bool LoadUserData(int slotIndex)
        {
            var userData = JsonSaveManager.LoadFromJson(slotIndex);
            
            if (userData != null)
            {
                _currentUserData = userData;
                _currentSaveSlot = slotIndex;
                
                // メモリリストも更新（互換性のため）
                _userDataContainers.Clear();
                _userDataContainers.Add(userData);
                
                LogUtility.Info($"セーブデータを読み込みました。スロット: {slotIndex}");
                OnUserDataChanged?.Invoke(_currentUserData);
                OnLoadCompleted?.Invoke(slotIndex);
                return true;
            }
            
            LogUtility.Warning($"セーブデータの読み込みに失敗しました。スロット: {slotIndex}");
            return false;
        }
        
        /// <summary>
        /// 新規ユーザーデータを作成する
        /// </summary>
        public void CreateNewUserData()
        {
            _currentUserData = new UserDataContainer(_userId);
            _currentSaveSlot = GetNextAvailableSaveSlot();
            
            // メモリリストにも追加（互換性のため）
            _userDataContainers.Add(_currentUserData);
            
            LogUtility.Info($"新規セーブデータを作成しました。スロット: {_currentSaveSlot}");
            OnUserDataChanged?.Invoke(_currentUserData);
        }
        
        /// <summary>
        /// 次に利用可能なセーブスロットを取得
        /// </summary>
        private int GetNextAvailableSaveSlot()
        {
            var existingSlots = JsonSaveManager.GetExistingSaveSlots();
            
            for (int i = 0; i < _maxSaveSlots; i++)
            {
                if (!existingSlots.Contains(i))
                {
                    return i;
                }
            }
            
            // 全スロット使用済みの場合は最古のスロットを使用
            return existingSlots.Count > 0 ? existingSlots[0] : 0;
        }

        /// <summary>
        /// ユーザーデータを作成する
        /// </summary>
        public void CreateUserData()
        {
            // 新規ユーザーデータを作成しリストに保存する
            _userDataContainers.Add(new UserDataContainer(_userId));
            
            // 最新のユーザーデータを現在利用中のユーザーデータ変数にキャッシュ
            _currentUserData = _userDataContainers[_userDataContainers.Count - 1];
            
            LogUtility.Info($"セーブデータを作成しました。Index: {_userDataContainers.Count - 1}, Count: {_userDataContainers.Count}");
        }

        /// <summary>
        /// 使用するユーザーデータを選択する
        /// </summary>
        public bool TrySelectUserData(int index)
        {
            if (index < 0 || index >= _userDataContainers.Count)
            {
                LogUtility.Warning($"無効なインデックスです。Index: {index}, Count: {_userDataContainers.Count}");
                return false;
            }
            
            var userData = _userDataContainers[index];

            if (userData == null)
            {
                LogUtility.Warning($"セーブデータが存在しません。Index: {index}, Count: {_userDataContainers.Count}");
                return false;
            }
            
            _currentUserData = userData;
            LogUtility.Info($"セーブデータを選択しています。Index: {index}, Count: {_userDataContainers.Count}");
            return true;
        }

        /// <summary>
        /// 現在のユーザーデータを保存
        /// </summary>
        public bool SaveCurrentUserData()
        {
            return SaveUserData(_currentSaveSlot);
        }
        
        /// <summary>
        /// 指定スロットにユーザーデータを保存
        /// </summary>
        public bool SaveUserData(int slotIndex)
        {
            if (_currentUserData == null)
            {
                LogUtility.Error("保存するユーザーデータが存在しません");
                return false;
            }

            OnExecuteSaveEvent?.Invoke();
            
            _currentUserData.UpdateAllSaveTimes();
            bool success = JsonSaveManager.SaveToJson(_currentUserData, slotIndex);
            
            if (success)
            {
                _currentSaveSlot = slotIndex;
                LogUtility.Info($"ユーザーデータを保存しました。スロット: {slotIndex}");
                OnSaveCompleted?.Invoke(slotIndex);
                _autoSaveTimer = 0f; // オートセーブタイマーをリセット
            }
            
            return success;
        }
        
        /// <summary>
        /// 指定スロットのセーブデータを削除
        /// </summary>
        public bool DeleteUserData(int slotIndex)
        {
            bool success = JsonSaveManager.DeleteSaveFile(slotIndex);
            
            if (success && slotIndex == _currentSaveSlot)
            {
                // 現在使用中のスロットが削除された場合
                _currentUserData = null;
                _currentSaveSlot = -1;
                _userDataContainers.Clear();
                
                LogUtility.Info($"現在使用中のセーブデータが削除されました。スロット: {slotIndex}");
                OnUserDataChanged?.Invoke(null);
            }
            
            return success;
        }

        /// <summary>
        /// 全てのユーザーデータを削除
        /// </summary>
        public void DeleteAllUserData()
        {
            JsonSaveManager.DeleteAllSaveFiles();
            _userDataContainers.Clear();
            _currentUserData = null;
            _currentSaveSlot = -1;
            
            LogUtility.Info("全てのユーザーデータを削除しました");
            OnUserDataChanged?.Invoke(null);
        }
        
        /// <summary>
        /// 存在するセーブスロットの情報を取得
        /// </summary>
        public List<SaveSlotInfo> GetSaveSlotInfoList()
        {
            var slotInfoList = new List<SaveSlotInfo>();
            var existingSlots = JsonSaveManager.GetExistingSaveSlots();
            
            foreach (int slot in existingSlots)
            {
                var userData = JsonSaveManager.LoadFromJson(slot);
                if (userData != null)
                {
                    slotInfoList.Add(new SaveSlotInfo(slot, userData.FieldUserData.UserId,
                        userData.FieldUserData.LastSaveTime, slot == _currentSaveSlot));
                }
            }
            
            return slotInfoList.OrderBy(info => info.SlotIndex).ToList();
        }
        
        /// <summary>
        /// セーブスロットが存在するかチェック
        /// </summary>
        public bool SaveSlotExists(int slotIndex)
        {
            return JsonSaveManager.SaveFileExists(slotIndex);
        }

        /// <summary>
        /// 自動保存の有効/無効を切り替え
        /// </summary>
        public void SetAutoSaveEnabled(bool enabled)
        {
            _autoSaveEnabled = enabled;
            if (!enabled)
            {
                _autoSaveTimer = 0f;
            }
            
            LogUtility.Info($"自動保存を{(enabled ? "有効" : "無効")}にしました");
        }

        /// <summary>
        /// 自動保存間隔を設定
        /// </summary>
        public void SetAutoSaveInterval(float intervalSeconds)
        {
            _autoSaveInterval = Mathf.Max(10f, intervalSeconds); // 最小10秒
            _autoSaveTimer = 0f;
            
            LogUtility.Info($"自動保存間隔を{_autoSaveInterval}秒に設定しました");
        }

#if UNITY_EDITOR
        
        /// <summary>
        /// 全セーブデータを削除 (Alt + Ctrl + D)
        /// </summary>
        [MenuItem("CryStar/Save Data/Delete All Save Data %&d")]
        public static void DeleteAllSaveData()
        {
            if (EditorUtility.DisplayDialog(
                    "警告", 
                    "全てのセーブデータを削除します。この操作は元に戻せません。\n本当に実行しますか？", 
                    "削除する", 
                    "キャンセル"))
            {
                var manager = FindObjectOfType<UserDataManager>();
                manager.DeleteAllUserData();
            }
        }
        
#endif
    }
}
