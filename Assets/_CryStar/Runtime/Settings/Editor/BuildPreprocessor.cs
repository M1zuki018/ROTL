#if UNITY_EDITOR
using CryStar.Settings.Data;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace CryStar.Settings.Editor
{
    /// <summary>
    /// ビルド前処理でProductDataSOのビルド番号を自動更新
    /// </summary>
    public class BuildPreprocessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            // プロジェクト内のすべてのProductDataSOを検索
            string[] guids = AssetDatabase.FindAssets("t:ProductDataSO");
        
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                ProductDataSO productData = AssetDatabase.LoadAssetAtPath<ProductDataSO>(assetPath);
            
                if (productData != null)
                {
                    // ビルド番号をインクリメント
                    productData.IncrementBuildNumber();
                }
            }
        
            // アセットの変更を保存
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
#endif