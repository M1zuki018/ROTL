using CryStar.Attribute;
using UnityEngine;

namespace CryStar.Menu
{
    /// <summary>
    /// CharacterStatus_Coordinator
    /// </summary>
    public class CharacterStatusCoordinator : CoordinatorBase
    {
        /// <summary>
        /// CoordinatorManager
        /// </summary>
        [SerializeField, HighlightIfNull] 
        private CharacterStatusView _view;
        
        /// <summary>
        /// Presenter
        /// </summary>
        private CharacterStatusPresenter _presenter = new CharacterStatusPresenter();
        
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