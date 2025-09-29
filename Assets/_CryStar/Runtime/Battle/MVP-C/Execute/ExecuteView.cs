using CryStar.Attribute;
using DG.Tweening;
using UnityEngine;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// Execute_View
    /// </summary>
    public class ExecuteView : MonoBehaviour
    {
        [SerializeField, HighlightIfNull] 
        private ExecutionLog[] _logArray;
        
        /// <summary>
        /// 使用している末尾のログのインデックス
        /// </summary>
        private int _usedLogIndex = 0;
        
        /// <summary>
        /// 表示したログの総数
        /// </summary>
        private int _totalLogCount = 0;
        
        private Sequence _sequence;

        private void OnDestroy()
        {
            Dispose();
        }
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
        }

        /// <summary>
        /// テキストを設定
        /// </summary>
        public void SetText(string text)
        {
            var showIndex = _usedLogIndex;
            _logArray[showIndex].SetText(text);
            // 一番下に表示されるようにHierarchyの順序を最後尾に移動
            _logArray[showIndex].transform.SetAsLastSibling();
            
            _totalLogCount++;
            
            // インデックスを更新
            // NOTE: 1を足した上で、配列の範囲内になるように剰余の計算を行う
            _usedLogIndex = (_usedLogIndex + 1) % _logArray.Length;
            
            // 念のためキル
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            
            // 配列が一周した場合、次に上書きされるログを非表示にする
            if (_totalLogCount > _logArray.Length - 1)
            {
                _sequence.Append(_logArray[_usedLogIndex].Hide());
            }
            
            _sequence.Append(_logArray[showIndex].Show());
        }

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            foreach (var log in _logArray)
            {
                log.Exit();
            }
            
            // インデックスとカウントをリセット
            _usedLogIndex = 0;
            _totalLogCount = 0;

            Dispose();
        }

        private void Dispose()
        {
            _sequence?.Kill();
            _sequence = null;
        }
    }
}