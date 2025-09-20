using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CryStar.Menu.UI
{
    /// <summary>
    /// メニュー画面でのアイテム表示の管理クラス
    /// </summary>
    public class UIContents_Item : MonoBehaviour
    {
        [SerializeField] private CustomImage _icon;
        [SerializeField] private CustomText _name;
        [SerializeField] private CustomText _quantity;
        
        private void Awake()
        {
            if (_icon == null)
            {
                _icon = transform.GetChild(0).GetComponent<CustomImage>();
            }

            if (_name == null)
            {
                _name = transform.GetChild(1).GetComponent<CustomText>();
            }

            if (_quantity == null)
            {
                _quantity = transform.GetChild(2).GetComponent<CustomText>();
            }
        }

        /// <summary>
        /// Viewを更新する（初期化はこのメソッドを利用するといい）
        /// </summary>
        public async UniTask SetContent(ViewData viewData)
        {
            _name.text = viewData.Name;
            _quantity.text = viewData.Quantity.ToString();
            await _icon.ChangeSpriteAsync(viewData.IconPath);
        }

        /// <summary>
        /// アイテムの個数表示を更新する
        /// </summary>
        public void SetQuantity(int quantity)
        {
            _quantity.text = quantity.ToString();
        }
        
        /// <summary>
        /// オブジェクトのアクティブ状態を変更する
        /// </summary>
        public void IsActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        
        public class ViewData
        {
            public string IconPath { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
        }
    }
}
