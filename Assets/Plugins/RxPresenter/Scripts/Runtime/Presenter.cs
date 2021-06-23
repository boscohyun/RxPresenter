using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Boscohyun.RxPresenter
{
    [DisallowMultipleComponent]
    public class Presenter : MonoBehaviour, IPresenter, IView
    {
        [SerializeField] protected bool showAtAwake;

        public HumbleReactivePresenter Humble { get; private set; }

        public PresenterState PresenterState => Humble.PresenterState;

        #region IView

        public bool ActiveSelf => gameObject.activeSelf;

        public void SetActive(bool active) => gameObject.SetActive(active);

        public virtual bool HasViewAnimator => default;

        public virtual IViewAnimator ViewAnimator => default;

        #endregion

        protected virtual void Awake()
        {
            Humble = CreateHumbleObject();
            if (showAtAwake)
            {
                ShowAtAwake();
            }
        }

        protected virtual HumbleReactivePresenter CreateHumbleObject() => new HumbleReactivePresenter(
            this,
            ShowAnimationBeginning,
            ShowAnimationAsync,
            ShowAnimationEnd,
            ShowAnimationBeginning,
            ShowAnimationAsync,
            ShowAnimationEnd
        );

        protected virtual void ShowAtAwake() => Show();

        public void Show(bool skipAnimation = default) => Humble.Show(skipAnimation);

        protected virtual void ShowAnimationBeginning(bool skip = default)
        {
            SetActive(true);

            if (!HasViewAnimator)
            {
                return;
            }

            if (!ViewAnimator.AnimatorAlwaysActive)
            {
                ViewAnimator.SetActive(true);
            }

            ViewAnimator.PlayAnimation(ViewAnimatorState.Show, skip ? 1f : 0f);
        }

        protected virtual async UniTask ShowAnimationAsync()
        {
            if (!HasViewAnimator)
            {
                return;
            }

            await UniTask.WaitWhile(() => ViewAnimator.CurrentAnimatorState != ViewAnimatorState.Show);
            await UniTask.WaitWhile(() => ViewAnimator.CurrentAnimatorStateNormalizedTime < 1f);
        }

        protected virtual void ShowAnimationEnd()
        {
            if (HasViewAnimator && !ViewAnimator.AnimatorAlwaysActive)
            {
                ViewAnimator.SetActive(false);
            }
        }

        public void Hide(bool skipAnimation = default) => Humble.Hide(skipAnimation);

        protected virtual void HideAnimationBeginning(bool skip = default)
        {
            if (!HasViewAnimator)
            {
                return;
            }

            if (!ViewAnimator.AnimatorAlwaysActive)
            {
                ViewAnimator.SetActive(true);
            }

            ViewAnimator.PlayAnimation(ViewAnimatorState.Hide, skip ? 1f : 0f);
        }

        protected virtual async UniTask HideAnimationAsync()
        {
            if (!HasViewAnimator)
            {
                return;
            }

            await UniTask.WaitWhile(() => ViewAnimator.CurrentAnimatorState != ViewAnimatorState.Hide);
            await UniTask.WaitWhile(() => ViewAnimator.CurrentAnimatorStateNormalizedTime < 1f);
        }

        protected virtual void HideAnimationEnd()
        {
            SetActive(false);

            if (HasViewAnimator && !ViewAnimator.AnimatorAlwaysActive)
            {
                ViewAnimator.SetActive(false);
            }
        }

        protected virtual void OnDestroy()
        {
            Dispose();
        }

        public virtual void Dispose()
        {
            Humble.Dispose();
        }
    }
}
