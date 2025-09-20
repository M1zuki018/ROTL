using System.Collections.Generic;
using CryStar.Core.Enums;
using UnityEngine;

namespace CryStar.Menu.UI
{
    /// <summary>
    /// キャラクターステータス画面の大きな枠の単位のコンテンツ
    /// </summary>
    public class UIContents_Status : MonoBehaviour
    {
        /// <summary>
        /// ステータスコンテンツのリスト
        /// </summary>
        [SerializeField]
        private List<UIContents_Parameter> _parameters = new List<UIContents_Parameter>();

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(ViewData viewData)
        {
            SetValue(StatusType.Level, viewData.Level);
            SetValue(StatusType.Hp, viewData.Hp);
            SetValue(StatusType.Will, viewData.Will);
            SetValue(StatusType.Stamina, viewData.Stamina);
            SetValue(StatusType.Sp, viewData.Sp);
            SetValue(StatusType.PhysicalAttack, viewData.PhysicalAttack);
            SetValue(StatusType.SkillAttack, viewData.SkillAttack);
            SetValue(StatusType.Intelligence, viewData.Intelligence);
            SetValue(StatusType.PhysicalDefense, viewData.PhysicalDefense);
            SetValue(StatusType.SkillDefense, viewData.SkillDefense);
            SetValue(StatusType.Speed, viewData.Speed);
            SetValue(StatusType.DodgeSpeed, viewData.DodgeSpeed);
            SetValue(StatusType.ArmorPenetration, viewData.ArmorPenetration);
            SetValue(StatusType.CriticalRate, viewData.CriticalRate);
            SetValue(StatusType.CriticalDamage, viewData.CriticalDamage);
        }
        
        /// <summary>
        /// 引数で指定したインデックスのステータスの値を更新する
        /// </summary>
        private void SetValue(StatusType statusType, int value)
        {
            _parameters[(int)statusType].SetValue(value);
        }
        
        public class ViewData
        {
            public int Level { get; set; }
            public int Hp { get; set; }
            public int Will { get; set; }
            public int Stamina { get; set; }
            public int Sp { get; set; }
            public int PhysicalAttack { get; set; }
            public int SkillAttack { get; set; }
            public int Intelligence { get; set; }
            public int PhysicalDefense { get; set; }
            public int SkillDefense { get; set; }
            public int Speed { get; set; }
            public int DodgeSpeed { get; set; }
            public int ArmorPenetration { get; set; }
            public int CriticalRate { get; set; }
            public int CriticalDamage { get; set; }
        }
    }
}
