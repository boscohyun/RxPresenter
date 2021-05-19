using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Boscohyun.RxPresenter.Examples.Unity
{
    public class ExampleScene01Controller : MonoBehaviour
    {
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
                .ThrottleFirst(TimeSpan.FromMilliseconds(500d))
                .Subscribe(_ => ShowOrHidePresenter(leftCube))
                .AddTo(gameObject);
            rightButton.OnClickAsObservable()
                .ThrottleFirst(TimeSpan.FromMilliseconds(500d))
                .Subscribe(_ => ShowOrHidePresenter(rightCube))
                .AddTo(gameObject);
        }

        private static void UpdateByState(PresenterState state, Selectable selectable, Text text)
        {
            switch (state)
            {
                case PresenterState.ShowAnimation:
                    selectable.interactable = false;
                    text.text = "Showing Left Cube...";
                    break;
                case PresenterState.Shown:
                    selectable.interactable = true;
                    text.text = "Hide Left Cube";
                    break;
                case PresenterState.HideAnimation:
                    selectable.interactable = false;
                    text.text = "Hiding Left Cube...";
                    break;
                case PresenterState.Hidden:
                    selectable.interactable = true;
                    text.text = "Show Left Cube";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void ShowOrHidePresenter(IPresenter presenter)
        {
            if (presenter.PresenterState == PresenterState.Hidden)
            {
                presenter.Show();   
            }
            else if (presenter.PresenterState == PresenterState.Shown)
            {
                presenter.Hide();
            }
        }
    }
}
