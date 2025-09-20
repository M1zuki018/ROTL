using System;

namespace iCON.Story.Data
{
    /// <summary>
    /// キャラクターの立ち絵のベースとなる素材のパスのデータ
    /// </summary>
    [Serializable]
    public class CharacterBasePathData
    {
        private string _hair;
        private string _body;
        private string _rear;
        
        /// <summary>
        /// 髪
        /// </summary>
        public string Hair => _hair;
        
        /// <summary>
        /// 体
        /// </summary>
        public string Body => _body;
        
        /// <summary>
        /// 後ろのパーツ
        /// </summary>
        public string Rear => _rear;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CharacterBasePathData(string hair, string body, string rear)
        {
            _hair = hair;
            _body = body;
            _rear = rear;
        }
    }
}
