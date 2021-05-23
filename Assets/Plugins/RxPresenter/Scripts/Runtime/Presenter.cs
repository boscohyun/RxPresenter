using System;
using UniRx;
using UnityEngine;

namespace Boscohyun.RxPresenter
{
    [DisallowMultipleComponent]
    public abstract class Presenter<T> : MonoBehaviour, IReactivePresenter<T>
        where T : Presenter<T>
    {
        [SerializeField] private bool showAtAwake;
        
        private ReactivePresenter _reactivePresenter;
        
        protected abstract IView View { get; }
        
        public PresenterState PresenterState => _reactivePresenter.PresenterState;
        
        public IObservable<T> OnPresenterStateChange => _reactivePresenter.OnPresenterStateChange.Select(_ => (T) this);
        public IObservable<T> OnShowAnimationBeginning => _reactivePresenter.OnPresenterStateChange.Select(_ => (T) this);
        public IObservable<T> OnShowAnimationEnd => _reactivePresenter.OnPresenterStateChange.Select(_ => (T) this);
        public IObservable<T> OnHideAnimationBeginning => _reactivePresenter.OnPresenterStateChange.Select(_ => (T) this);
        public IObservable<T> OnHideAnimationEnd => _reactivePresenter.OnPresenterStateChange.Select(_ => (T) this);

        protected virtual void Awake()
        {
            _reactivePresenter = new ReactivePresenter(View);
            if (showAtAwake)
            {
                Show();
            }
        }

        protected virtual void OnDestroy()
        {
            ((IDisposable) this).Dispose();
        }

        void IDisposable.Dispose()
        {
            _reactivePresenter.Dispose();
        }

        public void Show(bool skipAnimation = default) => _reactivePresenter.Show(skipAnimation);

        public void Show(Action<T> callback) => _reactivePresenter.Show(_ => callback?.Invoke((T) this));

        public void Show(bool skipAnimation, Action<T> callback) =>
            _reactivePresenter.Show(skipAnimation, _ => callback?.Invoke((T) this));

        public IObservable<T> ShowAsObservable(bool skipAnimation = default) =>
            _reactivePresenter.ShowAsObservable(skipAnimation).Select(_ => (T) this);

        public void Hide(bool skipAnimation = default) => _reactivePresenter.Hide(skipAnimation);

        public void Hide(Action<T> callback) => _reactivePresenter.Hide(_ => callback?.Invoke((T) this));

        public void Hide(bool skipAnimation, Action<T> callback) =>
            _reactivePresenter.Hide(skipAnimation, _ => callback?.Invoke((T) this));

        public IObservable<T> HideAsObservable(bool skipAnimation = default) =>
            _reactivePresenter.HideAsObservable(skipAnimation).Select(_ => (T) this);
    }
}
