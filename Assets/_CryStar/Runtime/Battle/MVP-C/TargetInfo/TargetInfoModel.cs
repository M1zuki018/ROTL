using CryStar.CommandBattle.Enums;
using CryStar.CommandBattle.Execution;
using CryStar.Core;
using Cysharp.Threading.Tasks;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// TargetInfo_Model
    /// </summary>
    public class TargetInfoModel
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
            if (_battleManager != null)
            {
                // ターゲット情報表示中のVolumeを利用する
                _battleManager.ChangeVolumeProfile(false);
            }
        }

        /// <summary>
        /// ターゲット情報を閉じてターン開始時の選択に映る
        /// </summary>
        public void StartBattle()
        {
            TryGetBattleManager();
            _battleManager.PlaySelectedSe(true).Forget();
            
            // 最初の行動選択へ移る
            _battleManager.CoordinatorManager.TransitionToPhase(BattlePhaseType.FirstSelect);
            
            // デフォルトのVolumeに切り替え
            _battleManager.ChangeVolumeProfile(true);
        }
        
        /// <summary>
        /// 敵のキャラクターアイコンのパスを取得する
        /// </summary>
        public string GetCharacterSprite()
        {
            // TODO: 一旦リストの一番最初のキャラクターを表示する想定だが
            // 必要になったらもっと自由に指定できるように修正する
            return _battleManager.Data.EnemyData[0].UserData.IconPath;
        }

        /// <summary>
        /// ターゲットの名前を取得する
        /// </summary>
        public string GetTargetName()
        {
            return _battleManager.Data.EnemyData[0].UserData.Name;
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