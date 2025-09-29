using System.Threading;
using CryStar.Attribute;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// バトルの行動ログを管理するクラス
    /// </summary>
    public class BattleLogContents : MonoBehaviour
    {
        [SerializeField, HighlightIfNull] 
        private ExecutionLog[] _logArray;
        
        /// <summary>
        /// ログの自動非表示時間（秒）
        /// </summary>
        [SerializeField]
        private float _autoHideDelay = 1f;
        
        /// <summary>
        /// 使用している末尾のログのインデックス
        /// </summary>
        private int _usedLogIndex = 0;
        
        /// <summary>
        /// 表示したログの総数
        /// </summary>
        private int _totalLogCount = 0;
        
        /// <summary>
        /// 各ログごとのCancellationTokenSourceの配列
        /// </summary>
        private CancellationTokenSource[] _ctsArray;

        #region Life cycle

        private void Awake()
        {
            // 各ログ用のCancellationTokenSourceを初期化
            _ctsArray = new CancellationTokenSource[_logArray.Length];
            for (int i = 0; i < _ctsArray.Length; i++)
            {
                _ctsArray[i] = new CancellationTokenSource();
            }
        }

        private void OnDestroy()
        {
            foreach (var cts in _ctsArray)
            {
                DisposeCancellationTokenSource(cts);
            }
        }

        #endregion
        
        /// <summary>
        /// テキストを設定
        /// </summary>
        public void SetLog(string text)
        {
            // インデックスをコピー
            var showIndex = _usedLogIndex;
            _logArray[showIndex].SetText(text);
            
            // 一番下に表示されるようにHierarchyの順序を最後尾に移動
            _logArray[showIndex].transform.SetAsLastSibling();
            
            _totalLogCount++;
            
            // インデックスを更新
            // NOTE: 1を足した上で、配列の範囲内になるように剰余の計算を行う
            _usedLogIndex = (_usedLogIndex + 1) % _logArray.Length;
            
            // 配列が一周した場合、次に上書きされるログを非表示にする
            if (_totalLogCount > _logArray.Length - 1)
            {
                // 上書きされるログの自動非表示をキャンセル
                DisposeCancellationTokenSource(_ctsArray[_usedLogIndex]);
                _ctsArray[_usedLogIndex] = new CancellationTokenSource();
                
                // 非表示アニメーションが終わった後に新しいログが表示されるようにする
                _logArray[_usedLogIndex].Hide().OnComplete(() =>
                {
                    _logArray[_usedLogIndex].gameObject.SetActive(false);
                    ShowNextLog(showIndex);
                });
                return;
            }

            ShowNextLog(showIndex);
            
        }

        /// <summary>
        /// ログをリセットする
        /// </summary>
        public void Reset()
        {
            // すべての自動非表示タスクをキャンセル
            for (int i = 0; i < _ctsArray.Length; i++)
            {
                DisposeCancellationTokenSource(_ctsArray[i]);
                _ctsArray[i] = new CancellationTokenSource();
            }
            
            foreach (var log in _logArray)
            {
                log.Exit();
            }
            
            // インデックスとカウントをリセット
            _usedLogIndex = 0;
            _totalLogCount = 0;
        }

        #region Private methods
        
        /// <summary>
        /// ログを表示する
        /// </summary>
        private void ShowNextLog(int showIndex)
        {
            DisposeCancellationTokenSource(_ctsArray[showIndex]);
            _ctsArray[showIndex] = new CancellationTokenSource();
            
            // 表示
            _logArray[showIndex].Show();
            
            // 自動非表示機能を呼び出す
            AutoHideAsync(showIndex, _ctsArray[showIndex].Token).Forget();
        }
        
        /// <summary>
        /// 一定時間後に自動で非表示にする
        /// </summary>
        private async UniTask AutoHideAsync(int logIndex, CancellationToken ct)
        {
            try
            {
                // 待機
                await UniTask.Delay(System.TimeSpan.FromSeconds(_autoHideDelay), cancellationToken: ct);
                
                // 非表示アニメーション
                _logArray[logIndex].Hide().OnComplete(() =>
                {
                    _logArray[logIndex].gameObject.SetActive(false);
                });
            }
            catch (System.OperationCanceledException)
            {
                // キャンセルされた場合は何もしない
            }
        }
        
        /// <summary>
        /// CancellationTokenSourceの購読解除用のヘルパーメソッド
        /// </summary>
        private void DisposeCancellationTokenSource(CancellationTokenSource cts)
        {
            cts?.Cancel();
            cts?.Dispose();
        }
        
        #endregion
    }
}