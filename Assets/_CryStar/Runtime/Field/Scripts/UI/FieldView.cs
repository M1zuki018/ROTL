using System;
using System.Threading;
using CryStar.Attribute;
using CryStar.Utility;
using CryStar.Utility.Enum;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CryStar.Field.UI
{
    /// <summary>
    /// Field CoordinatorManager
    /// </summary>
    public class FieldView : MonoBehaviour
    {
        [Header("目標表示用の設定")]
        [SerializeField, HighlightIfNull] private CustomText _objectiveText;
        [SerializeField] private float _displayTime = 3f;
        private CancellationTokenSource _cts = new CancellationTokenSource();
        
        private void Awake()
        {
            if (_objectiveText == null)
            {
                LogUtility.Error("目標表示用のテキストコンポーネントがアサインされていません", LogCategory.UI, this);
                return;   
            }

            _objectiveText.enabled = false;
        }

        private void OnDestroy()
        {
            // キャンセレーショントークンの解放処理
            CancelCurrentOperation();
        }

        /// <summary>
        /// 目標表示を行う
        /// </summary>
        public async UniTask ShowObjectiveText(string message)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            
            try
            {
                _objectiveText.enabled = true;
                // テキストを更新
                _objectiveText.SetText(message);

                // キャンセル可能
                await UniTask.Delay(TimeSpan.FromSeconds(_displayTime), cancellationToken: _cts.Token);

                _objectiveText.enabled = false;
            }
            catch (OperationCanceledException)
            {
                // 連続で目標表示が行われた場合にキャンセル処理が行われる
                // 正常な動作なので、特にログなどは出さない
            }
            catch (Exception ex)
            {
                // 予期しない例外の場合はログ出力
                LogUtility.Error($"目標表示中に予期しないエラーが発生: {ex}");
                _objectiveText.enabled = false; // 確実に非表示にする
            }
        }

        /// <summary>
        /// 現在の操作をキャンセル
        /// </summary>
        private void CancelCurrentOperation()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
    }
}

