using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryStar.Core.ReactiveExtensions
{
    /// <summary>
    /// 読み取り専用のリアクティブプロパティ
    /// </summary>
    public class ReadOnlyReactiveProperty<T> : IDisposable
    {
        /// <summary>
        /// 保持している値
        /// 継承クラスから変更可能
        /// </summary>
        protected T _value;
        
        /// <summary>
        /// 値の変更を購読しているコールバックのリスト
        /// </summary>
        protected readonly List<Action<T>> _observers = new List<Action<T>>();
        
        /// <summary>
        /// Dispose済みか
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// 現在の値（読み取り専用）
        /// </summary>
        public T Value => _value;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ReadOnlyReactiveProperty(T initialValue = default)
        {
            _value = initialValue;
        }

        /// <summary>
        /// 値の変更を購読し、即座に現在の値で通知
        /// 購読開始時に現在の値を受け取りたい場合に使用
        /// </summary>
        public IDisposable Subscribe(Action<T> observer)
        {
            if (_disposed) return new Subscription(() => { });
            
            if (observer != null)
            {
                _observers.Add(observer);
                
                // 購読時に現在の値を通知
                observer(_value);
            }

            // 購読解除用のSubscriptionを返す
            return new Subscription(() => _observers.Remove(observer));
        }

        /// <summary>
        /// 値の変更のみを購読（初期値通知なし）
        /// 現在の値ではなく、次回以降の変更のみを受け取りたい場合に使用する
        /// </summary>
        public IDisposable SubscribeToValueChanged(Action<T> observer)
        {
            if (_disposed) return new Subscription(() => { });
            
            if (observer != null)
                _observers.Add(observer);

            return new Subscription(() => _observers.Remove(observer));
        }

        /// <summary>
        /// 登録されたすべてのコールバックに現在の値を通知する
        /// </summary>
        protected virtual void NotifyObservers()
        {
            if (_disposed) return;
            
            // NOTE: 逆順でループすることで、通知中にリストが変更されても安全に処理を行う
            for (int i = _observers.Count - 1; i >= 0; i--)
            {
                try
                {
                    _observers[i]?.Invoke(_value);
                }
                catch (Exception ex)
                {
                    // 例外が発生してもログ出力して処理を継続
                    Debug.LogException(ex);
                }
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
            if (_disposed) return;
            
            _observers.Clear();
            _disposed = true;
        }

        /// <summary>
        /// ReactivePropertyから値への暗黙的変換
        /// property.Value の代わりに直接 property として使用可能にする
        /// 例: int health = healthProperty; // healthProperty.Value と同じ
        /// </summary>
        public static implicit operator T(ReadOnlyReactiveProperty<T> property)
        {
            return property.Value;
        }
    }
}