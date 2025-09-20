using CryStar.Attribute;
using UnityEngine;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// TryEscape_Coordinator
    /// </summary>
    public class TryEscapeCoordinator : CoordinatorBase
    {
        /// <summary>
        /// CoordinatorManager
        /// </summary>
        [SerializeField, HighlightIfNull] 
        private TryEscapeView _view;
        
        /// <summary>
        /// Presenter
        /// </summary>
        private TryEscapePresenter _presenter = new TryEscapePresenter();
        
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