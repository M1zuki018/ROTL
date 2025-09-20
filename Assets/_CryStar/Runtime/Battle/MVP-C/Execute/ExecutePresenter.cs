using Cysharp.Threading.Tasks;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// Execute_Presenter
    /// </summary>
    public class ExecutePresenter
    {
        private ExecuteView _view;
        private ExecuteModel _model;
    
        /// <summary>
        /// Setup（Enterのタイミングで呼び出し）
        /// </summary>
        public void Setup(ExecuteView view)
        {
            _view = view;
            _model = new ExecuteModel();
            
            _model.Setup();
            _view.Setup();
            
            Execute().Forget();
        }

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            _view?.Exit();
        }

        /// <summary>
        /// バトルを実行する
        /// </summary>
        private async UniTask Execute()
        {
            var commandList = _model.GetCommandList();

            foreach (var entry in commandList)
            {
                if (!entry.Executor.IsAlive)
                {
                    // 実行者が死亡している場合はスキップ
                    continue;
                }
                
                _view.SetText($"{entry.Executor.Name}の{entry.Command.DisplayName}");
                
                // 少し待つ
                await UniTask.Delay(1000);

                var message = await _model.ExecuteCommandAndGetMessage(entry);
                
                // メッセージを表示
                _view.SetText(message);
                
                // 少し待つ
                await UniTask.Delay(1000);
            }
            
            // バトル結果を元に次の処理を行う
            await _model.Next();
        }
    }
}