using CryStar.Core;
using CryStar.Core.Enums;
using CryStar.Data.Scene;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CryStar.Field.Event
{
    /// <summary>
    /// バトルを行うフィールドイベント
    /// </summary>
    public class FieldEvent_Battle : FieldEventBase
    {
        protected override void OnPlayerEnter(Collider2D playerCollider)
        {
            base.OnPlayerEnter(playerCollider);
            ServiceLocator.GetGlobal<SceneLoader>().LoadSceneAsync(new SceneTransitionData(SceneType.Battle, false, true)).Forget();
        }
    }
}