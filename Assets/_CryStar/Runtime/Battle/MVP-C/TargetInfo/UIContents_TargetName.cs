using UnityEngine;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// 敵の名前などの表示をするUI群を管理するクラス
    /// </summary>
    public class UIContents_TargetName : MonoBehaviour
    {
        [SerializeField] private CustomText _job;
        [SerializeField] private CustomText _name;
        [SerializeField] private CustomText _affiliation;
    }
}
