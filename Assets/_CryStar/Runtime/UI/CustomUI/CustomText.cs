using System;
using System.Linq;
using CryStar.Utility;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// CustomText
/// </summary>
[AddComponentMenu("Custom UI/Custom Text")]
public class CustomText : Text
{
    [SerializeField] private string _wordingKey;

    protected override void Awake()
    {
        base.Awake();

        if (!string.IsNullOrEmpty(_wordingKey))
        {
            SetWordingText(_wordingKey);
        }
    }

    /// <summary>
    /// テキストを設定する
    /// </summary>
    public void SetText(string text)
    {
        base.text = text;
    }

    /// <summary>
    /// 文言キーを使ってテキストを設定する
    /// </summary>
    public void SetWordingText(string wordingKey)
    {
        string wordingText = WordingMaster.GetText(wordingKey);
        if (wordingText != null)
        {
            m_Text = wordingText;
        }
    }

    /// <summary>
    /// 文言キーを使ってテキストを設定する（フォーマット対応）
    /// </summary>
    public void SetWordingText(string wordingKey, params object[] args)
    {
        string wordingText = WordingMaster.GetText(wordingKey);
        if (wordingText != null)
        {
            try
            {
                m_Text = string.Format(wordingText, args);
            }
            catch (FormatException ex)
            {
                LogUtility.Error($"フォーマットが変換出来ませんでした '{wordingKey}': {ex.Message}");
                
                // フォーマット失敗時は元のテキストを使用する
                m_Text = wordingText; 
            }
        }
    }

    /// <summary>
    /// 文言キーを使ってテキストを設定する（フォーマットの変数もWordingKeyに対応）
    /// </summary>
    public void SetWordingTextFormat(string wordingKey, params object[] args)
    {
        string wordingText = WordingMaster.GetText(wordingKey);
        string[] wordingArgs = args.Select(arg => WordingMaster.GetText(arg as string)).ToArray();
        
        if (wordingText != null)
        {
            try
            {
                m_Text = string.Format(wordingText, wordingArgs);
            }
            catch (FormatException ex)
            {
                LogUtility.Error($"フォーマットが変換出来ませんでした '{wordingKey}': {ex.Message}");
                
                // フォーマット失敗時は元のテキストを使用する
                m_Text = wordingText; 
            }
        }
    }
}
