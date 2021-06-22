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

        protected virtual HumbleReactivePresenter CreateHumbleObject() => new HumbleReactivePresenter(this);

        protected virtual void ShowAtAwake() => Show();

        public void Show(bool skipAnimation = default) => Humble.Show(skipAnimation);

        public void Hide(bool skipAnimation = default) => Humble.Hide(skipAnimation);

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
