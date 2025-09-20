using CryStar.Settings.Data;
using UnityEditor;
using UnityEngine;

namespace CryStar.Settings.Editor
{
    /// <summary>
    /// ProductDataSO用のカスタムエディタ
    /// </summary>
    [CustomEditor(typeof(ProductDataSO))]
    public class ProductDataSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            var gameData = (ProductDataSO)target;

            EditorGUILayout.LabelField("Version Info", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Full Version:", gameData.GetFullVersionString());

            EditorGUILayout.Space();

            if (GUILayout.Button("Increment Build Number"))
            {
                gameData.IncrementBuildNumber();
            }


            if (GUILayout.Button("Sync to PlayerSettings"))
            {
                PlayerSettings.bundleVersion = gameData.Version;
                PlayerSettings.companyName = gameData.AppName;
                Debug.Log("Synced to PlayerSettings");
            }
        }
    }
}