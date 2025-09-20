using CryStar.CommandBattle.Command;

namespace CryStar.CommandBattle.Data
{
    /// <summary>
    /// 実行待ちのバトルコマンドを表すエントリークラス
    /// </summary>
    public class BattleCommandEntryData
    {
        /// <summary>
        /// コマンドを実行するバトルユニット（実行者）
        /// </summary>
        public BattleUnitData Executor { get; set; }
        
        /// <summary>
        /// 実行されるバトルコマンドのインスタンス
        /// </summary>
        public IBattleCommand Command { get; set; }
        
        /// <summary>
        /// コマンドの実行対象となるバトルユニットの配列
        /// </summary>
        public BattleUnitData[] Targets { get; set; }
        
        /// <summary>
        /// コマンドの実行優先度
        /// </summary>
        public int Priority => Command.Priority;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="executor">コマンドを実行するバトルユニット（実行者）</param>
        /// <param name="command">実行されるバトルコマンドのインスタンス</param>
        /// <param name="targets">コマンドの実行対象となるバトルユニットの配列</param>
        public BattleCommandEntryData(BattleUnitData executor, IBattleCommand command, BattleUnitData[] targets)
        {
            Executor = executor;
            Command = command;
            Targets = targets;
        }
    }
}