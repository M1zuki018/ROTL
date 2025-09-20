using CryStar.Attribute;
using CryStar.Core;
using CryStar.Core.Enums;
using CryStar.Menu.Enums;
using iCON.Enums;
using iCON.UI;
using UnityEngine;

namespace CryStar.Menu.Execution
{
    /// <summary>
    /// メニューシステムを管理するクラス
    /// </summary>
    public class MenuManager : MonoBehaviour
    {
        /// <summary>
        /// InGameのUI管理クラス
        /// </summary>
        [SerializeField, HighlightIfNull]
        private InGameCanvasManager _view;
        
        /// <summary>
        /// Coordinator Manager
        /// </summary>
        [SerializeField, HighlightIfNull]
        private MenuCoordinator _menuCoordinator;
        
        /// <summary>
        /// メニューの状態
        /// </summary>
        [SerializeField]
        private MenuStateType _currentStateType;
        
        /// <summary>
        /// InGameのUI管理クラス
        /// </summary>
        public InGameCanvasManager View => _view;
        
        /// <summary>
        /// Coordinator Manager
        /// </summary>
        public MenuCoordinator MenuCoordinator => _menuCoordinator;

        #region Life cycle

        private void Start()
        {
            ServiceLocator.Register(this, ServiceType.Local);
            
            // メニューを全て非表示にする
            _menuCoordinator.AllHide();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_view.GetCurrentCanvasIndex() != (int)InGameCanvasType.MainMenu)
                {
                    // 現在メニューを開いていない状態だったらメニューを開いて早期リターン
                    _view.ShowCanvas(InGameCanvasType.MainMenu);
                    _menuCoordinator.TransitionToMenu(MenuStateType.MainMenu);
                    return;
                }
                else
                {
                    _menuCoordinator.CurrentCoordinator.Cancel();
                    // NOTE: 画面を閉じる処理はMVP側で実装
                }
            }
        }

        #endregion
    }
}

