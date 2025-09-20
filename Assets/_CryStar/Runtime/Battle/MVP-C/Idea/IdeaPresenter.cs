namespace CryStar.CommandBattle
{
    /// <summary>
    /// Idea_Presenter
    /// </summary>
    public class IdeaPresenter
    {
        private IdeaView _view;
        private IdeaModel _model;
    
        /// <summary>
        /// Setup（Enterのタイミングで呼び出し）
        /// </summary>
        public void Setup(IdeaView view)
        {
            _view = view;
            _model = new IdeaModel();
            
            _model.Setup();
            _view.Setup(onIdeaSelected: _model.HandleIdeaSelected);
        }

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel()
        {
            _model.Cancel();
        }

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            _view?.Exit();
            _model?.Exit();
        }
    }
}