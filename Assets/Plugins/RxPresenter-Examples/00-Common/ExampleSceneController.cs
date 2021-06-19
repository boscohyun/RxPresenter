using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Boscohyun.RxPresenter.Examples
{
    public class ExampleSceneController : MonoBehaviour
    {
        [SerializeField] private bool skipAnimation;
        [SerializeField] private List<Presenter> leftCubes;
        [SerializeField] private List<Presenter> rightCubes;
        [SerializeField] private TextButton leftTextButton;
        [SerializeField] private TextButton rightTextButton;

        private void Awake()
        {
            InitializeSubscriptions(leftCubes, leftTextButton);
            InitializeSubscriptions(rightCubes, rightTextButton);
        }

        private void InitializeSubscriptions(IReadOnlyList<Presenter> presenters, TextButton textButton)
        {
            for (var i = 0; i < presenters.Count; i++)
            {
                presenters[i].Humble.OnPresenterStateChange
                    .Subscribe(humble => UpdateByState(humble.PresenterState, textButton))
                    .AddTo(gameObject);
            }
            
            textButton.OnClickAsObservable()
                .Subscribe(_ => ShowOrHidePresenters(presenters))
                .AddTo(gameObject);
        }

        private static void UpdateByState(PresenterState state, TextButton textButton)
        {
            switch (state)
            {
                case PresenterState.ShowAnimation:
                    textButton.SetText($"Showing {textButton.name.Replace("Button", string.Empty)} Cubes...");
                    break;
                case PresenterState.Shown:
                    textButton.SetText($"Hide {textButton.name.Replace("Button", string.Empty)} Cubes");
                    break;
                case PresenterState.HideAnimation:
                    textButton.SetText($"Hiding {textButton.name.Replace("Button", string.Empty)} Cubes...");
                    break;
                case PresenterState.Hidden:
                    textButton.SetText($"Show {textButton.name.Replace("Button", string.Empty)} Cubes");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ShowOrHidePresenters(IReadOnlyList<IPresenter> presenters)
        {
            for (var i = 0; i < presenters.Count; i++)
            {
                ShowOrHidePresenter(presenters[i]);
            }
        }

        private void ShowOrHidePresenter(IPresenter presenter)
        {
            switch (presenter.PresenterState)
            {
                case PresenterState.Hidden:
                    presenter.Show(skipAnimation);
                    break;
                case PresenterState.Shown:
                    presenter.Hide(skipAnimation);
                    break;
            }
        }
    }
}
