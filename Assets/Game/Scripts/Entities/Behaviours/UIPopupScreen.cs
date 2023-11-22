using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AltaTestWork
{
    public class UIPopupScreen : SceneEntityBehaviour
    {
        public ReactiveCommand OnActionButton = new ReactiveCommand();
        [SerializeField] private Transform _screenRoot;
        [SerializeField] private Button _actionButton;

        public override void Ready()
        {
            _actionButton.onClick.AddListener(() => OnActionButton?.Execute());
        }

        public void Show()
        {
            _screenRoot.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _screenRoot.gameObject.SetActive(false);
        }
    }
}