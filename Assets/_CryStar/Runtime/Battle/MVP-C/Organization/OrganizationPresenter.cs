namespace CryStar.CommandBattle
{
    /// <summary>
    /// Organization_Presenter
    /// </summary>
    public class OrganizationPresenter
    {
        private OrganizationView _view;
        private OrganizationModel _model;
    
        /// <summary>
        /// Setup（Enterのタイミングで呼び出し）
        /// </summary>
        public void Setup(OrganizationView view)
        {
            _view = view;
            _model = new OrganizationModel();
            
            _model.Setup();
            _view.Setup();
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