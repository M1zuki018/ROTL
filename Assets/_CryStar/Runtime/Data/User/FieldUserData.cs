using System;
using CryStar.Field.Enums;
using CryStar.Field.Event;
using CryStar.Utility;
using CryStar.Utility.Enum;
using UnityEngine;

namespace CryStar.Data.User
{
     /// <summary>
     /// フィールドのユーザーデータ
     /// </summary>
     [Serializable]
     public class FieldUserData : CachedUserDataBase
     {
          [SerializeField] private int _lastMapId = 1; // 初期マップ
          [SerializeField] private Vector2 _lastPosition = Vector2.zero;
          [SerializeField] private MoveDirectionType _directionType = MoveDirectionType.Down;
          
          /// <summary>
          /// 最終位置のマップID
          /// </summary>
          public int LastMapId => _lastMapId;

          /// <summary>
          /// プレイヤーの座標
          /// </summary>
          public Vector2 LastPosition => _lastPosition;

          /// <summary>
          /// プレイヤーが向いている方向
          /// </summary>
          public MoveDirectionType DirectionType => _directionType;

          /// <summary>
          /// コンストラクタ
          /// </summary>
          public FieldUserData(int userId) : base(userId)
          {
               _lastMapId = 1; // 初期マップ
               _lastPosition = Vector3.zero;
               _directionType = MoveDirectionType.Down;
               
               // 実行時用のディクショナリーを構築する
               BuildCache();
          }

          /// <summary>
          /// マップIDを更新
          /// </summary>
          public void TransitionMap(int newMapId)
          {
               // 1以下にならないようにする
               _lastMapId = Math.Max(newMapId, 1);
          }

          /// <summary>
          /// 最終位置と回転を更新する
          /// </summary>
          public void SetLastTranslation(Vector2 position, MoveDirectionType direction)
          {
               _lastPosition = position;
               _directionType = direction;
          }
          
          /// <summary>
          /// クリアデータを更新
          /// イベントをクリアした際に呼び出す
          /// </summary>
          public void AddClearData(FieldEventBase fieldEvent)
          {
               if (fieldEvent == null)
               {
                    LogUtility.Warning("フィールドイベントがnullです", LogCategory.System);
                    return;
               }
               
               AddData(fieldEvent.EventID);
          }

          /// <summary>
          /// イベントがクリア済みかチェック
          /// </summary>
          public bool IsEventCleared(FieldEventBase fieldEvent)
          {
               // クリアしたときに辞書に登録されるため、辞書にキーが存在するかを調べる
               return DataCache.ContainsKey(fieldEvent.EventID);
          }
          
     }
}