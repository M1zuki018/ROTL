using Cysharp.Threading.Tasks;

namespace CryStar.MasterData
{
    /// <summary>
    /// マスターデータの基底インターフェース
    /// </summary>
    public interface IMasterData
    {
        /// <summary>
        /// ロード優先度
        /// </summary>
        LoadPriority Priority { get; }
        
        /// <summary>
        /// ロード済みか
        /// </summary>
        bool IsLoaded { get; }
        
        /// <summary>
        /// 非同期で読み込みを行うメソッド
        /// </summary>
        UniTask LoadAsync();
        
        /// <summary>
        /// アンロード用メソッド
        /// </summary>
        void Unload();
    }
}