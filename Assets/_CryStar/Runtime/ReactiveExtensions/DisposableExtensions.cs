using System;

namespace CryStar.Core.ReactiveExtensions
{
    /// <summary>
    /// IDisposableの拡張メソッド群
    /// </summary>
    public static class DisposableExtensions
    {
        /// <summary>
        /// IDisposableをCompositeDisposableに追加する拡張メソッド
        /// メソッドチェーンでリソース管理を簡潔に記述できる
        /// </summary>
        /// <param name="disposable">追加するIDisposable</param>
        /// <param name="compositeDisposable">追加先のCompositeDisposable</param>
        public static void AddTo(this IDisposable disposable, CompositeDisposable compositeDisposable)
        {
            compositeDisposable.Add(disposable);
        }
    }
}