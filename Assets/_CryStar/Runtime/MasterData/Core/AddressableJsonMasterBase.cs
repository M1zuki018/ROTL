using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryStar.MasterData
{
    /// <summary>
    /// Addressable + JSON形式マスターデータの基底クラス
    /// </summary>
    public abstract class AddressableJsonMasterBase<TKey, TValue> : IMasterData
    {
        /// <summary>
        /// ロード優先順
        /// </summary>
        public abstract LoadPriority Priority { get; }
        
        /// <summary>
        /// ロード済みか
        /// </summary>
        public bool IsLoaded => _data != null;
        
        /// <summary>
        /// データ
        /// </summary>
        protected Dictionary<TKey, TValue> _data;
        
        /// <summary>
        /// AddressableのアドレスまたはラベルでJSONをロード
        /// 継承先でプロパティを記述する
        /// </summary>
        protected abstract string AddressOrLabel { get; }
        
        /// <summary>
        /// ロード用のハンドル
        /// </summary>
        private AsyncOperationHandle<TextAsset> _handle;

        /// <summary>
        /// 非同期でデータをロード
        /// </summary>
        public virtual async UniTask LoadAsync()
        {
            if (IsLoaded)
            {
                Debug.LogWarning($"[{GetType().Name}] は既にロード済みです");
                return;
            }

            _handle = Addressables.LoadAssetAsync<TextAsset>(AddressOrLabel);
            var textAsset = await _handle.Task;

            if (textAsset == null)
            {
                Debug.LogError($"[{GetType().Name}] ロードに失敗しました: {AddressOrLabel}");
                _data = new Dictionary<TKey, TValue>();
                return;
            }

            LoadFromJson(textAsset.text);
        }

        /// <summary>
        /// Jsonファイルを読み込み
        /// </summary>
        protected abstract void LoadFromJson(string json);

        /// <summary>
        /// アンロード
        /// </summary>
        public virtual void Unload()
        {
            _data?.Clear();
            _data = null;

            if (_handle.IsValid())
            {
                Addressables.Release(_handle);
            }
        }

        /// <summary>
        /// キーで指定したデータを取得する
        /// </summary>
        public TValue Get(TKey key)
        {
            return _data.GetValueOrDefault(key);
        }

        /// <summary>
        /// 全てのデータを取得する
        /// </summary>
        public IEnumerable<TValue> GetAll()
        {
            return _data?.Values;
        }
    }
}