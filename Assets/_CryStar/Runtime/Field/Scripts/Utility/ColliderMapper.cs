using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct ColliderRect
{
    public int x, y, width, height;
    
    public ColliderRect(int x, int y, int width, int height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }
}

public class ColliderMapper : MonoBehaviour
{
    [Header("Map Settings")]
    [SerializeField] private Vector2 cellSize = Vector2.one;
    [SerializeField] private Vector2 mapOffset = Vector2.zero;
    [SerializeField] private bool showPreview = true;
    
    [Header("Test Data")]
    [SerializeField] private int mapWidth = 15;
    [SerializeField] private int mapHeight = 15;
    
    // テスト用の配列データ（複雑な建物レイアウト）
    private int[,] testMapData = new int[,]
    {
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,0,0,0,0,0,1,1,1,1,1,1,1,1,1},
        {1,0,0,0,0,0,1,1,1,1,1,1,1,1,1},
        {1,0,0,0,0,0,1,1,1,1,1,1,1,1,1},
        {1,0,0,0,0,0,1,1,0,1,1,1,1,1,1},
        {1,0,0,0,0,0,1,1,0,0,0,0,0,0,1},
        {1,0,0,0,0,0,1,0,0,0,0,0,0,0,1},
        {1,1,1,0,1,1,1,0,0,1,1,1,1,0,1},
        {0,1,1,0,1,1,1,0,0,1,1,1,1,0,1},
        {0,1,1,0,1,1,1,0,0,0,0,0,0,0,1},
        {0,1,1,0,1,1,1,0,0,0,0,0,0,0,1},
        {0,1,1,0,0,0,0,0,0,0,1,1,1,1,1},
        {0,1,0,0,0,0,0,0,0,0,1,0,0,0,0},
        {0,1,1,1,1,1,1,0,1,1,1,0,0,0,0},
        {0,0,0,0,0,0,1,1,1,0,0,0,0,0,0}
    };
    
    [Header("Generated Colliders")]
    [SerializeField] private List<GameObject> generatedColliders = new List<GameObject>();
    
    /// <summary>
    /// コライダーを生成（単純配置版）
    /// </summary>
    public void GenerateCollidersSimple()
    {
        ClearColliders();
        
        for (int y = 0; y < testMapData.GetLength(0); y++)
        {
            for (int x = 0; x < testMapData.GetLength(1); x++)
            {
                if (testMapData[y, x] == 1) // 1の箇所にコライダーを配置
                {
                    CreateCollider(x, y, 1, 1, $"Collider_{x}_{y}");
                }
            }
        }
        
        Debug.Log($"Generated {generatedColliders.Count} colliders (Simple)");
    }
    
    /// <summary>
    /// コライダーを生成（矩形結合）
    /// </summary>
    public void GenerateCollidersOptimized()
    {
        ClearColliders();
        
        bool[,] processed = new bool[testMapData.GetLength(0), testMapData.GetLength(1)];
        
        for (int y = 0; y < testMapData.GetLength(0); y++)
        {
            for (int x = 0; x < testMapData.GetLength(1); x++)
            {
                if (testMapData[y, x] == 1 && !processed[y, x])
                {
                    // 最大矩形を探す
                    var rect = FindLargestRectangle(x, y, processed);
                    
                    if (rect.width > 0 && rect.height > 0)
                    {
                        // 見つかった矩形をprocessedでマーク
                        for (int py = rect.y; py < rect.y + rect.height; py++)
                        {
                            for (int px = rect.x; px < rect.x + rect.width; px++)
                            {
                                processed[py, px] = true;
                            }
                        }
                        
                        // コライダー生成
                        CreateCollider(rect.x, rect.y, rect.width, rect.height, 
                                     $"Collider_{rect.x}_{rect.y}_W{rect.width}H{rect.height}");
                    }
                }
            }
        }
        
        Debug.Log($"Generated {generatedColliders.Count} colliders (Optimized)");
    }
    
    /// <summary>
    /// 指定位置から最大の矩形を探す
    /// </summary>
    private ColliderRect FindLargestRectangle(int startX, int startY, bool[,] processed)
    {
        if (testMapData[startY, startX] != 1 || processed[startY, startX])
            return new ColliderRect(0, 0, 0, 0);
        
        int maxWidth = 0;
        int maxHeight = 0;
        
        // 水平方向の最大幅を探す
        for (int x = startX; x < testMapData.GetLength(1); x++)
        {
            if (testMapData[startY, x] == 1 && !processed[startY, x])
                maxWidth++;
            else
                break;
        }
        
        // 各幅で最大の高さを探す
        for (int width = maxWidth; width >= 1; width--)
        {
            int height = 1;
            
            // この幅で何行まで続くかチェック
            for (int y = startY + 1; y < testMapData.GetLength(0); y++)
            {
                bool canExtend = true;
                for (int x = startX; x < startX + width; x++)
                {
                    if (testMapData[y, x] != 1 || processed[y, x])
                    {
                        canExtend = false;
                        break;
                    }
                }
                
                if (canExtend)
                    height++;
                else
                    break;
            }
            
            // より良い矩形が見つかったら更新
            if (width * height > maxWidth * maxHeight)
            {
                maxWidth = width;
                maxHeight = height;
            }
        }
        
        return new ColliderRect(startX, startY, maxWidth, maxHeight);
    }
    
    /// <summary>
    /// 個別のコライダーを作成
    /// </summary>
    private void CreateCollider(int gridX, int gridY, int width, int height, string name)
    {
        GameObject colliderObj = new GameObject(name);
        colliderObj.transform.SetParent(this.transform);
        
        // 位置計算（左上原点からUnity座標系に変換）
        Vector2 worldPos = new Vector2(
            mapOffset.x + (gridX + width * 0.5f) * cellSize.x,
            mapOffset.y - (gridY + height * 0.5f) * cellSize.y // Yは反転
        );
        
        colliderObj.transform.position = worldPos;
        
        // BoxCollider2D設定
        BoxCollider2D collider = colliderObj.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(cellSize.x * width, cellSize.y * height);
        
        generatedColliders.Add(colliderObj);
    }
    
    /// <summary>
    /// 生成済みコライダーをすべて削除
    /// </summary>
    public void ClearColliders()
    {
        for (int i = generatedColliders.Count - 1; i >= 0; i--)
        {
            if (generatedColliders[i] != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(generatedColliders[i]);
                }
                else
                {
                    DestroyImmediate(generatedColliders[i]);
                }
            }
        }
        generatedColliders.Clear();
    }
    
    /// <summary>
    /// CSVデータを解析してマップデータに変換
    /// </summary>
    public void LoadFromCSV(string csvText)
    {
        string[] lines = csvText.Split('\n');
        int height = lines.Length;
        int width = lines[0].Split(',').Length;
        
        int[,] newMapData = new int[height, width];
        
        for (int y = 0; y < height; y++)
        {
            string[] values = lines[y].Split(',');
            for (int x = 0; x < width && x < values.Length; x++)
            {
                if (int.TryParse(values[x].Trim(), out int value))
                {
                    newMapData[y, x] = value;
                }
            }
        }
        
        testMapData = newMapData;
        mapWidth = width;
        mapHeight = height;
    }
    
    /// <summary>
    /// プレビュー表示用のGizmos
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!showPreview) return;
        
        Gizmos.color = Color.red;
        
        for (int y = 0; y < testMapData.GetLength(0); y++)
        {
            for (int x = 0; x < testMapData.GetLength(1); x++)
            {
                if (testMapData[y, x] == 1)
                {
                    Vector2 worldPos = new Vector2(
                        mapOffset.x + (x + 0.5f) * cellSize.x,
                        mapOffset.y - (y + 0.5f) * cellSize.y
                    );
                    
                    Gizmos.DrawWireCube(worldPos, cellSize);
                }
            }
        }
    }
}