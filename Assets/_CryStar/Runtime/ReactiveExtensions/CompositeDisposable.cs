using System;
using System.Collections.Generic;

namespace CryStar.Core.ReactiveExtensions
{
    /// <summary>
    /// 複数のIDisposableを管理し、一括でDisposeするためのクラス
    /// GameObjectのOnDestroyなどで一括リソース解放に使用
    /// </summary>
    public class CompositeDisposable : IDisposable
    {
        /// <summary>
        /// 管理対象のIDisposableのリスト
        /// </summary>
        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        
        /// <summary>
        /// Dispose済みか
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// IDisposableを管理リストに追加
        /// 既にDisposeされている場合は即座にDisposeする
        /// </summary>
        public void Add(IDisposable disposable)
        {
            if (_disposed)
            {
                // 既にCompositeDisposableがDispose済みであれば、リストに追加せず引数のIDisposableもすぐにDispose
                disposable?.Dispose();
                return;
            }

            if (disposable != null)
            {
                _disposables.Add(disposable);
            }
        }

        /// <summary>
        /// 指定したIDisposableを管理リストから削除
        /// Disposeは行わない
        /// </summary>
        public void Remove(IDisposable disposable)
        {
            if (!_disposed && disposable != null)
            {
                // まだDispose済みではない場合はリストから削除
                _disposables.Remove(disposable);
            }
        }

        /// <summary>
        /// 管理しているすべてのIDisposableをDisposeし、リストをクリア
        /// </summary>
        public void Clear()
        {
            if (_disposed)
            {
                // 既にDispose済みであれば二重開放を防ぐため早期リターン
                return;
            }

            foreach (var disposable in _disposables)
            {
                disposable?.Dispose();
            }
            
            _disposables.Clear();
        }
        
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                // 既にDispose済みであれば処理を行う必要はないのでreturn
                return;
            }
            
            Clear();
            _disposed = true;
        }
    }
}
