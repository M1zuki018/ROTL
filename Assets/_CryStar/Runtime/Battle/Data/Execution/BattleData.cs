using System.Collections.Generic;
using CryStar.Utility;
using CryStar.Utility.Enum;

namespace CryStar.CommandBattle.Data
{
    /// <summary>
    /// バトルに使用するデータ
    /// </summary>
    public class BattleData
    {
        /// <summary>
        /// 戦闘に参加しているキャラクターのデータ
        /// </summary>
        public List<BattleUnitData> UnitData { get; private set; }
        
        /// <summary>
        /// 戦闘に参加している敵のデータ
        /// </summary>
        public List<BattleUnitData> EnemyData {get; private set;}
        
        /// <summary>
        /// 戦闘BGMのパス
        /// </summary>
        public string BGMPath { get; private set; }
        
        /// <summary>
        /// 戦闘に参加しているキャラクターの数
        /// </summary>
        public int UnitCount => UnitData.Count;

        /// <summary>
        /// 戦闘に参加している敵の数
        /// </summary>
        public int EnemyCount => EnemyData.Count;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BattleData(IReadOnlyList<int> units, IReadOnlyList<int> enemies, string bgmPath)
        {
            // ユニットのデータリストを作成
            UnitData = new List<BattleUnitData>(units.Count);
            for (int i = 0; i < units.Count; i++)
            {
                // キャラクターIDを渡してバトルデータを生成
                var unitData = new BattleUnitData(units[i]);
                UnitData.Add(unitData);
                LogUtility.Verbose($"生成されたUnitData {UnitCount}", LogCategory.Gameplay);
            }
            
            // 敵のデータリストを作成
            EnemyData = new List<BattleUnitData>(enemies.Count);
            for (int i = 0; i < enemies.Count; i++)
            {
                // キャラクターIDを渡してバトルデータを生成
                var enemyData = new BattleUnitData(enemies[i]);
                EnemyData.Add(enemyData);
                LogUtility.Verbose($"生成されたEnemyData {EnemyCount}", LogCategory.Gameplay);
            }
            
            BGMPath = bgmPath;
        }
    }
}
