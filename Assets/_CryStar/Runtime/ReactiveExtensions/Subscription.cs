using System;

namespace CryStar.Core.ReactiveExtensions
{
    /// <summary>
    /// 購読の解除を管理するクラス
    /// Subscribe系メソッドの戻り値として使用され、Disposeで購読解除を実行する
    /// </summary>
    public class Subscription : IDisposable
    {
        /// <summary>
        /// Dispose時に実行するアクション
        /// 通常はコールバックのリストからの削除処理
        /// </summary>
        private Action _onDispose;
        
        /// <summary>
        /// Dispose済みか
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Subscription(Action onDispose)
        {
            _onDispose = onDispose;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            
            // 解除処理を実行
            _onDispose?.Invoke();
            _onDispose = null;
            _disposed = true;
        }
    }
}