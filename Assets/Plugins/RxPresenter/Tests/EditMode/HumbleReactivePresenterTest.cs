using NUnit.Framework;

namespace Boscohyun.RxPresenter.Tests.EditMode
{
    using Fixtures;
    
    public class HumbleReactivePresenterTest
    {
        private HumbleReactivePresenter _humbleReactivePresenter;
        private HumbleReactivePresenter _animatedHumbleReactivePresenter;

        [SetUp]
        public void SetUp()
        {
            _humbleReactivePresenter = new HumbleReactivePresenter(new View(false));
            _animatedHumbleReactivePresenter = new HumbleReactivePresenter(new View(false, new ViewAnimator()));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Show_PresenterStateShown(bool skipAnimation)
        {
            _humbleReactivePresenter.Show(skipAnimation);
            _animatedHumbleReactivePresenter.Show(skipAnimation);
            if (skipAnimation)
            {
                Assert.AreEqual(PresenterState.Shown, _humbleReactivePresenter.PresenterState);
                Assert.AreEqual(PresenterState.Shown, _animatedHumbleReactivePresenter.PresenterState);
            }
            else
            {
                Assert.True(_humbleReactivePresenter.PresenterState == PresenterState.ShowAnimation ||
                            _humbleReactivePresenter.PresenterState == PresenterState.Shown);
                Assert.True(_animatedHumbleReactivePresenter.PresenterState == PresenterState.ShowAnimation ||
                            _animatedHumbleReactivePresenter.PresenterState == PresenterState.Shown);
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Show_Callback_PresenterStateShown(bool skipAnimation)
        {
            _humbleReactivePresenter.Show(skipAnimation, presenter =>
            {
                Assert.AreEqual(PresenterState.Shown, presenter.PresenterState);
            });
            _animatedHumbleReactivePresenter.Show(skipAnimation, presenter =>
            {
                Assert.AreEqual(PresenterState.Shown, presenter.PresenterState);
            });
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Hide_PresenterStateHidden(bool skipAnimation)
        {
            _humbleReactivePresenter.Hide(skipAnimation);
            _animatedHumbleReactivePresenter.Hide(skipAnimation);
            if (skipAnimation)
            {
                Assert.AreEqual(PresenterState.Hidden, _humbleReactivePresenter.PresenterState);
                Assert.AreEqual(PresenterState.Hidden, _animatedHumbleReactivePresenter.PresenterState);
            }
            else
            {
                Assert.True(_humbleReactivePresenter.PresenterState == PresenterState.HideAnimation ||
                            _humbleReactivePresenter.PresenterState == PresenterState.Hidden);
                Assert.True(_animatedHumbleReactivePresenter.PresenterState == PresenterState.HideAnimation ||
                            _animatedHumbleReactivePresenter.PresenterState == PresenterState.Hidden);
            }
        }
        
        [TestCase(false)]
        [TestCase(true)]
        public void Hide_Callback_PresenterStateShown(bool skipAnimation)
        {
            _humbleReactivePresenter.Hide(skipAnimation, presenter =>
            {
                Assert.AreEqual(PresenterState.Hidden, presenter.PresenterState);
            });
            _animatedHumbleReactivePresenter.Hide(skipAnimation, presenter =>
            {
                Assert.AreEqual(PresenterState.Hidden, presenter.PresenterState);
            });
        }
    }
}
