namespace CryStar.PerProject
{
    /// <summary>
    /// バトルで使用するターゲットデータ
    /// </summary>
    public class BattleTargetData
    {
        /// <summary>
        /// ターゲットタイプ
        /// </summary>
        private TargetType _targetType;
        
        /// <summary>
        /// 種族
        /// </summary>
        private RaceType _raceType;

        /// <summary>
        /// 攻撃タイプ
        /// </summary>
        private AttackType _attackType;

        /// <summary>
        /// 弱点タイプ
        /// </summary>
        private AttackType _weaknessType;

        /// <summary>
        /// 推奨レベル
        /// </summary>
        private string _recommendedLevel;

        /// <summary>
        /// 追加説明
        /// </summary>
        private string _additionalExplanation;

        /// <summary>
        /// 所属
        /// </summary>
        private string _affiliation;

        /// <summary>
        /// 役職
        /// </summary>
        private string _position;
        
        public TargetType TargetType => _targetType;
        public RaceType RaceType => _raceType;
        public AttackType AttackType => _attackType;
        public AttackType WeaknessType => _weaknessType;
        public string RecommendedLevel => _recommendedLevel;
        public string AdditionalExplanation => _additionalExplanation;
        public string Affiliation => _affiliation;
        public string Position => _position;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BattleTargetData(TargetType targetType, RaceType raceType, AttackType attackType, AttackType weaknessType, 
            string recommendedLevel, string additionalExplanation, string affiliation, string position)
        {
            _targetType = targetType;
            _raceType = raceType;
            _attackType = attackType;
            _weaknessType = weaknessType;
            _recommendedLevel = recommendedLevel;
            _additionalExplanation = additionalExplanation;
            _affiliation = affiliation;
            _position = position;
        }
    }
}
