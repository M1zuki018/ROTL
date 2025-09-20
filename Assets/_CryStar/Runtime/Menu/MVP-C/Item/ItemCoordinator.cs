using CryStar.Attribute;
using UnityEngine;

namespace CryStar.Menu
{
    /// <summary>
    /// Item_Coordinator
    /// </summary>
    public class ItemCoordinator : CoordinatorBase
    {
        /// <summary>
        /// CoordinatorManager
        /// </summary>
        [SerializeField, HighlightIfNull] 
        private ItemView _view;
        
        /// <summary>
        /// Presenter
        /// </summary>
        private ItemPresenter _presenter = new ItemPresenter();
        
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