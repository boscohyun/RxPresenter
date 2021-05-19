namespace Boscohyun.RxPresenter
{
    public interface IViewAnimator
    {
        bool AnimatorAlwaysActive { get; }
        
        ViewAnimationName CurrentAnimationName { get; }
        
        float CurrentAnimationNormalizedTime { get; }

        void PlayAnimation(ViewAnimationName viewAnimationName, float normalizedTime);
        
        void SetActive(bool active);
    }
}
