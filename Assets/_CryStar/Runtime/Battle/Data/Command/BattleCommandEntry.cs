using System;
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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            var other = (BattleCommandEntryData)obj;

            // Executor, Command, Targetsの組み合わせで同一性を判定
            return Executor == other.Executor &&
                   Command == other.Command &&
                   ArrayEquals(Targets, other.Targets);
        }

        public bool Equals(BattleCommandEntryData other)
        {
            return Equals(Executor, other.Executor) && Equals(Command, other.Command) && Equals(Targets, other.Targets);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Executor, Command, Targets);
        }

        private bool ArrayEquals(BattleUnitData[] array1, BattleUnitData[] array2)
        {
            if (array1 == null && array2 == null)
                return true;
    
            if (array1 == null || array2 == null)
                return false;
    
            if (array1.Length != array2.Length)
                return false;

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                    return false;
            }
    
            return true;
        }
    }
}