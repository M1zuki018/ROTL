using Cysharp.Threading.Tasks;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// Win_Presenter
    /// </summary>
    public class WinPresenter
    {
        private WinView _view;
        private WinModel _model;
    
        /// <summary>
        /// Setup（Enterのタイミングで呼び出し）
        /// </summary>
        public void Setup(WinView view)
        {
            _view = view;
            _model = new WinModel();
            
            _model.Setup();
            _view.Setup();
            
            Enter().Forget();
        }

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            _view?.Exit();
        }

        private async UniTask Enter()
        {
            _model.FinishBGM();
            
            // 戦闘結果のパネルを表示する
            var resultData = _model.GetResultData();
            _view.SetText(resultData.name, resultData.experience);
            
            await UniTask.Delay(4000);

            await _model.TransitionToInGameScene();
        }
    }
}