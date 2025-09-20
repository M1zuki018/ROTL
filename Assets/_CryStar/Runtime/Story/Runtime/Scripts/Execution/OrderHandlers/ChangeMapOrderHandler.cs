using CryStar.Core;
using CryStar.Story.Attributes;
using CryStar.Story.Data;
using CryStar.Story.Enums;
using CryStar.Story.UI;
using DG.Tweening;
using iCON.System;

namespace CryStar.Story.Execution
{
    /// <summary>
    /// ChangeMap - マップ移動
    /// </summary>
    [OrderHandler(OrderType.ChangeMap)]
    public class ChangeMapOrderHandler : OrderHandlerBase
    {
        private InGameManager _gameManager;
        
        public override OrderType SupportedOrderType => OrderType.ChangeMap;

        public override Tween HandleOrder(OrderData data, StoryView view)
        {
            if (_gameManager == null)
            {
                // FieldManagerがnullの場合、もう一度サービスロケーターから取得を試す
                _gameManager = ServiceLocator.GetLocal<InGameManager>();
            }
            
            // マップを移動する
            _gameManager.RemoveAndShowMap((int)data.OverrideTextSpeed);
            return null;
        }
    }
}
