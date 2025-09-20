using System;

namespace CryStar.CommandBattle.Data
{
    /// <summary>
    /// バトルコマンドの実行結果を表すクラス
    /// </summary>
    public class BattleCommandResultData
    {
        /// <summary>
        /// コマンドの実行が成功したか
        /// </summary>
        public bool IsSuccess { get; set; }
 
        /// <summary>
        /// ログとして表示するメッセージ
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// コマンド実行によって発生したバトルエフェクトの配列
        /// </summary>
        public BattleCommandEffectData[] Effects { get; set; }
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="isSuccess">コマンドが成功したか</param>
        /// <param name="message">ログとして表示するメッセージ</param>
        /// <param name="effects">コマンド実行によって発生したバトルエフェクトの配列</param>
        public BattleCommandResultData(bool isSuccess, string message = "", BattleCommandEffectData[] effects = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Effects = effects ?? Array.Empty<BattleCommandEffectData>();
        }
    }
}