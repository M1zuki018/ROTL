using System;
using System.Collections.Generic;
using System.Linq;
using CryStar.GameEvent.Attributes;
using CryStar.GameEvent.Enums;
using CryStar.GameEvent.Execution;
using CryStar.Story.Factory;
using iCON.System;

namespace CryStar.GameEvent.Factory
{
    /// <summary>
    /// Game Event Factory
    /// </summary>
    public static class GameEventFactory
    {
        /// <summary>
        /// ファクトリを初期化
        /// </summary>
        public static void Initialize()
        {
            HandlerFactoryBase<GameEventHandlerBase, GameEventType, GameEventHandlerAttribute>.Initialize();
        }

        /// <summary>
        /// 指定されたイベントタイプのハンドラーを生成
        /// </summary>
        public static GameEventHandlerBase CreateHandler(GameEventType eventType, params object[] constructorArgs)
        {
            return HandlerFactoryBase<GameEventHandlerBase, GameEventType, GameEventHandlerAttribute>.CreateHandler(eventType, constructorArgs);
        }

        /// <summary>
        /// 全てのハンドラーを生成
        /// </summary>
        public static Dictionary<GameEventType, GameEventHandlerBase> CreateAllHandlers(InGameManager gameManager)
        {
            return HandlerFactoryBase<GameEventHandlerBase, GameEventType, GameEventHandlerAttribute>
                .CreateAllHandlers(type => BuildConstructorArguments(type, gameManager));
        }

        /// <summary>
        /// 登録されているハンドラーの数を取得
        /// </summary>
        /// <returns></returns>
        public static int GetRegisteredGameEventCount()
        {
            return HandlerFactoryBase<GameEventHandlerBase, GameEventType, GameEventHandlerAttribute>
                .GetRegisteredHandlerCount();
        }
        
        /// <summary>
        /// 登録されているハンドラー数を取得
        /// </summary>
        public static int GetRegisteredHandlerCount()
        {
            return HandlerFactoryBase<GameEventHandlerBase, GameEventType, GameEventHandlerAttribute>
                .GetRegisteredHandlerCount();
        }

        /// <summary>
        /// 指定されたオーダータイプが登録済みかどうかを確認
        /// </summary>
        public static bool IsHandlerRegistered(GameEventType gameEventType)
        {
            return HandlerFactoryBase<GameEventHandlerBase, GameEventType, GameEventHandlerAttribute>
                .IsHandlerRegistered(gameEventType);
        }

        /// <summary>
        /// コンストラクタ引数を構築する
        /// </summary>
        private static object[] BuildConstructorArguments(Type handlerType, InGameManager gameManager)
        {
            var constructors = handlerType.GetConstructors();
            var constructor = constructors.OrderByDescending(c => c.GetParameters().Length).FirstOrDefault();

            if (constructor == null)
                return Array.Empty<object>();

            var parameters = constructor.GetParameters();
            var args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var paramType = parameters[i].ParameterType;
                
                if (paramType == typeof(InGameManager))
                {
                    args[i] = gameManager;
                }
                else if (paramType.IsValueType)
                {
                    args[i] = Activator.CreateInstance(paramType);
                }
                else
                {
                    args[i] = null;
                }
            }

            return args;
        }
    }
}
