using UnityEngine;

namespace Boscohyun.RxPresenter
{
    public class AnimatorPresenter<T> : Presenter<T>, IView, IViewAnimator
        where T : AnimatorPresenter<T>
    {
        private static readonly int AnimatorHashShow = Animator.StringToHash("Show");
        private static readonly int AnimatorHashHide = Animator.StringToHash("Hide");

        [SerializeField] private Animator animator;
        [SerializeField] private bool animatorAlwaysEnabled;

        #region IView

        bool IView.ActiveSelf => gameObject.activeSelf;

        bool IView.HasViewAnimator => animator;

        IViewAnimator IView.ViewAnimator => this;

        void IView.SetActive(bool active) => gameObject.SetActive(active);

        #endregion
        
        #region IViewAnimator

        int IViewAnimator.AnimatorActiveDelayFrame => 1;
        
        bool IViewAnimator.AnimatorAlwaysActive => animatorAlwaysEnabled;

        ViewAnimatorState IViewAnimator.CurrentAnimatorState =>
            animator.GetCurrentAnimatorStateInfo(default).shortNameHash == AnimatorHashHide
                ? ViewAnimatorState.Hide
                : ViewAnimatorState.Show;

        float IViewAnimator.CurrentAnimatorStateNormalizedTime =>
            animator.GetCurrentAnimatorStateInfo(default).normalizedTime;
        
        void IViewAnimator.PlayAnimation(ViewAnimatorState viewAnimatorState, float normalizedTime) =>
            animator.Play(
                viewAnimatorState == ViewAnimatorState.Hide
                    ? AnimatorHashHide
                    : AnimatorHashShow,
                default,
                normalizedTime);

        void IViewAnimator.SetActive(bool active) => animator.enabled = active;

        #endregion

        protected override IView View => this;
    }
}
