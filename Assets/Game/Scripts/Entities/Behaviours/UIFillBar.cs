using UnityEngine;
using UnityEngine.UI;

namespace AltaTestWork
{
    public class UIFillBar : SceneEntityBehaviour
    {
        [SerializeField] private Image _fillBar;
        [Range(0f, 1f)] [SerializeField] private float _initialValue;

        public override void Ready()
        {
            ChangeValue(_initialValue);
        }

        public void ChangeValue(float value)
        {
            value = Mathf.Clamp(value, 0f, 1f);
            _fillBar.fillAmount = value;
        }
    }
}