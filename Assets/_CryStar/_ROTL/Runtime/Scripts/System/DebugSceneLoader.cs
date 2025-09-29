using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// デバッグ用簡易シーンローダー
/// </summary>
public class DebugSceneLoader : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
