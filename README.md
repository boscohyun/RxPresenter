# RxPresenter

[![openupm](https://img.shields.io/npm/v/com.boscohyun.rxpresenter?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.boscohyun.rxpresenter/)

**RxPresenter** implement [Reactive Presenter][reactive-presenter-link] of [MV(R)P][mvp-link] pattern that can be used simply and usefully in [Unity][unity-link].

[reactive-presenter-link]: https://github.com/boscohyun/RxPresenter/blob/main/Assets/Plugins/RxPresenter/Scripts/Runtime/ReactivePresenter.cs
[mvp-link]: https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93presenter
[unity-link]: https://www.unity.com

# Dependency

- [UniRx][uni-rx-link]
- [UniTask][uni-task-link]

[uni-rx-link]: https://github.com/neuecc/UniRx
[uni-task-link]: https://github.com/Cysharp/UniTask

# How to Install(OpenUPM)

#### #1 Install "GitDependencyResolverForUnity"

```
$ openupm add com.coffee.git-dependency-resolver
```

#### #2 Install "RxPresenter"

```
$ openupm add com.boscohyun.rxpresenter
```

## How to Install(manifest.json)

#### #1 Install "GitDependencyResolverForUnity"

```json
{
  "dependencies": {
    "com.coffee.git-dependency-resolver": "https://github.com/mob-sakai/GitDependencyResolverForUnity.git"
  }
}
```

#### #2 Install "RxPresenter"

```json
{
  "dependencies": {
    "com.boscohyun.rxpresenter": "https://github.com/boscohyun/RxPresenter.git?path=Assets/Plugins/RxPresenter"
  }
}
```

# How to Use

### Control Presenter or ReactivePresenter

```c#
public class PresenterController : MonoBehaviour
{
    [SerializedField] Presenter presenter;
    
    // NOTE: ReactivePresenter just implement IReactivePresenter<T> based on Presenter with Presenter.Humble
    [SerializedField] ReactivePresenter reactivePresenter;
    
    public void Awake()
    {
        presenter.Humble.OnPresenterStateChange.Subscribe().AddTo(gameObject);
        presenter.Humble.OnShowAnimationBeginning.Subscribe().AddTo(gameObject);
        presenter.Humble.OnShowAnimationEnd.Subscribe().AddTo(gameObject);
        presenter.Humble.OnHideAnimationBeginning.Subscribe().AddTo(gameObject);
        presenter.Humble.OnHideAnimationEnd.Subscribe().AddTo(gameObject);
        
        reactivePresenter.OnPresenterStateChange.Subscribe().AddTo(gameObject);
        reactivePresenter.OnShowAnimationBeginning.Subscribe().AddTo(gameObject);
        reactivePresenter.OnShowAnimationEnd.Subscribe().AddTo(gameObject);
        reactivePresenter.OnHideAnimationBeginning.Subscribe().AddTo(gameObject);
        reactivePresenter.OnHideAnimationEnd.Subscribe().AddTo(gameObject);
    }
    
    public async void ShowPresenters()
    {
        // Just show
        presenter.Show();
        reactivePresenter.Show();
        
        // Show with callback
        presenter.Show(p => { }); // p: presenter
        reactivePresenter.Show(rp => { }); // rp: reactivePresenter
        
        // Show immediately whithout any animations
        presenter.Humble.Show(true, p => { }); // or (true, null);
        reactivePresenter.Show(true, rp => { }); // or (true, null);
        
        // Show as observable
        presenter.Humble.ShowAsObservable() // or (true) if you want show immediately
            .First()
            .Subscribe(p => { });
        reactivePresenter.ShowAsObservable() // or (true)  if you want show immediately
            .First()
            .Subscribe(rp => { });
        
        // Await showing task
        await presenter.Humble.ShowAsync();
        await reactivePresenter.ShowAsync();
    }
    
    public void HidePresenters()
    {
        // Just hide
        presenter.Hide();
        reactivePresenter.Hide();
        
        // Hide with callback
        presenter.Humble.Hide(p => { }); // p: presenter
        reactivePresenter.Hide(rp => { }); // rp: reactivePresenter
        
        // Hide immediately whithout any animations
        presenter.Humble.Hide(true, p => { }); // or (true, null);
        reactivePresenter.Hide(true, rp => { }); // or (true, null);
        
        // Hide as observable
        presenter.Humble.HideAsObservable() // or (true) if you want show immediately
            .First()
            .Subscribe(p => { });
        reactivePresenter.HideAsObservable() // or (true)  if you want show immediately
            .First()
            .Subscribe(rp => { });
        
        // Await hiding task
        await presenter.Humble.HideAsync();
        await reactivePresenter.HideAsync();
    }
}
```

### Custom ViewAnimator and Presenter or ReactivePresenter

You can custom your own ViewAnimators and Presenters like below.
- Unity Animator: [AnimatorViewAnimator][animator-view-animator-link], [AnimatorReactivePresenter][animator-reactive-presenter-link]
- DOTween: [DOTweenViewAnimator][dotween-view-animator-link], [DOTweenPresenter][dotween-presenter-link], [DOTweenReactivePresenter][dotween-reactive-presenter-link]

[animator-view-animator-link]: https://github.com/boscohyun/RxPresenter/blob/main/Assets/Plugins/RxPresenter/Scripts/Runtime/AnimatorViewAnimator.cs
[animator-reactive-presenter-link]: https://github.com/boscohyun/RxPresenter/blob/main/Assets/Plugins/RxPresenter/Scripts/Runtime/AnimatorReactivePresenter.cs
[dotween-view-animator-link]: https://github.com/boscohyun/RxPresenter/blob/main/Assets/Plugins/RxPresenter/External/DOTween/Scripts/DOTweenViewAnimator.cs
[dotween-presenter-link]: https://github.com/boscohyun/RxPresenter/blob/main/Assets/Plugins/RxPresenter/External/DOTween/Scripts/DOTweenPresenter.cs
[dotween-reactive-presenter-link]: https://github.com/boscohyun/RxPresenter/blob/main/Assets/Plugins/RxPresenter/External/DOTween/Scripts/DOTweenReactivePresenter.cs

```c#
// Skeleton code example
[Serializable]
public class CustomViewAnimator : IViewAnimator
{
    public int AnimatorActiveDelayFrame { get; }
    
    public bool AnimatorAlwaysActive { get; }
    
    public ViewAnimatorState CurrentAnimatorState { get; }
    
    public float CurrentAnimatorStateNormalizedTime { get; }

    public void PlayAnimation(ViewAnimatorState viewAnimatorState, float normalizedTime) { }
    
    public void SetActive(bool active) { }
}

public class CustomPresenter : Presenter
// or public class CustomPresenter : ReactivePresenter<CustomPresenter>
// or public class CustomPresenter<T> : ReactivePresenter<T> where T : CustomPresenter<T>
{
    [SerializeField] private CustomViewAnimator viewAnimator;

    public override bool HasViewAnimator => /* Check viewAnimator */;

    public override IViewAnimator ViewAnimator => viewAnimator;
}
```

# External Supports

- [DOTween](https://github.com/Demigiant/dotween)
- (wip) [Animation-Sequencer](https://github.com/brunomikoski/Animation-Sequencer)
