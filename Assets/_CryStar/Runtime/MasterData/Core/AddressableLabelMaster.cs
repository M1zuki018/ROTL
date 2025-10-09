using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryStar.MasterData
{
    /// <summary>
    /// 複数アセットをラベルでロードするマスター
    /// </summary>
    public abstract class AddressableLabelMaster<TKey, TValue> : IMasterData
    {
        public abstract LoadPriority Priority { get; }
        public bool IsLoaded => _data != null;
        
        /// <summary>
        /// データ
        /// </summary>
        protected Dictionary<TKey, TValue> _data;
        
        protected abstract string Label { get; }
        
        private AsyncOperationHandle<IList<TextAsset>> _handle;

        public virtual async UniTask LoadAsync()
        {
            if (IsLoaded)
            {
                Debug.LogWarning($"[{GetType().Name}] Already loaded.");
                return;
            }

            _handle = Addressables.LoadAssetsAsync<TextAsset>(Label, null);
            var assets = await _handle.Task;

            _data = new Dictionary<TKey, TValue>();

            foreach (var asset in assets)
            {
                LoadFromJson(asset.text);
            }
        }

        protected abstract void LoadFromJson(string json);

        public virtual void Unload()
        {
            _data?.Clear();
            _data = null;

            if (_handle.IsValid())
            {
                Addressables.Release(_handle);
            }
        }

        public TValue Get(TKey key)
        {
            return _data.GetValueOrDefault(key);
        }

        public IEnumerable<TValue> GetAll()
        {
            return _data?.Values;
        }
    }
}