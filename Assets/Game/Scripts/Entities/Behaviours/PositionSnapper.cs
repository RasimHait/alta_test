using UnityEngine;

namespace AltaTestWork
{
    public class PositionSnapper : SceneEntityBehaviour, IUpdateObserver
    {
        [SerializeField] private Transform _currentTarget;
        [SerializeField] private Vector3 _currentOffset;
        
        public override void Ready()
        {
        }

        public void Snap(Transform target, Vector3 offset)
        {
            _currentTarget = target;
            _currentOffset = offset;
        }

        public void Unsnap()
        {
            _currentTarget = null;
            _currentOffset = Vector3.zero;
        }

        public void Update()
        {
            if(!_currentTarget) return;

            Root.transform.position = _currentTarget.position + _currentOffset;
        }
    }
}