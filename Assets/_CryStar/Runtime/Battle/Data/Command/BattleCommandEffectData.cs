namespace CryStar.CommandBattle.Data
{
    /// <summary>
    /// 戦闘効果のデータ
    /// </summary>
    public class BattleCommandEffectData
    {
        /// <summary>
        /// コマンドを実行する対象
        /// </summary>
        public BattleUnitData Target { get; set; }
        
        /// <summary>
        /// 与えるダメージ量（回復の場合は負の数にする）
        /// </summary>
        public int Damage { get; set; }
        
        /// <summary>
        /// 効果名
        /// </summary>
        public string EffectName { get; set; }
        
        /// <summary>
        /// クリティカル判定かどうか
        /// </summary>
        public bool IsCritical { get; set; }
    }
}
