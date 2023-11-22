using UniRx;
using UnityEngine;

namespace AltaTestWork
{
    public class TargetDistanceCalculator: SceneEntityBehaviour, IUpdateObserver
    {
        [SerializeField] private Transform _target;
        public ReactiveProperty<float> CurrentDistance { get; private set; } = new ReactiveProperty<float>();

        public override void Ready()
        {
            CalculateDistance();
        }

        private void CalculateDistance()
        {
            if(!_target) return;
            var distance = (Root.transform.position - _target.position).magnitude;
            CurrentDistance.Value = distance;
        }

        public void Update()
        {
            CalculateDistance();
        }
    }
}