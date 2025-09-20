using CryStar.Attribute;
using UnityEngine;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// Idea_Coordinator
    /// </summary>
    public class IdeaCoordinator : CoordinatorBase
    {
        /// <summary>
        /// CoordinatorManager
        /// </summary>
        [SerializeField, HighlightIfNull] 
        private IdeaView _view;
        
        /// <summary>
        /// Presenter
        /// </summary>
        private IdeaPresenter _presenter = new IdeaPresenter();
        
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
    }
}