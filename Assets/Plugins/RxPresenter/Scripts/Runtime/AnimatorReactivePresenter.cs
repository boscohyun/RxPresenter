using UnityEngine;

namespace Boscohyun.RxPresenter
{
    public class AnimatorReactivePresenter<T> : ReactivePresenter<T> where T : AnimatorReactivePresenter<T>
    {
        [SerializeField] private AnimatorViewAnimator viewAnimator;

        public override bool HasViewAnimator => viewAnimator.HasAnimator;

        public override IViewAnimator ViewAnimator => viewAnimator;
    }
}
