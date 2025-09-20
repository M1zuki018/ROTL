using CryStar.Attribute;
using CryStar.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace iCON.Story.UI
{
    public class UIContents_Character : UIContentsCanvasGroupBase
    {
        // TODO: リファクタリング
        [Header("Custom Image")] 
        [SerializeField] private CustomImage _rearImage;
        [SerializeField] private CustomImage _bodyImage;
        [SerializeField] private CustomImage _faceImage;
        [SerializeField] private CustomImage _hairImage;
        
        [SerializeField, ExpandableSO] private MemeSettings _memeSettings;
        
        [Header("Pattern Settings")] 
        [SerializeField] private bool _useRandomPattern = false;
        [SerializeField] private bool[] _beatPattern = { true, false, true, false }; // どのパーツがどの拍で動くか
        
        private Vector3[] _originalPositions;
        private Sequence _beatSequence;
        private int _currentBeat = 0;
        private bool _isPlaying = false;
    
        private void Start()
        {
            // 元の位置を記録
            _originalPositions = new Vector3[4];
            _originalPositions[0] = _rearImage.rectTransform.anchoredPosition;
            _originalPositions[1] = _bodyImage.rectTransform.anchoredPosition;
            _originalPositions[2] = _faceImage.rectTransform.anchoredPosition;
            _originalPositions[3] = _hairImage.rectTransform.anchoredPosition;
            
            StartBeatAnimation();
        }
        
        private void OnDestroy()
        {
            // DOTweenのクリーンアップ
            _beatSequence?.Kill();
            
            var rectTransforms = new RectTransform[] 
            { 
                _rearImage?.rectTransform, 
                _bodyImage?.rectTransform, 
                _faceImage?.rectTransform, 
                _hairImage?.rectTransform 
            };
            
            foreach (var rectTransform in rectTransforms)
            {
                if (rectTransform != null)
                    rectTransform.DOKill();
            }
        }
        
        public async UniTask SetBaseImage(string rearPath, string bodyPath, string hairPath)
        {
            await UniTask.WhenAll(
                _hairImage.CanBeNullChangeSpriteAsync(hairPath),
                _rearImage.CanBeNullChangeSpriteAsync(rearPath),
                _bodyImage.CanBeNullChangeSpriteAsync(bodyPath)
            );
        }

        public async UniTask SetFaceImage(string facePath)
        {
            await _faceImage.ChangeSpriteAsync(facePath);
        }
    
        public void StartBeatAnimation()
        {
            if (_isPlaying) return;
            
            _isPlaying = true;
            _currentBeat = 0;
            CreateBeatSequence();
        }
        
        public void StopBeatAnimation()
        {
            if (!_isPlaying) return;
            
            _isPlaying = false;
            _beatSequence?.Kill();
            _beatSequence = null;
            
            // 全パーツを元の位置に戻す
            ResetAllParts();
        }
        
        private void CreateBeatSequence()
        {
            _beatSequence?.Kill();
            _beatSequence = DOTween.Sequence();
            
            // 無限ループのSequenceを作成
            _beatSequence.SetLoops(-1, LoopType.Restart);
            
            // 1拍分の処理を追加
            _beatSequence.AppendCallback(ProcessBeat);
            _beatSequence.AppendInterval(_memeSettings.BeatInterval);
        }
        
        private void ProcessBeat()
        {
            if (!_isPlaying) return;
            
            // 現在の拍でアニメーションするパーツを決定
            for (int i = 0; i < 4; i++)
            {
                bool shouldAnimate = _useRandomPattern ? 
                    Random.Range(0f, 1f) > 0.5f : 
                    _beatPattern[_currentBeat % _beatPattern.Length];
                
                if (shouldAnimate)
                {
                    AnimatePart(i);
                }
            }
            
            _currentBeat++;
        }
    
        private void AnimatePart(int partIndex)
        {
            RectTransform rectTransform;
            float bounceHeight;
            
            // パーツごとのRectTransformと高さを取得
            switch (partIndex)
            {
                case 0: // rear
                    rectTransform = _rearImage.rectTransform;
                    bounceHeight = _memeSettings.RearHeight;
                    break;
                case 1: // body
                    rectTransform = _bodyImage.rectTransform;
                    bounceHeight = _memeSettings.BodyHeight;
                    break;
                case 2: // face
                    rectTransform = _faceImage.rectTransform;
                    bounceHeight = _memeSettings.FaceHeight;
                    break;
                case 3: // hair
                    rectTransform = _hairImage.rectTransform;
                    bounceHeight = _memeSettings.HairHeight;
                    break;
                default:
                    return;
            }
            
            Vector3 targetPos = _originalPositions[partIndex] + Vector3.up * bounceHeight;
            
            // DOTweenシーケンスでバウンス効果を作成
            Sequence bounceSequence = DOTween.Sequence();
            
            bounceSequence.Append(
                rectTransform.DOAnchorPos(targetPos, _memeSettings.AnimationDuration * 0.1f)
                    .SetEase(Ease.OutQuad)
            );
            
            bounceSequence.Append(
                rectTransform.DOAnchorPos(_originalPositions[partIndex], _memeSettings.AnimationDuration * 0.9f)
                    .SetEase(_memeSettings.EaseType)
            );
        }
    
        private void ResetAllParts()
        {
            var rectTransforms = new RectTransform[] 
            { 
                _rearImage.rectTransform, 
                _bodyImage.rectTransform, 
                _faceImage.rectTransform, 
                _hairImage.rectTransform 
            };
            
            for (int i = 0; i < rectTransforms.Length; i++)
            {
                if (rectTransforms[i] != null)
                {
                    rectTransforms[i].DOKill(); // 進行中のアニメーションを停止
                    rectTransforms[i].DOAnchorPos(_originalPositions[i], 0.3f).SetEase(Ease.OutQuad);
                    rectTransforms[i].DOScale(Vector3.one, 0.3f).SetEase(Ease.OutQuad);
                }
            }
        }
    }
}