using UnityEngine;

namespace Boscohyun.RxPresenter
{
    public class AnimatorPresenter : Presenter
    {
        [SerializeField] private AnimatorViewAnimator viewAnimator;

        public override bool HasViewAnimator => viewAnimator.HasAnimator;

        public override IViewAnimator ViewAnimator => viewAnimator;
    }
}
