using CryStar.Core;
using CryStar.Core.Enums;
using CryStar.Data.Scene;
using CryStar.Menu.Enums;
using CryStar.Menu.Execution;
using Cysharp.Threading.Tasks;
using iCON.Enums;

namespace CryStar.Menu
{
    /// <summary>
    /// MainMenu_Model
    /// </summary>
    public class MainMenuModel
    {
        /// <summary>
        /// MenuManager
        /// </summary>
        private MenuManager _manager;
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            // TODO
        }

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel()
        {
            TryGetMenuManager();
            
            // すべてのメニュー画面を閉じる
            _manager.MenuCoordinator.AllHide();
            _manager.View.ShowCanvas(InGameCanvasType.InGame);
        }
        
        /// <summary>
        /// キャラクターステータスボタンを押したときの処理
        /// </summary>
        public void HandleStatusButton()
        {
            TryGetMenuManager();
            _manager.MenuCoordinator.TransitionToMenu(MenuStateType.CharacterStates);
        }

        /// <summary>
        /// アイテムボタンを押したときの処理
        /// </summary>
        public void HandleItemButton()
        {
            TryGetMenuManager();
            _manager.MenuCoordinator.TransitionToMenu(MenuStateType.Item);
        }

        /// <summary>
        /// Titleに戻るボタンを押したときの処理
        /// </summary>
        public void HandleBackTitleButton()
        {
            var sceneLoader = ServiceLocator.GetGlobal<SceneLoader>();
            sceneLoader.LoadSceneAsync(new SceneTransitionData(SceneType.Title, true, true)).Forget();
        }
        
        /// <summary>
        /// メニューマネージャーが取得できているか確認し、取得できていなかったらServiceLocatorから取得する
        /// </summary>
        private void TryGetMenuManager()
        {
            if (_manager == null)
            {
                _manager = ServiceLocator.GetLocal<MenuManager>();
            }
        }
    }
}