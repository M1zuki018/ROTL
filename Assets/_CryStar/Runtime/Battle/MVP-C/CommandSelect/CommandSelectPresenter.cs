namespace CryStar.CommandBattle
{
    /// <summary>
    /// CommandSelect_Presenter
    /// </summary>
    public class CommandSelectPresenter
    {
        private CommandSelectView _view;
        private CommandSelectModel _model;
    
        /// <summary>
        /// Setup（Enterのタイミングで呼び出し）
        /// </summary>
        public void Setup(CommandSelectView view)
        {
            _view = view;
            _model = new CommandSelectModel();
            
            _model.Setup();
            _view.Setup(
                onAttack: _model.Attack,
                onIdea: _model.Idea,
                onItem: _model.Item,
                onGuard: _model.Guard);
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