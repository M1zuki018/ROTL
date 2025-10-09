using CryStar.Core;
using CryStar.Core.UserData;
using CryStar.Data.User;
using CryStar.MasterData;
using CryStar.Menu.Enums;
using CryStar.Menu.Execution;
using CryStar.Menu.UI;

namespace CryStar.Menu
{
    /// <summary>
    /// CharacterStatus_Model
    /// </summary>
    public class CharacterStatusModel
    {
        /// <summary>
        /// MenuManager
        /// </summary>
        private MenuManager _manager;
        
        /// <summary>
        /// UserDataManager
        /// </summary>
        private UserDataManager _userDataManager;
        
        /// <summary>
        /// ユーザーデータの参照
        /// </summary>
        private CharacterUserData UserData => _userDataManager.CurrentUserData.CharacterUserData;

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            if (_userDataManager == null)
            {
                // UserDataManagerの参照がなければGlobalServiceから取得
                _userDataManager = ServiceLocator.GetGlobal<UserDataManager>();
            }
        }

        /// <summary>
        /// ViewDataを取得する
        /// </summary>
        public UIContents_Status.ViewData GetViewData(int characterId = 1)
        {
            var userData = UserData.GetCharacterUserData(characterId);
            var level = userData.Level;
            
            return new UIContents_Status.ViewData()
            {
                Level = level,
                Hp = MasterGrowthCharacter.GetHp(characterId, level) - userData.DecreaseHp + userData.BonusHp,
                Will = 5, // TODO
                Stamina = 50, // TODO
                Sp = MasterGrowthCharacter.GetSp(characterId, level) - userData.DecreaseSp + userData.BonusSp,
                PhysicalAttack = MasterGrowthCharacter.GetAttack(characterId, level) + userData.BonusAttack,
                SkillAttack = MasterGrowthCharacter.GetAttack(characterId, level) + userData.BonusAttack, // TODO
                Intelligence = MasterGrowthCharacter.GetStatusResistance(characterId, level) + userData.BonusStatusResistance,
                PhysicalDefense = MasterGrowthCharacter.GetDefense(characterId, level) + userData.BonusDefense,
                SkillDefense = MasterGrowthCharacter.GetDefense(characterId, level) + userData.BonusDefense, // TODO
                Speed = MasterGrowthCharacter.GetSpeed(characterId, level) + userData.BonusSpeed,
                DodgeSpeed = MasterGrowthCharacter.GetDodgeSpeed(characterId, level) + userData.BonusDodgeSpeed,
                ArmorPenetration = MasterGrowthCharacter.GetArmorPenetration(characterId, level) + userData.BonusArmorPenetration,
                CriticalRate = MasterGrowthCharacter.GetCriticalRate(characterId, level) + userData.BonusCriticalRate,
                CriticalDamage = MasterGrowthCharacter.GetCriticalDamage(characterId, level) + userData.BonusCriticalDamage
            };
        }

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel()
        {
            TryGetMenuManager();
            // メインメニューへ遷移
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