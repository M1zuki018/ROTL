using DG.Tweening;
using UnityEngine;

/// <summary>
/// Meme動画のような動きの設定を行うためのスクリプタブルオブジェクト
/// </summary>
[CreateAssetMenu(fileName = "Meme Settings", menuName = "Meme Settings")]
public class MemeSettings : ScriptableObject
{
    // TODO: リファクタリング
    [Header("Animation Settings")] 
    [SerializeField] private float _beatInterval = 0.5f; // 拍の間隔（秒）
    [SerializeField] private float _rearHeight = 1f; // Y軸の移動量
    [SerializeField] private float _bodyHeight = 1f; // Y軸の移動量
    [SerializeField] private float _faceHeight = 1f; // Y軸の移動量
    [SerializeField] private float _hairHeight = 1f; // Y軸の移動量
    [SerializeField] private float _animationDuration = 0.5f; // アニメーション時間
    [SerializeField] private Ease _easeType = Ease.OutBounce;
    
    public float BeatInterval => _beatInterval;
    public float RearHeight => _rearHeight;
    public float BodyHeight => _bodyHeight;
    public float FaceHeight => _faceHeight;
    public float HairHeight => _hairHeight;
    public float AnimationDuration => _animationDuration;
    public Ease EaseType => _easeType;
}
