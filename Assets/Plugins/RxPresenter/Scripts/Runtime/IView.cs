namespace Boscohyun.RxPresenter
{
    public interface IView
    {
        bool ActiveSelf { get; }
        
        bool HasViewAnimator { get; }
        
        IViewAnimator ViewAnimator { get; }
        
        void SetActive(bool active);
    }
}
