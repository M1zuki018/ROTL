using CryStar.PerProject;
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
        
        /// <summary>
        /// 推奨レベル表示のための文言キー
        /// </summary>
        private const string RECOMMENDED_WORDING_KEY = "RECOMMENDED_LEVEL";

        private void Awake()
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }
        }

        /// <summary>
        /// 敵の情報の一行目のUIを設定する
        /// NOTE: 「種族 - 攻撃タイプ - 弱点タイプ」
        /// </summary>
        public void SetFirstLine(RaceType raceType, AttackType attackType, AttackType weaknessType)
        {
            var text = $"{raceType.ToString()} - Type {attackType.ToString()} - Weak {weaknessType.ToString()}";
            _firstLine.SetText(text);
        }

        /// <summary>
        /// 推奨レベル
        /// NOTE: 「Recommended Level - ○○」
        /// </summary>
        public void SetSecondLine(string recommendedLevel)
        {
            var text = $"{WordingMaster.GetText(RECOMMENDED_WORDING_KEY)} - {recommendedLevel}";
            _secondLine.SetText(text);
        }

        /// <summary>
        /// 追加説明
        /// </summary>
        public void SetThirdLine(string additionalExplanation)
        {
            _thirdLine.SetText(additionalExplanation);
        }
    }
}