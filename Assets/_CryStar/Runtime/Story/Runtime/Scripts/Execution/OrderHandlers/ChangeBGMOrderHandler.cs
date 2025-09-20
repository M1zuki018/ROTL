using CryStar.Audio;
using CryStar.Core;
using CryStar.Story.Attributes;
using CryStar.Story.Data;
using CryStar.Story.Enums;
using CryStar.Story.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace CryStar.Story.Execution
{
    /// <summary>
    /// ChangeBGM - BGM変更
    /// </summary>
    [OrderHandler(OrderType.ChangeBGM)]
    public class ChangeBGMOrderHandler : OrderHandlerBase
    {
        /// <summary>
        /// AudioManager
        /// </summary>
        private AudioManager _audioManager;
        
        public override OrderType SupportedOrderType => OrderType.ChangeBGM;
        
        public override Tween HandleOrder(OrderData data, StoryView view)
        {
            if (_audioManager == null)
            {
                _audioManager = ServiceLocator.GetGlobal<AudioManager>();
            }
            _audioManager.CrossFadeBGM(data.FilePath, data.Duration).Forget();
            return null;
        }
    }
}