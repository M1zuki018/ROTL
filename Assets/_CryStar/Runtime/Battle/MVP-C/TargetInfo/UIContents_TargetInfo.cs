using UnityEngine;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// 敵の情報を表示するUIを管理するクラス
    /// </summary>
    public class UIContents_TargetInfo : MonoBehaviour
    {
        [SerializeField] private CustomText _firstLine;
        [SerializeField] private CustomText _secondLine;
        [SerializeField] private CustomText _thirdLine;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        public CanvasGroup CanvasGroup => _canvasGroup;

        private void Awake()
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }
        }
    }
}