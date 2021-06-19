#if RXPRESENTER_DOTWEEN_SUPPORT
using Boscohyun.RxPresenter.External.DOTween;
using DG.Tweening;
using UnityEngine;

namespace Boscohyun.RxPresenter.Examples
{
    public class ColorDOTweenPresenter : DOTweenPresenter
    {
        [SerializeField] private Color _showEndValue;
        [SerializeField] private float _showDuration;
        [SerializeField] private Color _hideEndValue;
        [SerializeField] private float _hideDuration;

        private Material _material;

        protected override void Awake()
        {
            _material = GetComponent<Renderer>().material;
            base.Awake();
        }
        
        protected override Tween GetShowTween() => _material
            .DOColor(_showEndValue, _showDuration)
            .SetAutoKill(false)
            .Pause();

        protected override Tween GetHideTween() => _material
            .DOColor(_hideEndValue, _hideDuration)
            .SetAutoKill(false)
            .Pause();
    }
}
#endif
