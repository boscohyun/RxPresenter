using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Boscohyun.RxPresenter.Examples
{
    public class ExampleScene01Controller : MonoBehaviour
    {
        [SerializeField] private bool skipAnimation;
        [SerializeField] private VanillaPresenter leftCube;
        [SerializeField] private VanillaPresenter rightCube;
        [SerializeField] private Button leftButton;
        [SerializeField] private Text leftButtonText;
        [SerializeField] private Button rightButton;
        [SerializeField] private Text rightButtonText;

        private void Awake()
        {
            leftCube.OnPresenterStateChange
                .Subscribe(presenter => UpdateByState(presenter.PresenterState, leftButton, leftButtonText))
                .AddTo(gameObject);
            rightCube.OnPresenterStateChange
                .Subscribe(presenter => UpdateByState(presenter.PresenterState, rightButton, rightButtonText))
                .AddTo(gameObject);
            
            leftButton.OnClickAsObservable()
                .Subscribe(_ => ShowOrHidePresenter(leftCube))
                .AddTo(gameObject);
            rightButton.OnClickAsObservable()
                .Subscribe(_ => ShowOrHidePresenter(rightCube))
                .AddTo(gameObject);
        }

        private static void UpdateByState(PresenterState state, Selectable selectable, Text text)
        {
            switch (state)
            {
                case PresenterState.ShowAnimation:
                    text.text = "Showing Left Cube...";
                    break;
                case PresenterState.Shown:
                    text.text = "Hide Left Cube";
                    break;
                case PresenterState.HideAnimation:
                    text.text = "Hiding Left Cube...";
                    break;
                case PresenterState.Hidden:
                    text.text = "Show Left Cube";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ShowOrHidePresenter(IPresenter presenter)
        {
            if (presenter.PresenterState == PresenterState.Hidden)
            {
                presenter.Show(skipAnimation);   
            }
            else if (presenter.PresenterState == PresenterState.Shown)
            {
                presenter.Hide(skipAnimation);
            }
        }
    }
}
