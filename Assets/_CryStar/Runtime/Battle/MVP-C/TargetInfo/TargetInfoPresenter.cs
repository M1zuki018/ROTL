using Cysharp.Threading.Tasks;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// TargetInfo_Presenter
    /// </summary>
    public class TargetInfoPresenter
    {
        private TargetInfoView _view;
        private TargetInfoModel _model;
    
        /// <summary>
        /// Setup（Enterのタイミングで呼び出し）
        /// </summary>
        public void Setup(TargetInfoView view)
        {
            _view = view;
            _model = new TargetInfoModel();
            
            _model.Setup();
            _view.Setup(_model.StartBattle);
            
            SetupSequence().Forget();
        }

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            _view?.Exit();
        }

        /// <summary>
        /// セットアップの一連の処理
        /// </summary>
        private async UniTask SetupSequence()
        {
            // キャラクターの画像差し替え
            await _view.SetCharacterPreview(_model.GetCharacterSprite());
            
            _view.SetTargetName(_model.GetTargetName());
            
            // TODO: 名前などほかのコンテンツの初期化を行う
            
            _view.PlayAnimation();
        }
    }
}