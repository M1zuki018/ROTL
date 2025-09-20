using iCON.Enums;

namespace iCON.UI
{
    /// <summary>
    /// タイトルシーンのCanvasManager
    /// </summary>
    public class TitleCanvasManager : SceneCanvasManagerBase
    {
        /// <summary>
        /// 指定したキャンバスの参照を取得する
        /// </summary>
        public WindowBase GetCanvas(TitleCanvasType canvasType)
        {
            return base.GetCanvas((int)canvasType);
        }
        
        /// <summary>
        /// キャンバスを切り替える
        /// </summary>
        public void ShowCanvas(TitleCanvasType canvasType)
        {
            base.ShowCanvas((int)canvasType);
        }

        /// <summary>
        /// 現在の画面の上に新しい画面をオーバーレイとして表示する
        /// </summary>
        public void PushCanvas(TitleCanvasType canvasType)
        {
            base.PushCanvas((int)canvasType);
        }
    }
}