using UniRx;
using UnityEngine;

namespace AltaTestWork
{
    public class SizeChanger : SceneEntityBehaviour
    {
        [field: SerializeField] public float MinimalSizePercent { get; private set; }
        private float _initialSize;

        public override void Ready()
        {
            _initialSize = Root.transform.localScale.magnitude;
        }

        public bool ChangeSize(float value, bool clamp = true)
        {
            if (clamp)
            {
                if (!SizeCanBeChanged()) return false;
            }

            Root.transform.localScale += Vector3.one * value;

            return true;
        }

        public float GetPercent()
        {
            var currentSize = Root.transform.localScale.magnitude;
            return currentSize / _initialSize * 100f;
        }
        
        public float GetPercentToMinimum()
        {
            return Mathf.InverseLerp(MinimalSizePercent, 100f, GetPercent()) * 100f;
        }

        public bool SizeCanBeChanged()
        {
            return GetPercent() > MinimalSizePercent;
        }
    }
}