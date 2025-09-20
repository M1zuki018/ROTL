using System.Collections.Generic;
using CryStar.Menu.UI;
using UnityEngine;

namespace CryStar.Menu
{
    /// <summary>
    /// Item_View
    /// </summary>
    public class ItemView : MonoBehaviour
    {
        /// <summary>
        /// アイテム説明のコンテンツ
        /// </summary>
        [SerializeField]
        private UIContents_ItemDescription _itemDescription;
        
        /// <summary>
        /// スクロールビューのコンテンツ
        /// </summary>
        [SerializeField]
        private UIContents_ScrollView _scrollView;
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(List<UIContents_Item.ViewData> viewDataList)
        {
            foreach (UIContents_Item.ViewData viewData in viewDataList)
            {
                _scrollView.SetupContent(viewData);
            }
        }

        // TODO: 実装

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            _itemDescription.Reset();
        }
    }
}