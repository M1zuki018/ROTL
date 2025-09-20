namespace CryStar.Menu
{
    /// <summary>
    /// CharacterStatus_Presenter
    /// </summary>
    public class CharacterStatusPresenter
    {
        private CharacterStatusView _view;
        private CharacterStatusModel _model;
    
        /// <summary>
        /// Setup（Enterのタイミングで呼び出し）
        /// </summary>
        public void Setup(CharacterStatusView view)
        {
            _view = view;
            _model = new CharacterStatusModel();
            
            _model.Setup();
            _view.Setup(_model.GetViewData());
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