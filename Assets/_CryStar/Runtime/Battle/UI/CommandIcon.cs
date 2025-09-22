using CryStar.CommandBattle.Data;
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
        
        private BattleCommandEntryData _entryData;

        public BattleCommandEntryData EntryData => _entryData;
        
        /// <summary>
        /// アイコンを設定したあとに表示する
        /// </summary>
        public async UniTask OnGet(BattleCommandEntryData commandData)
        {
            _entryData = commandData;
            
            await _icon.ChangeSpriteAsync("Assets/AssetStoreTools/Images/Battle/UI/Selector/Selector_Attack.png");
            gameObject.SetActive(true);
        }

        public void OnRelease()
        {
            gameObject.SetActive(false);
        }
    }
}