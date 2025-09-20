/// <summary>
/// MVP-C（Model CoordinatorManager Presenter with Coordinators）パターンを利用するときの
/// コーディネータークラスのベースクラス
/// </summary>
public class CoordinatorBase : WindowBase
{
    /// <summary>
    /// Canvasが切り替わり非表示になるときの処理
    /// </summary>
    public virtual void Enter()
    {
        Show();
    }
    
    /// <summary>
    /// Canvasが切り替わったときの処理
    /// </summary>
    public virtual void Exit()
    {
        Hide();
    }

    /// <summary>
    /// Canvasを閉じるなどのキャンセル処理
    /// NOTE: Updateで監視されて呼び出されている
    /// キャンセル処理がある継承先のクラスで実装を書く
    /// </summary>
    public virtual void Cancel(){ }
}
