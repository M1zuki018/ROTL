using CryStar.Attribute;
using UnityEngine;

namespace CryStar.Menu
{
    /// <summary>
    /// MainMenu_Coordinator
    /// </summary>
    public class MainMenuCoordinator : CoordinatorBase
    {
        /// <summary>
        /// CoordinatorManager
        /// </summary>
        [SerializeField, HighlightIfNull] 
        private MainMenuView _view;
        
        /// <summary>
        /// Presenter
        /// </summary>
        private MainMenuPresenter _presenter = new MainMenuPresenter();
        
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