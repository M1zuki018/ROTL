using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CryStar.Boot.UI
{
    /// <summary>
    /// 開始シーンを選択するためのドロップダウンコンポーネント
    /// </summary>
    [RequireComponent(typeof(Dropdown))]
    public class SceneSelectionDropdown : MonoBehaviour
    {
        /// <summary>
        /// ドロップダウンから除外したいシーン名のリスト
        /// </summary>
        private static readonly HashSet<string> ExcludedSceneNames = new HashSet<string>
        {
            "Bootstrap",
            "Load"
        };
        
        /// <summary>
        /// ドロップダウンの参照
        /// </summary>
        private Dropdown _dropdown;
        
        /// <summary>
        /// ドロップダウンのインデックスと実際のビルドインデックスのマッピング
        /// </summary>
        private readonly List<int> _dropdownToBuildIndexMap = new List<int>();
        
        /// <summary>
        /// 現在選択されているシーンのビルドインデックス
        /// </summary>
        public int SelectedSceneIndex => GetSelectedBuildIndex();
        
        #region Life cycle
        
        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            InitializeDropdown();
        }
        
        #endregion
        
        /// <summary>
        /// ドロップダウンの初期化
        /// </summary>
        private void InitializeDropdown()
        {
            _dropdown = GetComponent<Dropdown>();
            
            // 選択肢の設定
            PopulateDropdownOptions();
            
            // EventHandlerの登録
            SetupEventHandlers();
            
            // ドロップダウンを閉じた状態に設定する
            _dropdown.Hide();
            if (_dropdown.template != null)
            {
                _dropdown.template.gameObject.SetActive(false);
            }
        }

        #region ドロップダウンの初期化
        
        /// <summary>
        /// ドロップダウンの選択肢を設定
        /// </summary>
        private void PopulateDropdownOptions()
        {
            // ドロップダウンのオプションとIndexMapを初期化しておく
            _dropdown.options.Clear();
            _dropdownToBuildIndexMap.Clear();
            
            // ビルドセッティングに登録されているシーンの数だけ反復処理を行う
            for (int buildIndex = 0; buildIndex < SceneManager.sceneCountInBuildSettings; buildIndex++)
            {
                // シーン名取得
                string sceneName = GetSceneNameFromBuildIndex(buildIndex);
                
                if (IsSceneSelectable(sceneName))
                {
                    // ドロップダウン・IndexMapに追加
                    _dropdown.options.Add(new Dropdown.OptionData(sceneName));
                    _dropdownToBuildIndexMap.Add(buildIndex);
                }
            }
            
            // 初期選択を最初の有効なシーンに設定
            if (_dropdown.options.Count > 0)
            {
                _dropdown.value = 0;
            }
        }
        
        /// <summary>
        /// ビルドインデックスからシーン名を取得
        /// </summary>
        private string GetSceneNameFromBuildIndex(int buildIndex)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(buildIndex);
            return Path.GetFileNameWithoutExtension(scenePath);
        }
        
        /// <summary>
        /// 選択したくないシーンのリストの中に含まれていないかチェックする
        /// </summary>
        private bool IsSceneSelectable(string sceneName)
        {
            return !ExcludedSceneNames.Contains(sceneName);
        }
        
        #endregion
        
        /// <summary>
        /// イベントハンドラーの設定
        /// </summary>
        private void SetupEventHandlers()
        {
            _dropdown.onValueChanged.RemoveAllListeners();
            _dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }
        
        /// <summary>
        /// ドロップダウンの値変更時の処理
        /// </summary>
        private void OnDropdownValueChanged(int dropdownIndex)
        {
            // 特に何もしない（SelectedSceneIndexプロパティで取得される）
        }
        
        /// <summary>
        /// 現在選択されているビルドインデックスを取得
        /// </summary>
        private int GetSelectedBuildIndex()
        {
            if (_dropdown == null || _dropdown.value >= _dropdownToBuildIndexMap.Count)
            {
                return 0;
            }
            
            return _dropdownToBuildIndexMap[_dropdown.value];
        }
    }
}