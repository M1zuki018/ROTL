namespace CryStar.GameEvent.Enums
{
    /// <summary>
    /// ゲームイベントの列挙型
    /// </summary>
    public enum GameEventType
    {
        GameClear = 0, // ゲームクリア
        Objective = 1, // 目標表示
        StoryPreload = 2, // ストーリーデータの事前ロード
        PlayStory = 3, // ストーリー再生
        ChangeMap = 4, // マップ変更
        BattleStart = 5, // バトル
        EventTransition = 6, // 別のイベントを実行
        
        Custom = 99,
    }
}