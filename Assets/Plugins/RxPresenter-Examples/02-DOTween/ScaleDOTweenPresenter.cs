#if RXPRESENTER_DOTWEEN_SUPPORT
using Boscohyun.RxPresenter.External.DOTween;
using DG.Tweening;
using UnityEngine;

namespace Boscohyun.RxPresenter.Examples
{
    public class ScaleDOTweenPresenter : DOTweenPresenter
    {
        [SerializeField, Range(0f, 1f)] private float _showEndValue = 1f;
        [SerializeField] private float _showDuration;
        [SerializeField, Range(0f, 1f)] private float _hideEndValue;
        [SerializeField] private float _hideDuration;

        protected override Tween GetShowTween() => transform
            .DOScale(_showEndValue, _showDuration)
            .SetEase(Ease.OutElastic)
            .SetAutoKill(false)
            .Pause();

        protected override Tween GetHideTween() => transform
            .DOScale(_hideEndValue, _hideDuration)
            .SetEase(Ease.OutExpo)
            .SetAutoKill(false)
            .Pause();
    }
}
#endif
