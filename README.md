# RxPresenter

**RxPresenter** implement [Reactive Presenter][reactive-presenter-link] of [MV(R)P][mvp-link] pattern that can be used simply and usefully in [Unity][unity-link].
**RxPresenter** uses [UniRx][uni-rx-link] and [UniTask][uni-task-link].

[reactive-presenter-link]: https://github.com/boscohyun/RxPresenter/blob/main/Assets/Plugins/RxPresenter/Scripts/Runtime/ReactivePresenter.cs
[mvp-link]: https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93presenter
[unity-link]: https://www.unity.com
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

# External Supports

- [DOTween](https://github.com/Demigiant/dotween)
- (wip) [Animation-Sequencer](https://github.com/brunomikoski/Animation-Sequencer)
- (wip) [UTween](https://github.com/ls9512/UTween)
