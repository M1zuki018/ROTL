using System.Collections.Generic;
using CryStar.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// MVP-Cパターンのマネージャーの基底クラス
/// </summary>
public abstract class CoordinatorManagerBase : CustomBehaviour
{
    [SerializeField] protected List<CoordinatorBase> _coordinators = new List<CoordinatorBase>();
    [SerializeField] protected int _defaultIndex = 1;
    [SerializeField] protected bool _defalutOpen = true;
    
    protected Stack<int> _coordinatorStack = new Stack<int>(); // スタック管理用
    protected int _currentIndex = -1; // 現在アクティブなコーディネーターのインデックス
    
    /// <summary>
    /// 現在アクティブなコーディネーターを取得する
    /// </summary>
    public CoordinatorBase CurrentCoordinator => _coordinators[_currentIndex];
    
    public override UniTask OnStart()
    {
        if (_defalutOpen)
        {
            TransitionTo(_defaultIndex);
        }
        
        return base.OnStart();
    }

    /// <summary>
    /// 指定したキャンバスを表示し、それ以外を非表示にする
    /// </summary>
    public virtual void TransitionTo(int coordinatorIndex)
    {
        // インデックスの範囲チェック
        if (coordinatorIndex < 0 || coordinatorIndex >= _coordinators.Count)
        {
            Debug.LogError($"コーディネーターインデックスが範囲外です: {coordinatorIndex}");
            return;
        }

        if (_currentIndex >= 0)
        {
            // 現在のコーディネーターの切り替わり処理を実行
            // NOTE: 初期値を-1にしているためエラーにならないように条件文を書いている
            _coordinators[_currentIndex]?.Exit();
        }
        
        // 全てのコーディネーターを非アクティブ化
        foreach (var coordinator in _coordinators)
        {
            coordinator?.Exit();
        }
        
        // スタックをクリアして新しいインデックスをプッシュ
        _coordinatorStack.Clear();
        _coordinatorStack.Push(coordinatorIndex);
        
        // 現在のインデックスを更新
        _currentIndex = coordinatorIndex;
        
        // 新しいコーディネーターの開始処理
        _coordinators[coordinatorIndex]?.Enter();
    }

    
    /// <summary>
    /// 現在の画面の上に新しい画面をオーバーレイとして表示する
    /// </summary>
    public virtual void PushCoordinator(int coordinatorIndex)
    {
        // インデックスの範囲チェック
        if (coordinatorIndex < 0 || coordinatorIndex >= _coordinators.Count)
        {
            Debug.LogError($"コーディネーターインデックスが範囲外です: {coordinatorIndex}");
            return;
        }
        
        // 同じキャンバスが最上位に既に表示されている場合は何もしない
        if (_currentIndex == coordinatorIndex)
            return;
        
        // 現在のコーディネーターの切り替わり処理を実行
        _coordinators[_currentIndex]?.Exit();
        
        // キャンバス切り替え
        for (int i = 0; i < _coordinators.Count; i++)
        {
            if (i != coordinatorIndex)
            {
                // 対象以外のコーディネーターは反応しないようにする
                _coordinators[i]?.Block();
            }
        }
        
        _coordinatorStack.Push(coordinatorIndex); // スタックに新しいインデックスをプッシュ
        _currentIndex = coordinatorIndex; // 現在のインデックスを更新
        _coordinators[coordinatorIndex].Enter(); // 新しいコーディネーターの切り替わり処理を実行
    }

    /// <summary>
    /// 最上位の画面を閉じて、一つ前の画面に戻る
    /// </summary>
    public virtual void PopCoordinator()
    {
        // スタックが空または1つしかない場合は何もしない
        if (_coordinatorStack.Count <= 1)
        {
            Debug.Log("これ以上前の画面に戻れません");
            return;
        }
        
        int currentIndex = _coordinatorStack.Pop(); // 現在の画面をポップ
        int previousIndex = _coordinatorStack.Peek(); // 一つ前の画面を取得
        
        // 現在のコーディネーターの切り替わり処理を実行
        _coordinators[currentIndex]?.Exit();
        _coordinators[previousIndex]?.Unblock();　// 一つ前の画面のブロックを解除
        
        _currentIndex = previousIndex; // 現在のインデックスを更新
        _coordinators[previousIndex].Enter(); // 新しいコーディネーターの切り替わり処理を実行
    }
    
    /// <summary>
    /// 特定のインデックスまでスタックをポップする
    /// </summary>
    public virtual void PopToCoordinator(int targetIndex)
    {
        // スタックに目的のインデックスがない場合
        if (!IsCoordinatorInStack(targetIndex))
        {
            Debug.LogError($"指定されたインデックス {targetIndex} はスタック内に存在しません");
            return;
        }
        
        // 目的のインデックスが最上位の場合は何もしない
        if (_currentIndex == targetIndex)
            return;
            
        // 現在のコーディネーターの切り替わり処理を実行
        _coordinators[_currentIndex]?.Exit();
            
        // 目的のインデックスが出てくるまでポップして非表示にする
        while (_coordinatorStack.Count > 0 && _coordinatorStack.Peek() != targetIndex)
        {
            int index = _coordinatorStack.Pop();
            _coordinators[index]?.Exit();
        }
        
        // 目的のキャンバスをUnblockする
        _coordinators[targetIndex]?.Unblock();
        
        _currentIndex = targetIndex; // 現在のインデックスを更新
        _coordinators[targetIndex].Enter(); // 新しいコーディネーターの切り替わり処理を実行
    }

    /// <summary>
    /// 全てのコーディネーターを非アクティブ化する
    /// </summary>
    public virtual void AllHide()
    {
        foreach (var coordinator in _coordinators)
        {
            coordinator?.Exit();
        }
        
        // スタックをクリア
        _coordinatorStack.Clear();

        _currentIndex = -1;
    }
    
    /// <summary>
    /// スタック内に特定のキャンバスが存在するかチェック
    /// </summary>
    protected bool IsCoordinatorInStack(int index)
    {
        return _coordinatorStack.Contains(index);
    }
    
    /// <summary>
    /// 現在のキャンバスインデックスを取得
    /// </summary>
    public int GetCurrentCoordinatorIndex()
    {
        return _currentIndex;
    }

    /// <summary>
    /// コーディネーターの参照を取得する
    /// </summary>
    public CoordinatorBase GetCoordinator(int index)
    {
        return _coordinators[index];
    }
}
