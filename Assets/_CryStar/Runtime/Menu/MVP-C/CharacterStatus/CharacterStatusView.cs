using CryStar.Menu.UI;
using UnityEngine;

namespace CryStar.Menu
{
    /// <summary>
    /// CharacterStatus_View
    /// </summary>
    public class CharacterStatusView : MonoBehaviour
    {
        /// <summary>
        /// ステータス表記のコンテンツ
        /// </summary>
        [SerializeField] 
        private UIContents_Status _status;
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(UIContents_Status.ViewData viewData)
        {
            // TODO: キャラクターを切り替えるボタンを追加→ボタンが押されたときに表示されるキャラクターを切り替える処理
            _status.Setup(viewData);
        }

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            // Actionの解放処理
        }
    }
}