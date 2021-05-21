using NUnit.Framework;

namespace Boscohyun.RxPresenter.Tests.EditMode
{
    using Fixtures;
    
    public class ViewAnimatorTest
    {
        [TestCase(false)]
        [TestCase(true)]
        public void Construct(bool animatorAlwaysActive)
        {
            var viewAnimator = new ViewAnimator(animatorAlwaysActive);
            Assert.AreEqual(animatorAlwaysActive, viewAnimator.AnimatorAlwaysActive);
        }
        
        [Test, Combinatorial]
        public void SetActive(
            [Values(false, true)] bool animatorAlwaysActive,
            [Values(false, true)] bool active)
        {
            var viewAnimator = new ViewAnimator(animatorAlwaysActive);
            viewAnimator.SetActive(active);
            Assert.AreEqual(active, viewAnimator.ActiveSelf);
        }

        [Test, Combinatorial]
        public void PlayAnimation(
            [Values(false, true)] bool animatorAlwaysActive,
            [Values(ViewAnimatorState.Hide, ViewAnimatorState.Show)]
            ViewAnimatorState viewAnimatorState,
            [Range(0f, 1f, 0.1f)] float normalizedTime)
        {
            var viewAnimator = new ViewAnimator(animatorAlwaysActive);
            viewAnimator.PlayAnimation(viewAnimatorState, normalizedTime);
            Assert.AreEqual(viewAnimatorState, viewAnimator.CurrentAnimatorState);
            Assert.AreEqual(normalizedTime, viewAnimator.CurrentAnimatorStateNormalizedTime);
        }
    }
}
