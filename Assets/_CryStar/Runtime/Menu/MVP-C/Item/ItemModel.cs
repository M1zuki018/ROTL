using System.Collections.Generic;
using CryStar.Core;
using CryStar.Core.UserData;
using CryStar.Item;
using CryStar.Item.Data;
using CryStar.Menu.Enums;
using CryStar.Menu.Execution;
using CryStar.Menu.UI;
using iCON.Enums;

namespace CryStar.Menu
{
    /// <summary>
    /// Item_Model
    /// </summary>
    public class ItemModel
    {
        /// <summary>
        /// MenuManager
        /// </summary>
        private MenuManager _manager;
        
        /// <summary>
        /// InventoryManager
        /// </summary>
        private InventoryManager _inventoryManager;
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            if (_inventoryManager == null)
            {
                _inventoryManager = ServiceLocator.GetGlobal<InventoryManager>();
            }
        }

        public List<UIContents_Item.ViewData> GetAllItems()
        {
            var allItems = _inventoryManager.GetAllItems();
            
            var items = new List<UIContents_Item.ViewData>();
            foreach (var item in allItems)
            {
                items.Add(new UIContents_Item.ViewData
                {
                    Name = item.Name,
                    IconPath = item.IconPath,
                    Quantity = item.Id
                });
            }

            return items;
        }

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel()
        {
            TryGetMenuManager();
            _manager.MenuCoordinator.TransitionToMenu(MenuStateType.MainMenu);
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