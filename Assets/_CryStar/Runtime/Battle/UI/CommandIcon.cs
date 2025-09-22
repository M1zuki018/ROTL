using CryStar.CommandBattle.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CryStar.CommandBattle.UI
{
    /// <summary>
    /// コマンドのアイコン表示
    /// </summary>
    public class CommandIcon : MonoBehaviour
    {
        /// <summary>
        /// アイコンを表示するコンポーネントの参照
        /// </summary>
        [SerializeField]
        private CustomImage _icon;
        
        [SerializeField]
        private Text _name;
        
        private BattleCommandEntryData _entryData;

        public BattleCommandEntryData EntryData => _entryData;
        
        /// <summary>
        /// アイコンを設定したあとに表示する
        /// </summary>
        public async UniTask OnGet(BattleCommandEntryData commandData)
        {
            _entryData = commandData;
            
            // キャラクター名を表示
            _name.text = $"{commandData.Executor.Name} Stand-by";
            _icon.color = commandData.Executor.UserData.CharacterColor;
            
            gameObject.SetActive(true);
        }

        public void OnRelease()
        {
            gameObject.SetActive(false);
        }
    }
}