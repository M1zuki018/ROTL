using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ColliderMapper))]
public class ColliderMapperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        GUILayout.Space(10);
        
        ColliderMapper mapper = (ColliderMapper)target;
        
        GUILayout.Label("Collider Generation", EditorStyles.boldLabel);
        
        GUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Generate Simple", GUILayout.Height(30)))
        {
            mapper.GenerateCollidersSimple();
            EditorUtility.SetDirty(mapper);
        }
        
        if (GUILayout.Button("Generate Optimized", GUILayout.Height(30)))
        {
            mapper.GenerateCollidersOptimized();
            EditorUtility.SetDirty(mapper);
        }
        
        GUILayout.EndHorizontal();
        
        if (GUILayout.Button("Clear All Colliders", GUILayout.Height(25)))
        {
            mapper.ClearColliders();
            EditorUtility.SetDirty(mapper);
        }
        
        GUILayout.Space(10);
        
        // テスト用CSV入力エリア
        GUILayout.Label("CSV Test Input", EditorStyles.boldLabel);
        
        if (csvInput == null)
        {
            csvInput = GetSampleCSV();
        }
        
        csvInput = GUILayout.TextArea(csvInput, GUILayout.Height(100));
        
        if (GUILayout.Button("Load from CSV Input"))
        {
            mapper.LoadFromCSV(csvInput);
            EditorUtility.SetDirty(mapper);
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("Reset to Sample Data"))
        {
            csvInput = GetSampleCSV();
        }
    }
    
    private string csvInput;
    
    private string GetSampleCSV()
    {
        return @"1,1,1,1,1,1,1,1,1,1,1,1,1,1,1
1,0,0,0,0,0,1,1,1,1,1,1,1,1,1
1,0,0,0,0,0,1,1,1,1,1,1,1,1,1
1,0,0,0,0,0,1,1,1,1,1,1,1,1,1
1,0,0,0,0,0,1,1,0,1,1,1,1,1,1
1,0,0,0,0,0,1,1,0,0,0,0,0,0,1
1,0,0,0,0,0,1,0,0,0,0,0,0,0,1
1,1,1,0,1,1,1,0,0,1,1,1,1,0,1
0,1,1,0,1,1,1,0,0,1,1,1,1,0,1
0,1,1,0,1,1,1,0,0,0,0,0,0,0,1
0,1,1,0,1,1,1,0,0,0,0,0,0,0,1
0,1,1,0,0,0,0,0,0,0,1,1,1,1,1
0,1,0,0,0,0,0,0,0,0,1,0,0,0,0
0,1,1,1,1,1,1,0,1,1,1,0,0,0,0
0,0,0,0,0,0,1,1,1,0,0,0,0,0,0";
    }
}