namespace CryStar.MasterData
{
    /// <summary>
    /// マスターデータのロード優先度の列挙型
    /// </summary>
    public enum LoadPriority
    {
        /// <summary>
        /// 常駐（起動時ロード）
        /// </summary>
        Resident,
        
        /// <summary>
        /// キャッシュ（初回アクセス時ロード）
        /// </summary>
        Cached,
        
        /// <summary>
        /// シーン限定（シーンと同時にロード）
        /// </summary>
        SceneLocal,
        
        /// <summary>
        /// オンデマンド（使用直前ロード）
        /// </summary>
        OnDemand
    }
}