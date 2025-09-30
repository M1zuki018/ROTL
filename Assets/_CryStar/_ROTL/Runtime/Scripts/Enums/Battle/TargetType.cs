namespace CryStar.PerProject
{
    /// <summary>
    /// ターゲットタイプ
    /// </summary>
    public enum TargetType
    {
        /// <summary>
        /// 契約による束縛
        /// NOTE: vs人間
        /// </summary>
        Bind,
        
        /// <summary>
        /// 敵対対象
        /// NOTE: vs魔物
        /// </summary>
        Hostile,
        
        /// <summary>
        /// 排除対象
        /// NOTE: 一部ボス戦など
        /// </summary>
        Elimination,
    }
}