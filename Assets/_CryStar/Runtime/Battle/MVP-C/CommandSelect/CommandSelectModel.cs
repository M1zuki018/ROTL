using CryStar.CommandBattle.Enums;
using CryStar.CommandBattle.Execution;
using CryStar.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

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
            
            // 最初のキャラクターのみ、SEの再生を行う
            PlayFirstSelectSe();
        }

        /// <summary>
        /// 行動選択中のキャラクターアイコンのパスを取得する
        /// </summary>
        public string GetCharacterSprite()
        {
            return _battleManager.SelectingUnitIconPath;
        }

        /// <summary>
        /// 行動選択中のキャラクターのテーマカラーを取得する
        /// </summary>
        /// <returns></returns>
        public Color GetCharacterColor()
        {
            return _battleManager.CurrentSelectingUnitData.UserData.CharacterColor;
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
            
            // スキル画面を表示する
            _battleManager.CoordinatorManager.PushCoordinator(BattlePhaseType.Idea);
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
        /// もどる
        /// </summary>
        public void Back()
        {
            // キャンセルSEを再生
            _battleManager.PlayCancelSound().Forget();
            
            // 一つ前のキャラクターのコマンド選択にもどれるか確認
            if (_battleManager.CheckBackCommandSelect())
            {
                _battleManager.CoordinatorManager.TransitionToPhase(BattlePhaseType.CommandSelect);
            }
            else
            {
                // 自分より前にキャラクターがいなかった場合は、最初の行動選択にもどる
                _battleManager.CoordinatorManager.TransitionToPhase(BattlePhaseType.FirstSelect);
            }
        }
        
        /// <summary>
        /// 最初のキャラクターの選択を始めるときのSEを再生する
        /// </summary>
        private void PlayFirstSelectSe()
        {
            return;
            
            // TODO: 若干微妙かもと思いコメントアウト
            if (_battleManager.CurrentCommandSelectIndex != 0)
            {
                return;
            }
            
            TryGetBattleManager();
            _battleManager.PlayStartCommandSelectSe().Forget();
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