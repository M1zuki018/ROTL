// ============================================================================
// AUTO GENERATED - DO NOT MODIFY
// Generated at: 2025-08-04 16:52:55
// ============================================================================

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CryStar.Story.Data;

/// <summary>
/// ストーリーシーン情報の定数クラス
/// </summary>
public static class MasterStoryScene
{
    private static readonly Dictionary<int, StorySceneData> _sceneData = new Dictionary<int, StorySceneData>
    {
        {
            1, new StorySceneData(1, "朝～キャスト用オフィス", 1, 1, 1, 
                "A3:O63", 1.0f, new Vector3(0.0f, 0.0f, 0.0f), null
            )
        },
        {
            2, new StorySceneData(2, "探索はじめ", 1, 1, 2, 
                "A66:O68", 1.0f, new Vector3(0.0f, 0.0f, 1.0f), null
            )
        },
        {
            3, new StorySceneData(3, "かがるエリア会話1", 1, 1, 3, 
                "A71:O80", 1.0f, new Vector3(0.0f, 0.0f, 2.0f), null
            )
        },
        {
            4, new StorySceneData(4, "かがるエリア会話２", 1, 1, 4, 
                "A71:O80", 1.0f, new Vector3(0.0f, 0.0f, 3.0f), null
            )
        },
        {
            5, new StorySceneData(5, "衣装スタッフ会話", 1, 1, 5, 
                "A83:O90", 1.0f, new Vector3(0.0f, 0.0f, 4.0f), null
            )
        },
        {
            6, new StorySceneData(6, "お昼休憩", 1, 1, 6, 
                "A98:O103", 1.0f, new Vector3(0.0f, 0.0f, 0.0f), null
            )
        },
        {
            7, new StorySceneData(7, "警察陣登場", 1, 1, 7, 
                "A109:O111", 1.0f, new Vector3(0.0f, 0.0f, 0.0f), null
            )
        },
    };

    /// <summary>
    /// IDからストーリーシーンデータを取得
    /// </summary>
    public static StorySceneData GetSceneById(int id)
    {
        return _sceneData.GetValueOrDefault(id, null);
    }

    /// <summary>
    /// シーン名からストーリーシーンデータを取得
    /// </summary>
    public static StorySceneData GetSceneByName(string sceneName)
    {
        return _sceneData.Values.FirstOrDefault(scene => scene.SceneName == sceneName);
    }

    /// <summary>
    /// チャプターIDとシーンIDからストーリーシーンデータを取得
    /// </summary>
    public static StorySceneData GetSceneById(int chapterId, int sceneId)
    {
        return _sceneData.Values.FirstOrDefault(scene => 
            scene.ChapterId == chapterId && scene.SceneId == sceneId);
    }

    /// <summary>
    /// 指定チャプターの全シーンを取得
    /// </summary>
    public static IEnumerable<StorySceneData> GetScenesByChapter(int chapterId)
    {
        return _sceneData.Values.Where(scene => scene.ChapterId == chapterId)
                                .OrderBy(scene => scene.SceneId);
    }

    /// <summary>
    /// 指定パートの全シーンを取得
    /// </summary>
    public static IEnumerable<StorySceneData> GetScenesByPart(int partId)
    {
        return _sceneData.Values.Where(scene => scene.PartId == partId)
                                .OrderBy(scene => scene.ChapterId)
                                .ThenBy(scene => scene.SceneId);
    }

    /// <summary>
    /// 前提ストーリーが指定されているシーンを取得
    /// </summary>
    public static IEnumerable<StorySceneData> GetScenesWithPrerequisite(int prerequisiteStoryId)
    {
        return _sceneData.Values.Where(scene => scene.PrerequisiteStoryId == prerequisiteStoryId);
    }

    /// <summary>
    /// 全ストーリーシーンのIDリストを取得
    /// </summary>
    public static IEnumerable<int> GetAllSceneIds()
    {
        return _sceneData.Keys;
    }

    /// <summary>
    /// 全ストーリーシーンデータを取得
    /// </summary>
    public static IEnumerable<StorySceneData> GetAllScenes()
    {
        return _sceneData.Values.OrderBy(scene => scene.Id);
    }

    /// <summary>
    /// シーンが実行可能かチェック（前提ストーリーの条件確認）
    /// </summary>
    public static bool CanExecuteScene(int sceneId, HashSet<int> completedStories)
    {
        var scene = GetSceneById(sceneId);
        if (scene == null) return false;

        // 前提ストーリーが指定されていない場合は実行可能
        if (!scene.PrerequisiteStoryId.HasValue) return true;

        // 前提ストーリーが完了している場合は実行可能
        return completedStories.Contains(scene.PrerequisiteStoryId.Value);
    }
}

