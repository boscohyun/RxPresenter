using UnityEngine;
using UnityEngine.UI;

namespace Boscohyun.RxPresenter.Examples
{
    public class TextButton : Button
    {
        [SerializeField] private Text text;

        protected override void Reset()
        {
            base.Reset();
            text = gameObject.GetComponentInChildren<Text>();
        }

        public TextButton SetText(string message)
        {
            text.text = message;
            return this;
        }
    }
}
