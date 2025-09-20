namespace CryStar.GameEvent.Enums
{
    /// <summary>
    /// ゲームイベントの実行タイプ
    /// </summary>
    public enum ExecutionType
    {
        /// <summary>
        /// 順次実行
        /// </summary>
        Sequential,
        
        /// <summary>
        /// 並列実行
        /// </summary>
        Parallel,
        
        /// <summary>
        /// ハンドルごとに指定
        /// </summary>
        Mixed
    }
}
