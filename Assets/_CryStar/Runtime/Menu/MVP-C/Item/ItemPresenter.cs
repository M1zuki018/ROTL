namespace CryStar.Menu
{
    /// <summary>
    /// Item_Presenter
    /// </summary>
    public class ItemPresenter
    {
        private ItemView _view;
        private ItemModel _model;
    
        /// <summary>
        /// Setup（Enterのタイミングで呼び出し）
        /// </summary>
        public void Setup(ItemView view)
        {
            _view = view;
            _model = new ItemModel();
            
            _model.Setup();
            _view.Setup(_model.GetAllItems());
        }

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel()
        {
            _model?.Cancel();
        }

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            _view?.Exit();
        }
    }
}