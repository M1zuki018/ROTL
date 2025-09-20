using CryStar.CommandBattle.Enums;
using CryStar.CommandBattle.Execution;
using CryStar.Core;
using Cysharp.Threading.Tasks;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// CommandSelect_Model
    /// </summary>
    public class CommandSelectModel
    {
        /// <summary>
        /// BattleManager
        /// </summary>
        private BattleManager _battleManager;

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            _battleManager = ServiceLocator.GetLocal<BattleManager>();
        }

        /// <summary>
        /// 攻撃コマンドを選択したときの処理
        /// </summary>
        public void Attack()
        {
            TryGetBattleManager();
            
            // コマンドを記録
            _battleManager.AddCommandList(CommandType.Attack);
            Next();
        }

        /// <summary>
        /// Idea
        /// </summary>
        public void Idea()
        {
            TryGetBattleManager();
            
            _battleManager.PlaySelectedSe(false).Forget();
            _battleManager.CoordinatorManager.TransitionToPhase(BattlePhaseType.Idea);
        }

        /// <summary>
        /// Item
        /// </summary>
        public void Item()
        {
            // TODO: 実装
        }

        /// <summary>
        /// ガード
        /// </summary>
        public void Guard()
        {
            TryGetBattleManager();
            
            _battleManager.AddCommandList(CommandType.Guard);
            Next();
        }
        
        /// <summary>
        /// 次の行動に進める
        /// </summary>
        private void Next()
        {
            _battleManager.PlaySelectedSe(true).Forget();
            
            // 次のコマンド選択に移れるか確認
            if (_battleManager.CheckNextCommandSelect())
            {
                _battleManager.CoordinatorManager.TransitionToPhase(BattlePhaseType.CommandSelect);
            }
            else
            {
                // バトル実行に移る
                _battleManager.CoordinatorManager.TransitionToPhase(BattlePhaseType.Execute);
            }
        }

        /// <summary>
        /// バトルマネージャーが取得できているか確認し、取得できていなかったらServiceLocatorから取得する
        /// </summary>
        private void TryGetBattleManager()
        {
            if (_battleManager == null)
            {
                _battleManager = ServiceLocator.GetLocal<BattleManager>();
            }
        }
    }
}