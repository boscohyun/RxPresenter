using NUnit.Framework;

namespace Boscohyun.RxPresenter.Tests.EditMode
{
    using Fixtures;
    
    public class ViewAnimatorTest
    {
        public void Construct(
            [Values(0, 1)] int animatorActiveDelayFrame,
            [Values(false, true)] bool animatorAlwaysActive)
        {
            var viewAnimator = new ViewAnimator(animatorActiveDelayFrame, animatorAlwaysActive);
            Assert.AreEqual(animatorActiveDelayFrame, viewAnimator.AnimatorActiveDelayFrame);
            Assert.AreEqual(animatorAlwaysActive, viewAnimator.AnimatorAlwaysActive);
        }
        
        [Test, Combinatorial]
        public void SetActive(
            [Values(0, 1)] int animatorActiveDelayFrame,
            [Values(false, true)] bool animatorAlwaysActive,
            [Values(false, true)] bool active)
        {
            var viewAnimator = new ViewAnimator(animatorActiveDelayFrame, animatorAlwaysActive);
            viewAnimator.SetActive(active);
            Assert.AreEqual(active, viewAnimator.ActiveSelf);
        }

        [Test, Combinatorial]
        public void PlayAnimation(
            [Values(0, 1)] int animatorActiveDelayFrame,
            [Values(false, true)] bool animatorAlwaysActive,
            [Values(ViewAnimatorState.Hide, ViewAnimatorState.Show)]
            ViewAnimatorState viewAnimatorState,
            [Range(0f, 1f, 0.1f)] float normalizedTime)
        {
            var viewAnimator = new ViewAnimator(animatorActiveDelayFrame, animatorAlwaysActive);
            viewAnimator.PlayAnimation(viewAnimatorState, normalizedTime);
            Assert.AreEqual(viewAnimatorState, viewAnimator.CurrentAnimatorState);
            Assert.AreEqual(normalizedTime, viewAnimator.CurrentAnimatorStateNormalizedTime);
        }
    }
}
