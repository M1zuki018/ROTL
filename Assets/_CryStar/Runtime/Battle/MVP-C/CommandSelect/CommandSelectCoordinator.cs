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

        public override void Exit()
        {
            _presenter?.Exit();
            base.Exit();
        }
    }
}