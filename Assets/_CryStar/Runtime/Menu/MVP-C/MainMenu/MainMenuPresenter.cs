namespace CryStar.Menu
{
    /// <summary>
    /// MainMenu_Presenter
    /// </summary>
    public class MainMenuPresenter
    {
        private MainMenuView _view;
        private MainMenuModel _model;
    
        /// <summary>
        /// Setup（Enterのタイミングで呼び出し）
        /// </summary>
        public void Setup(MainMenuView view)
        {
            _view = view;
            _model = new MainMenuModel();
            
            _model.Setup();
            _view.Setup(
                onStatus: _model.HandleStatusButton,
                onItem: _model.HandleItemButton,
                onBackTitle: _model.HandleBackTitleButton);
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