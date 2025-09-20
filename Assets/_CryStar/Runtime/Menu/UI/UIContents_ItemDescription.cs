using UnityEngine;

namespace CryStar.Menu.UI
{
    /// <summary>
    /// アイテム説明欄のViewを管理するクラス
    /// </summary>
    public class UIContents_ItemDescription : MonoBehaviour
    {
        [SerializeField] private CustomText _name;
        [SerializeField] private CustomText _description;

        private void Awake()
        {
            if (_name == null)
            {
                _name = transform.GetChild(0).GetComponent<CustomText>();
            }

            if (_description == null)
            {
                _description = transform.GetChild(1).GetComponent<CustomText>();
            }

            // 初期化しておく
            Reset();
        }

        /// <summary>
        /// アイテム名を設定する
        /// </summary>
        public void SetItemName(string name)
        {
            _name.text = name;
        }

        /// <summary>
        /// アイテムの説明を設定する
        /// </summary>
        public void SetItemDescription(string description)
        {
            _description.text = description;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Reset()
        {
            _name.text = "";
            _description.text = "";
        }
    }
}
