using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CryStar.Menu.UI
{
    /// <summary>
    /// Scrollビューの管理クラス
    /// </summary>
    public class UIContents_ScrollView : MonoBehaviour
    {
        [SerializeField] private UIContents_Item _itemContentsPrefab;
        [SerializeField] private Transform[] _contentsRoots;
        private Dictionary<string, UIContents_Item> _items = new Dictionary<string, UIContents_Item>();

        
        /// <summary>
        /// アイコンのセットアップ
        /// </summary>
        public void SetupContent(UIContents_Item.ViewData viewData)
        {
            var content = Instantiate(_itemContentsPrefab, _contentsRoots[_items.Count / 2]); 
            content.SetContent(viewData).Forget();
            if (!_items.ContainsKey(name))
            {
                _items.Add(name, content);
            }
        }

        /// <summary>
        /// 非表示にする
        /// </summary>
        public void Enabled(string name)
        {
            _items[name].IsActive(false);
            _items.Remove(name);
        }
    }
}
