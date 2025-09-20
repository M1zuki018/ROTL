using CryStar.Attribute;
using CryStar.Audio;
using CryStar.Core;
using CryStar.Core.Enums;
using CryStar.Core.UserData;
using CryStar.Data.Scene;
using CryStar.Utility;
using CryStar.Utility.Enum;
using Cysharp.Threading.Tasks;
using iCON.Constants;
using iCON.Enums;
using iCON.Performance;
using iCON.UI;
using UnityEditor;
using UnityEngine;

namespace iCON.System
{
    /// <summary>
    /// タイトルシーンのマネージャー
    /// </summary>
    public class TitleSceneManager : MonoBehaviour
    {
        /// <summary>
        /// タイトルスプラッシュの演出マネージャー
        /// </summary>
        [SerializeField, HighlightIfNull]
        private TitleSplashManager _titleSplashManager;

        /// <summary>
        /// CanvasManager
        /// </summary>
        [SerializeField]
        private TitleCanvasManager _canvasManager;
        
        /// <summary>
        /// Title用BGMのパス
        /// </summary>
        [SerializeField] 
        private string _bgmPath;
        
        /// <summary>
        /// BGMのフェードアウト
        /// </summary>
        [SerializeField]
        private float _bgmFadeDuration = 2f;

        /// <summary>
        /// AudioManager
        /// </summary>
        private AudioManager _audioManager;
        
        /// <summary>
        /// Title
        /// </summary>
        private CanvasController_Title _titleCC;
        
        /// <summary>
        /// コンフィグ
        /// </summary>
        private CanvasController_Config _configCC;

        /// <summary>
        /// ゲーム終了確認
        /// </summary>
        private CanvasController_QuitConfirm _quitConfirmCC;
        
        /// <summary>
        /// Playモードを選択するパネル
        /// </summary>
        private CanvasController_PlayStyleSelect _playStyleSelectCC;
        
        /// <summary>
        /// ユーザーデータマネージャー
        /// </summary>
        private UserDataManager _userDataManager;
        
        /// <summary>
        /// タイトルスプラッシュ完了済み
        /// </summary>
        private bool _isSplashCompleted;

        #region Life cycle

        /// <summary>
        /// Start
        /// </summary>
        private void Start()
        {
            if (ValidateComponents())
            {
                // コンポーネントの検証を行う
                return;
            }
            
            _audioManager = ServiceLocator.GetGlobal<AudioManager>();
            _userDataManager = ServiceLocator.GetGlobal<UserDataManager>();
            
            ShowTitleCanvas();
            
            // タイトルスプラッシュのゲームオブジェクトを確実にアクティブにしておく
            _titleSplashManager.gameObject.SetActive(true);
            _titleSplashManager.SetupEndAction(OnSplashCompleted);
            
            // タイトルスプラッシュの演出を開始
            _titleSplashManager.Play();
        }

        /// <summary>
        /// Update
        /// </summary>
        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space) || UnityEngine.Input.GetKeyDown(KeyCode.Return))
            {
                _titleSplashManager.Skip();
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                if (_canvasManager.CurrentCanvas == _configCC // コンフィグ
                    || _canvasManager.CurrentCanvas == _playStyleSelectCC // プレイモード選択画面
                    || _canvasManager.CurrentCanvas == _quitConfirmCC) // ゲーム終了画面
                {
                    _canvasManager.PopCanvas();
                }
            }
        }

        private void OnDestroy()
        {
            UnbindTitleCanvasEvents();
        }

        #endregion
        
        /// <summary>
        /// スプラッシュ演出完了時の処理
        /// </summary>
        private void OnSplashCompleted() => PlayTitleBGMAsync().Forget();

        /// <summary>
        /// ゲームを最初から始めるボタンクリック時の処理
        /// </summary>
        private void OnNewGameButtonClicked() => PushPlayStyleSelectCanvas();

        /// <summary>
        /// ゲームを始める
        /// </summary>
        private void PlayGame()
        {
            CheckSaveData();
            TransitionToInGameAsync().Forget();
        }

        /// <summary>
        /// ゲームを続きから始めるボタンクリック時の処理
        /// </summary>
        private void OnLoadGameButtonClicked()
        {
            // TODO: セーブデータのスロットを選択できるようにする可能性があるので、一旦メソッドは分けておく
            CheckSaveData();
            TransitionToInGameAsync().Forget();
        }

        /// <summary>
        /// ゲーム終了ボタンクリック時の処理
        /// </summary>
        private void OnQuitGameButtonClicked() => PushQuitConfirm();
        
        /// <summary>
        /// 設定画面を開くボタンクリック時の処理
        /// </summary>
        private void OnConfigButtonClicked() => ShowConfigCanvas();

        /// <summary>
        /// ゲーム終了ボタンクリック時の処理
        /// </summary>
        private void OnQuit()
        {
#if UNITY_EDITOR
            // エディタ実行中の場合
            EditorApplication.isPlaying = false;
#else
    // ビルド済みアプリケーションの場合
    Application.Quit();
#endif
        }
        
        /// <summary>
        /// ゲーム終了処理をキャンセルする処理
        /// </summary>
        private void OnQuitCancel() => _canvasManager.PopCanvas(); 
        
        /// <summary>
        /// コンポーネントの検証
        /// </summary>
        private bool ValidateComponents()
        {
            if (_titleSplashManager == null)
            {
                LogUtility.Error("TitleSplashManagerがアサインされていません", LogCategory.System);
                return true;
            }

            if (_canvasManager == null)
            {
                LogUtility.Error("Canvas Controllerがアサインされていません", LogCategory.System);
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// TitleBGMを再生する
        /// </summary>
        private async UniTask PlayTitleBGMAsync()
        {
            if (_isSplashCompleted)
            {
                // 既にスプラッシュを終えてBGMを流していたら以降の処理は行わない
                return;
            }
            
            if (_bgmPath == null || string.IsNullOrEmpty(_bgmPath))
            {
                return;
            }
            
            // 二度処理が行われないようにフラグを立てる
            _isSplashCompleted = true;

            if (_audioManager != null)
            {
                await _audioManager.PlayBGMWithFadeIn(_bgmPath, _bgmFadeDuration);                
            }
        }

        /// <summary>
        /// インゲームシーンへ遷移する
        /// </summary>
        private async UniTask TransitionToInGameAsync()
        {
            if (_audioManager != null)
            {
                await _audioManager.PlaySE(KSEPath.Select, 1);
            
                // BGMのフェードアウト後にシーン遷移
                await _audioManager.FadeOutBGM(_bgmFadeDuration);    
            }
            
            await ServiceLocator.GetGlobal<SceneLoader>().LoadSceneAsync(new SceneTransitionData(SceneType.InGame, true));
        }
        
        /// <summary>
        /// セーブデータを選ぶ
        /// </summary>
        private void CheckSaveData()
        {
            // TODO: 仮実装
            if (!_userDataManager.TrySelectUserData(0))
            {
                _userDataManager.CreateUserData();
            }
        }

        /// <summary>
        /// CanvasControllerのボタン押下処理を追加
        /// </summary>
        private void BindTitleCanvasEvents()
        {
            if (_titleCC != null)
            {
                _titleCC.OnNewGameButtonClicked += OnNewGameButtonClicked;
                _titleCC.OnLoadGameButtonClicked += OnLoadGameButtonClicked;
                _titleCC.OnConfigButtonClicked += OnConfigButtonClicked;
                _titleCC.OnQuitButtonClicked += OnQuitGameButtonClicked;
            }
        }

        private void BindQuitConfirmEvents()
        {
            if (_quitConfirmCC != null)
            {
                _quitConfirmCC.OnYesButtonClicked += OnQuit;
                _quitConfirmCC.OnNoButtonClicked += OnQuitCancel;
            }
        }

        private void BindPlayStyleSelectEvents()
        {
            if (_playStyleSelectCC != null)
            {
                _playStyleSelectCC.OnStoryModeButtonClicked += PlayGame;
                _playStyleSelectCC.OnBattleModeButtonClicked += PlayGame;
            }
        }
        
        /// <summary>
        /// CanvasControllerのボタン押下処理を解除
        /// </summary>
        private void UnbindTitleCanvasEvents()
        {
            if (_titleCC != null)
            {
                _titleCC.OnNewGameButtonClicked -= OnNewGameButtonClicked;
                _titleCC.OnLoadGameButtonClicked -= OnLoadGameButtonClicked;
                _titleCC.OnConfigButtonClicked -= OnConfigButtonClicked;
                _titleCC.OnQuitButtonClicked -= OnQuitGameButtonClicked;
            }
            
            if (_quitConfirmCC != null)
            {
                _quitConfirmCC.OnYesButtonClicked -= OnQuit;
                _quitConfirmCC.OnNoButtonClicked -= OnQuitCancel;
            }

            if (_playStyleSelectCC != null)
            {
                _playStyleSelectCC.OnStoryModeButtonClicked -= PlayGame;
                _playStyleSelectCC.OnBattleModeButtonClicked -= PlayGame;
            }
        }
        
        /// <summary>
        /// タイトル画面を表示する
        /// </summary>
        private void ShowTitleCanvas()
        {
            if (_titleCC == null)
            {
                _titleCC = _canvasManager.GetCanvas(TitleCanvasType.Title) as CanvasController_Title;
            }

            _canvasManager.ShowCanvas(TitleCanvasType.Title);
            BindTitleCanvasEvents();
        }

        /// <summary>
        /// コンフィグ画面を表示する
        /// </summary>
        private void ShowConfigCanvas()
        {
            if (_configCC == null)
            {
                _configCC = _canvasManager.GetCanvas(TitleCanvasType.Config) as CanvasController_Config;
            }
            
            _canvasManager.PushCanvas(TitleCanvasType.Config);
        }

        /// <summary>
        /// ゲーム終了確認パネルを表示する
        /// </summary>
        private void PushQuitConfirm()
        {
            if (_quitConfirmCC == null)
            {
                _quitConfirmCC = _canvasManager.GetCanvas(TitleCanvasType.QuitConfirm) as CanvasController_QuitConfirm;
            }

            _canvasManager.PushCanvas(TitleCanvasType.QuitConfirm);
            BindQuitConfirmEvents();
        }

        /// <summary>
        /// プレイモード変更パネルを表示する
        /// </summary>
        private void PushPlayStyleSelectCanvas()
        {
            if (_playStyleSelectCC == null)
            {
                _playStyleSelectCC = _canvasManager.GetCanvas(TitleCanvasType.PlayStyleSelect) as CanvasController_PlayStyleSelect;
            }

            _canvasManager.PushCanvas(TitleCanvasType.PlayStyleSelect);
            BindPlayStyleSelectEvents();
        }
    }
}