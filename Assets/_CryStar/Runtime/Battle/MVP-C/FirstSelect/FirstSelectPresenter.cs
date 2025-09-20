namespace CryStar.CommandBattle
{
    /// <summary>
    /// FirstSelect_Presenter
    /// </summary>
    public class FirstSelectPresenter
    {
        private FirstSelectView _view;
        private FirstSelectModel _model;

        /// <summary>
        /// Setup（Enterのタイミングで呼び出し）
        /// </summary>
        public void Setup(FirstSelectView view)
        {
            _view = view;
            _model = new FirstSelectModel();

            _model.Setup();
            _view.Setup(
                startAction: _model.StartBattle, 
                escapeAction: _model.TryEscape);
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