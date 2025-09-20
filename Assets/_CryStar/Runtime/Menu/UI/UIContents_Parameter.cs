using UnityEngine;

namespace CryStar.Menu.UI
{
    /// <summary>
    /// パラメーター名とその値がセットになったUIContents
    /// </summary>
    public class UIContents_Parameter : MonoBehaviour
    {
        /// <summary>
        /// パラメーター名のCustomText
        /// </summary>
        [SerializeField] 
        private CustomText _paramName;
        
        /// <summary>
        /// パラメーターの値
        /// </summary>
        [SerializeField]
        private CustomText _paramValue;

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            if (_paramName == null)
            {
                // nullの場合子オブジェクト1番目からCustomTextを取得
                _paramName = transform.GetChild(0).GetComponent<CustomText>();
            }

            if (_paramValue == null)
            {
                // nullの場合子オブジェクト2番目からCustomTextを取得
                _paramValue = transform.GetChild(1).GetComponent<CustomText>();
            }
        }

        /// <summary>
        /// パラメーターの値を変更する
        /// </summary>
        public void SetValue(int value)
        {
            _paramValue.text = value.ToString();
        }
    }
}
