using NUnit.Framework;

namespace Boscohyun.RxPresenter.Tests.EditMode
{
    using Fixtures;
    
    public class ViewTest
    {
        [TestCase(false)]
        [TestCase(true)]
        public void Construct(bool active)
        {
            var view = new View(active);
            Assert.AreEqual(active, view.ActiveSelf);
            Assert.False(view.HasViewAnimator);
            Assert.Null(view.ViewAnimator);
            
            var animatedView = new View(active, new ViewAnimator());
            Assert.AreEqual(active, animatedView.ActiveSelf);
            Assert.True(animatedView.HasViewAnimator);
            Assert.NotNull(animatedView.ViewAnimator);
        }

        [TestCase(false)]
        public void SetActive(bool active)
        {
            var view = new View(false);
            view.SetActive(active);
            Assert.AreEqual(active, view.ActiveSelf);
            view.SetActive(!active);
            Assert.AreEqual(!active, view.ActiveSelf);
        }
    }
}