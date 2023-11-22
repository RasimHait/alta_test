using UniRx;
using UnityEngine;

namespace AltaTestWork
{
    public class ContactDetector : SceneEntityBehaviour, ITriggerObserver
    {
        public readonly ReactiveCommand<SceneEntity> OnContact = new ReactiveCommand<SceneEntity>();

        public override void Ready()
        {
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out SceneEntity entity))
            {
                OnContact?.Execute(entity);
            }
        }
    }
}