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
        [SerializeField] private CanvasGroup _canvasGroup;
        
        public CanvasGroup CanvasGroup => _canvasGroup;

        #region Life cycle

        private void Awake()
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }
        }

        #endregion

        /// <summary>
        /// 役職を設定する
        /// </summary>
        public void SetJob(string job)
        {
            _job.text = job;
        }
        
        /// <summary>
        /// 名前を設定する
        /// </summary>
        public void SetTargetName(string targetName)
        {
            _name.text = targetName;
        }

        /// <summary>
        /// 所属を設定する
        /// </summary>
        public void SetAffiliation(string affiliation)
        {
            _affiliation.text = affiliation;   
        }
    }
}
