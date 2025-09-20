namespace CryStar.CommandBattle
{
    /// <summary>
    /// Lose_Presenter
    /// </summary>
    public class LosePresenter
    {
        private LoseView _view;
        private LoseModel _model;
    
        /// <summary>
        /// Setup（Enterのタイミングで呼び出し）
        /// </summary>
        public void Setup(LoseView view)
        {
            _view = view;
            _model = new LoseModel();
            
            _model.Setup();
            _view.Setup();
            
            Enter();
        }

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            _view?.Exit();
        }

        /// <summary>
        /// 処理を実行
        /// </summary>
        private void Enter()
        {
            _model?.Enter();
        }
    }
}