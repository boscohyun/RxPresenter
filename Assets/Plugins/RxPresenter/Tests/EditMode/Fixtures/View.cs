namespace Boscohyun.RxPresenter.Tests.EditMode.Fixtures
{
    public class View : IView
    {
        public bool ActiveSelf { get; private set; }
        public bool HasViewAnimator { get; }
        public IViewAnimator ViewAnimator { get; }
        
        public View(bool activeSelf)
        {
            ActiveSelf = activeSelf;
            HasViewAnimator = false;
            ViewAnimator = null;
        }
        
        public View(bool activeSelf, IViewAnimator viewAnimator)
        {
            ActiveSelf = activeSelf;
            HasViewAnimator = true;
            ViewAnimator = viewAnimator;
        }

        public void SetActive(bool active) => ActiveSelf = active;
    }
}
