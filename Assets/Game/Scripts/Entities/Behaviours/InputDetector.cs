using UniRx;
using UnityEngine;

namespace AltaTestWork
{
    public class InputDetector : SceneEntityBehaviour, IUpdateObserver
    {
        public readonly ReactiveCommand OnClick = new ReactiveCommand();
        public readonly ReactiveCommand OnHold = new ReactiveCommand();
        public readonly ReactiveCommand OnRelease = new ReactiveCommand();

        public override void Ready()
        {
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                OnClick?.Execute();
            }
            
            if (Input.GetKey(KeyCode.Mouse0))
            {
                OnHold?.Execute();
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                OnRelease?.Execute();
            }
        }
    }
}