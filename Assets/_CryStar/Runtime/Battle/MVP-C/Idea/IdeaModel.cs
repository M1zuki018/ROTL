using CryStar.CommandBattle.Enums;
using CryStar.CommandBattle.Execution;
using CryStar.Core;
using Cysharp.Threading.Tasks;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// Idea_Model
    /// </summary>
    public class IdeaModel
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
        /// Cancel
        /// </summary>
        public void Cancel()
        {
            // バトルマネージャーの参照がとれているか確認
            TryGetBattleManager();
            
            _battleManager.PlayCancelSound().Forget();
            // コマンド選択に戻る
            _battleManager.CoordinatorManager.TransitionToPhase(BattlePhaseType.CommandSelect);
        }

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            // バトルマネージャーの参照がとれているか確認
            TryGetBattleManager();
            _battleManager.CoordinatorManager.PopCoordinator();
        }
        
        /// <summary>
        /// Ideaが選択されたときの処理
        /// </summary>
        public void HandleIdeaSelected(int selectedIdeaId)
        {
            // バトルマネージャーの参照がとれているか確認
            TryGetBattleManager();
            
            // TODO: 未実装
            // 現在選択中のユニットのアイデアのIDリストから、選択されたアイデアの情報を取得する
            //var selectedIdea = _battleManager.CurrentSelectingUnitData.UserData.IdeaIdList[selectedIdeaId];
            
            // 選択されたアイデアをコマンドリストに追加
            _battleManager.AddCommandList(CommandType.Idea);
            Next();
        }
        
        /// <summary>
        /// 次の行動に進める
        /// </summary>
        private void Next()
        {
            _battleManager.PlaySelectedSe(true).Forget();
            // 次のコマンド選択があるかチェックし、適切な状態に遷移
            _battleManager.CoordinatorManager.TransitionToPhase(_battleManager.CheckNextCommandSelect()
            ? BattlePhaseType.CommandSelect : BattlePhaseType.Execute);
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