namespace Boscohyun.RxPresenter
{
    public interface IView
    {
        bool ActiveSelf { get; }
        
        void SetActive(bool active);
        
        bool HasViewAnimator { get; }
        
        IViewAnimator ViewAnimator { get; }
    }
}
