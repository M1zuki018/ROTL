// ============================================================================
// AUTO GENERATED - DO NOT MODIFY
// Generated at: 2025-09-02 11:41:54
// ============================================================================

using System.Collections.Generic;

/// <summary>
/// ワーディングキー定数クラス
/// </summary>
public static class WordingMaster
{
    private static readonly Dictionary<string, string> _data = new Dictionary<string, string>
    {
        { "BOOT_START", "Start" },
        { "LOADING", "Loading..." },
        { "STORY_IMMERSED", "UI\n非表示" },
        { "STORY_AUTO", "オート\n再生" },
        { "STORY_SKIP", "スキップ" },
        { "BATTLE_COMMAND_ATTCK", "アタック" },
        { "BATTLE_COMMAND_IDEA", "idea" },
        { "BATTLE_COMMAND_ITEM", "item" },
        { "BATTLE_COMMAND_GUARD", "まもる" },
        { "BATTLE_FIRSTSELECT_START", "たたかう" },
        { "BATTLE_FIRSTSELECT_ESCAPE", "にげる" },
        { "BATTLE_IDEA_COMMAND", "command" },
        { "BATTLE_IDEA_ACTOR", "actor" },
        { "BATTLE_FAILED_ESCAPE", "▼逃げ場などない" },
        { "TITLE_NEWGAME", "最初から" },
        { "TITLE_LOAD", "つづきから" },
        { "TITLE_CONFIG", "設定" },
        { "TITLE_COPYRIGHT", "©2025 CryStar Studio All rights reserved." },
        { "TITLE_VERSION", "ver. 0.0.0" },
        { "TITLE_PLAYSTYLE_STORY", "ストーリー体験モード" },
        { "TITLE_PLAYSTYLE_BATTLE", "戦闘単体モード" },
        { "TITLE_PLAYSTYLE_DESCRIPTION", "所要時間{0}分！\n{1}人向け！" },
        { "QUIT_CONFIRM", "ゲームを終了しますか？" },
        { "YES", "はい" },
        { "NO", "いいえ" },
        { "TITLE_PLAYSTYLE", "プレイモード選択" },
        { "STORY_TIME_REQUIRED", "5" },
        { "BATTLE_TIME_REQUIRED", "3" },
        { "FOR_THOSE_STORY", "ストーリーを知りたい" },
        { "FOR_THOSE_BATTLE", "サクっとバトルを体験したい" },
        { "PARAM_LEVEL", "レベル" },
        { "PARAM_HP", "体力" },
        { "PARAM_WILL", "意思力" },
        { "PARAM_STAMINA", "スタミナ" },
        { "PARAM_SP", "アイデアポイント" },
        { "PARAM_PHYSICAL_ATTACK", "物理攻撃力" },
        { "PARAM_SKILL_ATTACK", "アイデア攻撃力" },
        { "PARAM_INTELLIGENCE", "知力" },
        { "PARAM_PHYSICAL_DEFENSE", "物理防御力" },
        { "PARAM_SKILL_DEFENSE", "アイデア防御力" },
        { "PARAM_SPEED", "攻撃速度" },
        { "PARAM_DODGE_SPEED", "回避速度" },
        { "PARAM_ARMOR_PENETRATION", "防御無視" },
        { "PARAM_CRITICAL_LATE", "クリティカル率" },
        { "PARAM_CRITICAL_DAMAGE", "クリティカルダメージ" },
        { "STATUS", "ステータス" },
        { "EQUIOMENT", "装備" },
    };

    public static string GetText(string key)
    {
        return _data.GetValueOrDefault(key, null);
    }
}
