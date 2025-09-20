using System.Collections.Generic;

namespace CryStar.Core.ReactiveExtensions
{
    /// <summary>
    /// 書き込み可能なリアクティブプロパティ
    /// </summary>
    public class ReactiveProperty<T> : ReadOnlyReactiveProperty<T>
    {
        /// <summary>
        /// 値のプロパティ（読み書き可能）
        /// 設定時は値の変更チェックと通知を自動実行
        /// </summary>
        public new T Value
        {
            get => _value;
            set => SetValue(value);
        }
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ReactiveProperty(T initialValue = default) : base(initialValue) { }

        /// <summary>
        /// 値を設定し、変更があった場合のみ購読者に通知
        /// EqualityComparerを使用して値の比較を行う
        /// </summary>
        protected virtual void SetValue(T newValue)
        {
            if (EqualityComparer<T>.Default.Equals(_value, newValue))
            {
                // 現在の値と新しい値が等しい場合は何もしない（通知も行わない）
                return;
            }

            _value = newValue;
            
            // 購読者に変更を通知する
            NotifyObservers();
        }

        /// <summary>
        /// 値が変更されていなくても強制的に購読者に通知を送る
        /// UIの強制更新などで使用
        /// </summary>
        public void ForceNotify()
        {
            NotifyObservers();
        }
    }
}
