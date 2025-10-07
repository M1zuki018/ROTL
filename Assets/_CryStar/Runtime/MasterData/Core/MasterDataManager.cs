using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryStar.MasterData
{
    /// <summary>
    /// マスターデータ管理クラス（シングルトン）
    /// </summary>
    public class MasterDataManager
    {
        /// <summary>
        /// インスタンス
        /// </summary>
        private static MasterDataManager _instance;
        public static MasterDataManager Instance => _instance ??= new MasterDataManager();
        
        /// <summary>
        /// 各マスターデータのkvp
        /// </summary>
        private readonly Dictionary<Type, IMasterData> _masters = new();
        
        /// <summary>
        /// 各マスターデータのロードに使用するAddressableのハンドル
        /// </summary>
        private readonly Dictionary<string, AsyncOperationHandle<IList<TextAsset>>> _labelHandles = new();

        /// <summary>
        /// マスターデータを取得する
        /// </summary>
        public T Get<T>() where T : class, IMasterData, new()
        {
            var type = typeof(T);

            if (_masters.TryGetValue(type, out var cached))
            {
                return cached as T;
            }

            // 同期的に取得したい場合はすでにロード済みである必要がある
            Debug.LogWarning($"[MasterData] {type.Name} はロードされていません。GetAsync() か PreloadAsync() を先に呼び出してください");
            return null;
        }

        /// <summary>
        /// マスターデータを非同期取得
        /// 未ロードなら自動ロード
        /// </summary>
        public async UniTask<T> GetAsync<T>() where T : class, IMasterData, new()
        {
            var type = typeof(T);

            if (_masters.TryGetValue(type, out var cached))
            {
                return cached as T;
            }

            var master = new T();
            await master.LoadAsync();
            _masters[type] = master;

            Debug.Log($"[MasterData] ロード中: {type.Name}");
            return master;
        }

        /// <summary>
        /// ラベル指定で一括プリロードを行う
        /// </summary>
        public async UniTask PreloadByLabelAsync(string label)
        {
            if (_labelHandles.ContainsKey(label))
            {
                Debug.Log($"[MasterData] ラベル '{label}' は既にロード済みです");
                return;
            }

            var handle = Addressables.LoadAssetsAsync<TextAsset>(label, null);
            await handle.Task;

            _labelHandles[label] = handle;
            Debug.Log($"[MasterData] プリロード ラベル: {label} ({handle.Result.Count} 個のアセット)");
        }

        /// <summary>
        /// 複数のマスターデータをプリロードする
        /// </summary>
        public async UniTask PreloadAsync(params Type[] masterTypes)
        {
            var tasks = new List<UniTask>();

            foreach (var type in masterTypes)
            {
                if (!_masters.ContainsKey(type))
                {
                    var master = Activator.CreateInstance(type) as IMasterData;
                    if (master != null)
                    {
                        tasks.Add(UniTask.Create(async () =>
                        {
                            await master.LoadAsync();
                            _masters[type] = master;
                        }));
                    }
                }
            }

            await UniTask.WhenAll(tasks);
        }

        /// <summary>
        /// 特定のマスターをアンロード
        /// </summary>
        public void Unload<T>() where T : class, IMasterData
        {
            var type = typeof(T);
            if (_masters.TryGetValue(type, out var master))
            {
                master.Unload();
                _masters.Remove(type);
                Debug.Log($"[MasterData] アンロード: {type.Name}");
            }
        }

        /// <summary>
        /// ラベル指定でアンロード
        /// </summary>
        public void UnloadLabel(string label)
        {
            if (_labelHandles.TryGetValue(label, out var handle))
            {
                Addressables.Release(handle);
                _labelHandles.Remove(label);
                Debug.Log($"[MasterData] アンロード ラベル: {label}");
            }
        }

        /// <summary>
        /// すべてのマスターデータをアンロード
        /// </summary>
        public void UnloadAll()
        {
            foreach (var master in _masters.Values)
            {
                master.Unload();
            }
            _masters.Clear();

            foreach (var handle in _labelHandles.Values)
            {
                Addressables.Release(handle);
            }
            _labelHandles.Clear();

            Debug.Log("[MasterData] すべてのマスターデータをアンロードしました");
        }
    }   
}