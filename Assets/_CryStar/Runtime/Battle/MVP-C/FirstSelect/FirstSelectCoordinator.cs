using CryStar.Attribute;
using UnityEngine;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// FirstSelect_Coordinator
    /// </summary>
    public class FirstSelectCoordinator : CoordinatorBase
    {
        /// <summary>
        /// CoordinatorManager
        /// </summary>
        [SerializeField, HighlightIfNull] 
        private FirstSelectView _view;
        
        /// <summary>
        /// Presenter
        /// </summary>
        private FirstSelectPresenter _presenter = new FirstSelectPresenter();

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