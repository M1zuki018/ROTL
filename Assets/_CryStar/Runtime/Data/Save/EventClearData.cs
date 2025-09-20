using System;

namespace CryStar.Data
{
    /// <summary>
    /// Json保存用のイベントデータ
    /// NOTE: 実行時はDictionaryに変換して使うとパフォーマンス的にいい
    /// </summary>
    [Serializable]
    public class IdCountPairData : IEquatable<IdCountPairData>
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id;
        
        /// <summary>
        /// カウント
        /// </summary>
        public int Count;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IdCountPairData(int id, int count)
        {
            Id = id;
            Count = count;
        }

        /// <summary>
        /// 等しいかどうか判定
        /// </summary>
        public bool Equals(IdCountPairData other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && Count == other.Count;
        }
        
        /// <summary>
        /// 文字列表現を返す
        /// </summary>
        public override string ToString()
        {
            return $"IdCountPairData(Id: {Id}, Count: {Count})";
        }
    }
}
