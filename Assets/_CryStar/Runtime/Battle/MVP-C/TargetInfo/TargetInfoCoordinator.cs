using CryStar.Attribute;
using UnityEngine;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// TargetInfo_Coordinator
    /// </summary>
    public class TargetInfoCoordinator : CoordinatorBase
    {
        /// <summary>
        /// CoordinatorManager
        /// </summary>
        [SerializeField, HighlightIfNull] 
        private TargetInfoView _view;
        
        /// <summary>
        /// Presenter
        /// </summary>
        private TargetInfoPresenter _presenter = new TargetInfoPresenter();
        
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