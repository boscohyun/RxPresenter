using Boscohyun.RxPresenter.External.DOTween;
using DG.Tweening;
using UnityEngine;

namespace Boscohyun.RxPresenter.Examples
{
    public class SequenceDOTweenPresenter : DOTweenPresenter
    {
        [SerializeField, Range(0f, 1f)] private float _showEndScale = 1f;
        [SerializeField] private float _showScaleDuration;
        [SerializeField] private Color _showEndColor;
        [SerializeField] private float _showColorDuration;
        [SerializeField, Range(0f, 1f)] private float _hideEndScale;
        [SerializeField] private float _hideScaleDuration;
        [SerializeField] private Color _hideEndColor;
        [SerializeField] private float _hideColorDuration;
        
        private Material _material;

        protected override void Awake()
        {
            _material = GetComponent<Renderer>().material;
            base.Awake();
        }

        protected override Tween GetShowTween() => DOTween.Sequence()
                .Append(transform
                    .DOScale(_showEndScale, _showScaleDuration)
                    .SetEase(Ease.OutElastic))
                .Append(_material.DOColor(_showEndColor, _showColorDuration))
                .SetAutoKill(false)
                .Pause();

        protected override Tween GetHideTween() => DOTween.Sequence()
            .Append(_material.DOColor(_hideEndColor, _hideColorDuration))
            .Append(transform
                .DOScale(_hideEndScale, _hideScaleDuration)
                .SetEase(Ease.OutElastic))
            .SetAutoKill(false)
            .Pause();
    }
}
