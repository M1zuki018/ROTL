using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CryStar.CommandBattle.UI
{
    /// <summary>
    /// コマンドのアイコン表示
    /// </summary>
    [RequireComponent(typeof(CustomImage))]
    public class CommandIcon : MonoBehaviour
    {
        /// <summary>
        /// アイコンを表示するコンポーネントの参照
        /// </summary>
        [SerializeField]
        private CustomImage _icon;

        /// <summary>
        /// アイコンを設定したあとに表示する
        /// </summary>
        public async UniTask OnGet(string iconPath)
        {
            await _icon.ChangeSpriteAsync(iconPath);
            gameObject.SetActive(true);
        }

        public void OnRelease()
        {
            gameObject.SetActive(false);
        }
    }
}