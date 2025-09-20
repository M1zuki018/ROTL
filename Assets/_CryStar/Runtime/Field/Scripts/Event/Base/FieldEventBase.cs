using CryStar.Core;
using CryStar.Core.UserData;
using CryStar.Data.User;
using CryStar.Field.Enums;
using UnityEngine;

namespace CryStar.Field.Event
{
    /// <summary>
    /// フィールドイベントのベースクラス
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public abstract class FieldEventBase : MonoBehaviour
    {
        /// <summary>
        /// イベントID
        /// </summary>
        [SerializeField]
        private int _eventID;
        
        /// <summary>
        /// インタラクション後の振る舞い
        /// </summary>
        [Header("インタラクション設定")]
        [SerializeField] 
        private InteractionBehaviorType _behaviorType;
        
        /// <summary>
        /// Collider2D
        /// </summary>
        private Collider2D _col;

        /// <summary>
        /// イベントが発火した回数
        /// </summary>
        private int _count = 0;
        
        /// <summary>
        /// ユーザーデータ
        /// </summary>
        private UserDataManager _userDataManager;
        
        /// <summary>
        /// イベントID
        /// </summary>
        public int EventID => _eventID;
        
        /// <summary>
        /// インタラクション後の振る舞い
        /// </summary>
        protected InteractionBehaviorType BehaviorType => _behaviorType;
        
        /// <summary>
        /// イベントが発火した回数
        /// </summary>
        protected int Count => _count;
        
        /// <summary>
        /// フィールドユーザーデータ
        /// </summary>
        private FieldUserData FieldUserData => _userDataManager.CurrentUserData.FieldUserData;

        #region Life cycle

        /// <summary>
        /// Start
        /// </summary>
        protected virtual void Start()
        {
            _userDataManager = ServiceLocator.GetGlobal<UserDataManager>();
            
            // コライダーを取得。トリガーに設定しておく
            if (TryGetComponent(out _col))
            {
                _col.isTrigger = true;
            }
        }

        #endregion
        
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            // プレイヤーか判定
            if (!IsValidPlayer(other))
            {
                return;
            }
            
            OnPlayerEnter(other);
            PostExecute();
        }
        
        /// <summary>
        /// プレイヤーかどうかを判定
        /// </summary>
        protected virtual bool IsValidPlayer(Collider2D other)
        {
            return other.CompareTag("Player");
        }
        
        /// <summary>
        /// プレイヤーが範囲に入った時の処理（継承先でオーバーライド）
        /// </summary>
        protected virtual void OnPlayerEnter(Collider2D playerCollider) { }
        
        /// <summary>
        /// イベント実行後の処理
        /// </summary>
        protected virtual void PostExecute()
        {
            _count++;
            
            switch (_behaviorType)
            {
                case InteractionBehaviorType.OneTime:
                    Destroy(gameObject);
                    break;
            }

            if (_userDataManager == null)
            {
                // ユーザーデータマネージャーが取得できていなかったら取得しておく
                _userDataManager = ServiceLocator.GetGlobal<UserDataManager>();
            }
            
            // クリアしたことを記録する
            FieldUserData.AddData(_eventID);
        }

        protected bool Equals(FieldEventBase other)
        {
            return base.Equals(other) && _eventID == other._eventID && _behaviorType == other._behaviorType && Equals(_col, other._col);
        }

        public override int GetHashCode()
        {
            return _eventID.GetHashCode();
        }
    }
}