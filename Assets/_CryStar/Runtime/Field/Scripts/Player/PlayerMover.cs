using CryStar.Core;
using CryStar.Core.ReactiveExtensions;
using CryStar.Core.UserData;
using CryStar.Data.User;
using CryStar.Field.Data;
using CryStar.Field.Enums;
using iCON.Enums;
using iCON.System;
using iCON.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CryStar.Field.Player
{
    /// <summary>
    /// プレイヤーの動きを管理するクラス
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMover : MonoBehaviour
    {
        /// <summary>
        /// スプライトアニメーションのコントローラー
        /// </summary>
        [SerializeField]
        private SpriteAnimationController _animController;

        /// <summary>
        /// アニメーションの画像を設定するスクリプタブルオブジェクト
        /// </summary>
        [SerializeField]
        private PlayerAnimationSetting _animationSetting;
        
        /// <summary>
        /// 移動速度
        /// </summary>
        [SerializeField]
        private float _moveSpeed = 5f;

        /// <summary>
        /// ダッシュ時の移動速度にかかる倍率
        /// </summary>
        [SerializeField] 
        private float _dashMultiply = 1.5f;
        
        /// <summary>
        /// 移動のInputActionReference
        /// </summary>
        [Header("入力関連の設定")] 
        [SerializeField]
        private InputActionReference _moveInput;
        
        /// <summary>
        /// ダッシュ切り替えのInputActionReference
        /// </summary>
        [SerializeField] 
        private InputActionReference _dashInput;
        
        /// <summary>
        /// プレイヤーの入力を管理するクラス
        /// </summary>
        private PlayerMoveInput _input;
        
        /// <summary>
        /// 現在向いている方向
        /// </summary>
        private MoveDirectionType _directionType;
        
        /// <summary>
        /// Rigidbody2Dコンポーネント
        /// </summary>
        private Rigidbody2D _rigidbody2D;
        
        /// <summary>
        /// 現在の移動入力
        /// </summary>
        private Vector2 _currentMoveInput;

        /// <summary>
        /// 現在の移動速度
        /// </summary>
        private float _currentMoveSpeed;
        
        /// <summary>
        /// 行動可能か
        /// </summary>
        private bool _canMove;
        
        private UserDataManager _userDataManager;
        
        /// <summary>
        /// InGameManagerのCurrentStateリアクティブプロパティの監視を解除するためのCompositeDisposable
        /// </summary>
        private CompositeDisposable _disposable = new CompositeDisposable();
        
        /// <summary>
        /// フィールドユーザーデータ
        /// </summary>
        private FieldUserData FieldUserData => _userDataManager.CurrentUserData.FieldUserData;

        #region Life cycle

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
            // 重力を無効化
            _rigidbody2D.gravityScale = 0f;
            
            // 回転を固定
            _rigidbody2D.freezeRotation = true;

            // 移動速度をSerializeFieldで設定した速度に設定
            _currentMoveSpeed = _moveSpeed;
        }

        /// <summary>
        /// Start
        /// </summary>
        private void Start()
        {
            // 初期化
            Initialize();

            // 入力を受け取るクラスを生成
            _input = new PlayerMoveInput(_moveInput, _dashInput, UpdateDirection, HandleDash, HandleDashCancel);
            
            // InGameの状態を監視してStoryの時に行動制限をかけるようにしたいので
            // InGameManagerのリアクティブプロパティを購読する
            var inGameManager = ServiceLocator.GetLocal<InGameManager>();
            inGameManager.CurrentStateProp.Subscribe(ChangeState).AddTo(_disposable);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialize()
        {
            // セーブデータを読み込んで初期位置を設定
            _userDataManager = ServiceLocator.GetGlobal<UserDataManager>();
            transform.position = FieldUserData.LastPosition;
            _directionType = FieldUserData.DirectionType;
            
            // 向いている方向にあわせてSpriteを変更
            ChangeAnimationSprites();
            
            // セーブ時のイベントを購読
            _userDataManager.OnExecuteSaveEvent += OnExecuteSaveEvent;
        }

        /// <summary>
        /// FixedUpdate
        /// </summary>
        private void FixedUpdate()
        {
            // Rigidbodyのvelocityを設定して移動
            Vector2 velocity = _currentMoveInput * _currentMoveSpeed;
            _rigidbody2D.linearVelocity = velocity;
        }
        
        /// <summary>
        /// Destroy
        /// </summary>
        private void OnDestroy()
        {
            // 入力購読を破棄
            _input.Dispose();
            _disposable?.Dispose();

            if (_userDataManager != null)
            {
                _userDataManager.OnExecuteSaveEvent -= OnExecuteSaveEvent;
            }
        }

        #endregion

        /// <summary>
        /// InGameの状態が変更されたときに呼ばれる処理
        /// </summary>
        private void ChangeState(InGameStateType state)
        {
            if (state == InGameStateType.Field)
            {
                // Fieldの場合は動くことができる
                _canMove = true;
                return;
            }
            
            // それ以外の場合は動けない
            _canMove = false;
            
            // 移動が止まるように各変数もリセットする
            _currentMoveInput = Vector2.zero;
            _rigidbody2D.linearVelocity = _currentMoveInput;
        }
        
        /// <summary>
        /// 入力に基づいて方向を更新
        /// </summary>
        private void UpdateDirection(InputAction.CallbackContext ctx)
        {
            if (!_canMove)
            {
                return;
            }
            
            _currentMoveInput = ctx.ReadValue<Vector2>();
            
            if (_currentMoveInput == Vector2.zero)
            {
                // 入力がない場合は向きの変更は行わない
                return;
            }
            
            // 方向を判定して更新
            var newDirection = DetermineDirection(_currentMoveInput);
            if (_directionType != newDirection)
            {
                _directionType = newDirection;
                ChangeAnimationSprites();
            }
        }
        
        /// <summary>
        /// 入力ベクトルから方向を判定
        /// </summary>
        private MoveDirectionType DetermineDirection(Vector2 input)
        {
            // 水平移動が優先（左右の判定）
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                return input.x > 0 ? MoveDirectionType.Right : MoveDirectionType.Left;
            }
            // 垂直移動（上下の判定）
            else
            {
                return input.y > 0 ? MoveDirectionType.Up : MoveDirectionType.Down;
            }
        }

        /// <summary>
        /// アニメーションの画像を差し替える
        /// </summary>
        private void ChangeAnimationSprites()
        {
            var sprites = _animationSetting.GetSprites(_directionType);
        
            // nullの可能性があるので確認
            if (sprites != null)
            {
                _animController.ChangeSprites(sprites);
            }
        }

        /// <summary>
        /// ダッシュ開始（長押ししている間だけダッシュする）
        /// </summary>
        private void HandleDash(InputAction.CallbackContext ctx)
        {
            if (!_canMove)
            {
                return;
            }

            // デフォルトの速度にダッシュ時の倍率をかけたものをスピードに設定する
            _currentMoveSpeed = _moveSpeed * _dashMultiply;
        }

        /// <summary>
        /// ダッシュキャンセル
        /// </summary>
        private void HandleDashCancel(InputAction.CallbackContext ctx)
        {
            // 速度をデフォルトに戻す
            _currentMoveSpeed = _moveSpeed;
        }
        
        /// <summary>
        /// セーブ時に呼び出す処理
        /// </summary>
        private void OnExecuteSaveEvent()
        {
            // 最終位置を保存
            FieldUserData.SetLastTranslation(transform.position, _directionType);
        }
    }
}