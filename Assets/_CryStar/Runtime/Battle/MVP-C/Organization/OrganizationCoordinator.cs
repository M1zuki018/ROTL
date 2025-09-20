using CryStar.Attribute;
using UnityEngine;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// Organization_Coordinator
    /// </summary>
    public class OrganizationCoordinator : CoordinatorBase
    {
        /// <summary>
        /// CoordinatorManager
        /// </summary>
        [SerializeField, HighlightIfNull] 
        private OrganizationView _view;
        
        /// <summary>
        /// Presenter
        /// </summary>
        private OrganizationPresenter _presenter = new OrganizationPresenter();
        
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