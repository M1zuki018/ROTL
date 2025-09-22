using CryStar.Attribute;
using UnityEngine;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// CommandSelect_Coordinator
    /// </summary>
    public class CommandSelectCoordinator : CoordinatorBase
    {
        /// <summary>
        /// CoordinatorManager
        /// </summary>
        [SerializeField, HighlightIfNull] 
        private CommandSelectView _view;
        
        /// <summary>
        /// Presenter
        /// </summary>
        private CommandSelectPresenter _presenter = new CommandSelectPresenter();
        
        public override void Enter()
        {
            base.Enter();
            _presenter?.Setup(_view);
        }
        
        public override void Cancel()
        {
            _presenter?.Cancel();
        }

        public override void Exit()
        {
            _presenter?.Exit();
            base.Exit();
        }

        public override void Block()
        {
            if (_presenter.CheckShowedOverlay())
            {
                // もしオーバーレイが開かれている状態であればBlock処理は行わない
                return;
            }
            
            base.Block();
        }

        /// <summary>
        /// 次の選択へ進める処理
        /// NOTE: アイデアやスキルのコーディネーターが利用する
        /// </summary>
        public void NextPhase()
        {
            _presenter.NextPhase();
        }
    }
}