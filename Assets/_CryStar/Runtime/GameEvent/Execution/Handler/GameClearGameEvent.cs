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
    /// GameClear - ゲームクリア
    /// </summary>
    [GameEventHandler(GameEventType.GameClear)]
    public class GameClearGameEvent : GameEventHandlerBase
    {
        /// <summary>
        /// SceneLoader
        /// </summary>
        private SceneLoader _sceneLoader;
        
        /// <summary>
        /// シーン遷移時に必要なデータクラス
        /// </summary>
        private SceneTransitionData _titleTransitionData = new(SceneType.Title, true, true);
        
        public override GameEventType SupportedGameEventType => GameEventType.GameClear;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameClearGameEvent(InGameManager inGameManager) : base(inGameManager) { }
        
        /// <summary>
        /// ゲームクリア処理
        /// </summary>
        public override async UniTask HandleGameEvent(GameEventParameters parameters)
        {
            if (_sceneLoader == null)
            {
                // SceneLoaderの参照がない場合はサービスロケーターから取得
                _sceneLoader = ServiceLocator.GetGlobal<SceneLoader>();
            }

            if (_titleTransitionData == null)
            {
                // Titleシーンへの遷移データが存在しなければ作成
                _titleTransitionData = new SceneTransitionData(SceneType.Title, true, true);
            }
            
            // シーン遷移を実行
            await _sceneLoader.LoadSceneAsync(_titleTransitionData);
        }
    }
}
