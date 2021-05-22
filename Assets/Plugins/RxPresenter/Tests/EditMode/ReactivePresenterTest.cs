using NUnit.Framework;

namespace Boscohyun.RxPresenter.Tests.EditMode
{
    using Fixtures;
    
    public class ReactivePresenterTest
    {
        private ReactivePresenter _reactivePresenter;
        private ReactivePresenter _animatedReactivePresenter;

        [SetUp]
        public void SetUp()
        {
            _reactivePresenter = new ReactivePresenter(new View(false));
            _animatedReactivePresenter = new ReactivePresenter(new View(false, new ViewAnimator()));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Show_PresenterStateShown(bool skipAnimation)
        {
            _reactivePresenter.Show(skipAnimation);
            _animatedReactivePresenter.Show(skipAnimation);
            if (skipAnimation)
            {
                Assert.AreEqual(PresenterState.Shown, _reactivePresenter.PresenterState);
                Assert.AreEqual(PresenterState.Shown, _animatedReactivePresenter.PresenterState);
            }
            else
            {
                Assert.True(_reactivePresenter.PresenterState == PresenterState.ShowAnimation ||
                            _reactivePresenter.PresenterState == PresenterState.Shown);
                Assert.True(_animatedReactivePresenter.PresenterState == PresenterState.ShowAnimation ||
                            _animatedReactivePresenter.PresenterState == PresenterState.Shown);
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Show_Callback_PresenterStateShown(bool skipAnimation)
        {
            _reactivePresenter.Show(skipAnimation, presenter =>
            {
                Assert.AreEqual(PresenterState.Shown, presenter.PresenterState);
            });
            _animatedReactivePresenter.Show(skipAnimation, presenter =>
            {
                Assert.AreEqual(PresenterState.Shown, presenter.PresenterState);
            });
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Hide_PresenterStateHidden(bool skipAnimation)
        {
            _reactivePresenter.Hide(skipAnimation);
            _animatedReactivePresenter.Hide(skipAnimation);
            if (skipAnimation)
            {
                Assert.AreEqual(PresenterState.Hidden, _reactivePresenter.PresenterState);
                Assert.AreEqual(PresenterState.Hidden, _animatedReactivePresenter.PresenterState);
            }
            else
            {
                Assert.True(_reactivePresenter.PresenterState == PresenterState.HideAnimation ||
                            _reactivePresenter.PresenterState == PresenterState.Hidden);
                Assert.True(_animatedReactivePresenter.PresenterState == PresenterState.HideAnimation ||
                            _animatedReactivePresenter.PresenterState == PresenterState.Hidden);
            }
        }
        
        [TestCase(false)]
        [TestCase(true)]
        public void Hide_Callback_PresenterStateShown(bool skipAnimation)
        {
            _reactivePresenter.Hide(skipAnimation, presenter =>
            {
                Assert.AreEqual(PresenterState.Hidden, presenter.PresenterState);
            });
            _animatedReactivePresenter.Hide(skipAnimation, presenter =>
            {
                Assert.AreEqual(PresenterState.Hidden, presenter.PresenterState);
            });
        }
    }
}
