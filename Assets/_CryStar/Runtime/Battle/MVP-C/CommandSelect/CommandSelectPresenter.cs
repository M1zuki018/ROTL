using Cysharp.Threading.Tasks;
using UnityEngine;

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
                onGuard: _model.Guard,
                onBack: _model.Back,
                onHover: _model.Hover);
            
            // 左側のキャラクターの画像を設定する
            _view.SetCharacterPreview(_model.GetCharacterSprite()).Forget();
            
            // ぼかしの色をキャラクターカラーに変更する
            _view.SetEffectColor(_model.GetCharacterColor());
            
            _view.CallEntranceAnimation();
        }

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel()
        {
            _view.CallExitAnimation(_model.Back);
        }
        
        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            _view?.Exit();
        }

        /// <summary>
        /// オーバーレイが開かれているか確認
        /// </summary>
        public bool CheckShowedOverlay()
        {
            return _model.CheckShowedOverlay();
        }
    }
}