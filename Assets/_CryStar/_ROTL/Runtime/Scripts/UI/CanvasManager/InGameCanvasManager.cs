using iCON.Enums;

namespace iCON.UI
{
    /// <summary>
    /// インゲームシーンのCanvasManager
    /// </summary>
    public class InGameCanvasManager : SceneCanvasManagerBase
    {
        /// <summary>
        /// 指定したキャンバスの参照を取得する
        /// </summary>
        public WindowBase GetCanvas(InGameCanvasType canvasType)
        {
            return base.GetCanvas((int)canvasType);
        }
        
        /// <summary>
        /// キャンバスを切り替える
        /// </summary>
        public void ShowCanvas(InGameCanvasType canvasType)
        {
            base.ShowCanvas((int)canvasType);
        }

        /// <summary>
        /// 現在の画面の上に新しい画面をオーバーレイとして表示する
        /// </summary>
        public void PushCanvas(InGameCanvasType canvasType)
        {
            base.PushCanvas((int)canvasType);
        }
    }
}