using UnityEngine;

namespace AltaTestWork
{
    public class MoveForward : SceneEntityBehaviour, IUpdateObserver
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _accelerationDuration;
        [SerializeField] private AnimationCurve _accelerationCurve;

        private float _currentAccelerationTime;
        private float _currentAccelerationFactor;
        private bool _isMoving;

        public override void Ready()
        {
        }

        public void MoveStart()
        {
            _isMoving = true;
        }

        public void MoveStop()
        {
            _isMoving = false;
        }

        public void BreakAcceleration()
        {
            _currentAccelerationTime = 0f;
        }

        public void Update()
        {
            if (_isMoving)
            {
                HandleAcceleration(true);
            }
            else if (_currentAccelerationTime > 0)
            {
                HandleAcceleration(false);
            }
            else
            {
                return;
            }
            
            HandleMovement();
        }

        private void HandleMovement()
        {
            var transform = Root.transform;
            var currentPosition = transform.position;
            var targetPosition = currentPosition + transform.forward;
            var targetStep = _speed * _currentAccelerationFactor;

            Root.transform.position = Vector3.MoveTowards(currentPosition, targetPosition, targetStep * Time.deltaTime);
        }

        private void HandleAcceleration(bool positive)
        {
            var step = positive ? _currentAccelerationTime + Time.deltaTime : _currentAccelerationTime - Time.deltaTime;
            _currentAccelerationTime = Mathf.Clamp(step, 0, _accelerationDuration);

            ApplyAccelerationChange();
        }

        private void ApplyAccelerationChange()
        {
            _currentAccelerationFactor = _accelerationCurve.Evaluate(_currentAccelerationTime / _accelerationDuration);
        }
    }
}