using System;
using UnityEngine;

namespace Boscohyun.RxPresenter
{
    [Serializable]
    public class AnimatorViewAnimator : IViewAnimator
    {
        private static readonly int AnimatorHashShow = Animator.StringToHash("Show");
        private static readonly int AnimatorHashHide = Animator.StringToHash("Hide");

        [SerializeField] private Animator animator;
        [SerializeField] private bool animatorAlwaysEnabled;

        public bool HasAnimator => animator;

        public AnimatorViewAnimator(Animator animator, bool animatorAlwaysEnabled)
        {
            this.animator = animator;
            this.animatorAlwaysEnabled = animatorAlwaysEnabled;
        }
        
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
    }
}
