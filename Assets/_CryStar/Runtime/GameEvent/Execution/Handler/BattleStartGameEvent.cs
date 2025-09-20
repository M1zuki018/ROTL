using CryStar.Core;
using CryStar.Core.Enums;
using CryStar.Data.Scene;
using CryStar.GameEvent.Attributes;
using CryStar.GameEvent.Data;
using CryStar.GameEvent.Enums;
using Cysharp.Threading.Tasks;
using iCON.System;

namespace CryStar.GameEvent.Execution
{
    /// <summary>
    /// BattleStart - バトル開始
    /// </summary>
    [GameEventHandler(GameEventType.BattleStart)]
    public class BattleStartGameEvent : GameEventHandlerBase
    {
        /// <summary>
        /// SceneLoader
        /// </summary>
        private SceneLoader _sceneLoader;
        
        /// <summary>
        /// シーン遷移時に必要なデータクラス
        /// </summary>
        private SceneTransitionData _battleTransitionData = new(SceneType.Battle, true, true);

        
        public override GameEventType SupportedGameEventType => GameEventType.BattleStart;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BattleStartGameEvent(InGameManager inGameManager) : base(inGameManager) { }
        
        /// <summary>
        /// バトル開始
        /// </summary>
        public override async UniTask HandleGameEvent(GameEventParameters parameters)
        {
            if (_sceneLoader == null)
            {
                // SceneLoaderの参照がない場合はサービスロケーターから取得
                _sceneLoader = ServiceLocator.GetGlobal<SceneLoader>();
            }

            if (_battleTransitionData == null)
            {
                // Titleシーンへの遷移データが存在しなければ作成
                _battleTransitionData = new SceneTransitionData(SceneType.Battle, true, true);
            }
            
            // シーン遷移を実行
            await _sceneLoader.LoadSceneAsync(_battleTransitionData);
        }
    }
}