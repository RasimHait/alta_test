using UniRx;
using UnityEngine;

namespace AltaTestWork
{
    [RequireBehaviour(typeof(MoveForward), typeof(ContactDetector), typeof(Exploder), typeof(SizeChanger), typeof(PositionSnapper))]
    public class Bullet : SceneEntity
    {
        private SceneEntityTag _availableContactWith;
        private MoveForward _movement;
        private ContactDetector _contactDetector;
        private SizeChanger _sizeChanger;
        private Exploder _exploder;
        private PositionSnapper _snapper;

        protected override void OnReady()
        {
            _movement = GetBehaviour<MoveForward>();
            _contactDetector = GetBehaviour<ContactDetector>();
            _exploder = GetBehaviour<Exploder>();
            _sizeChanger = GetBehaviour<SizeChanger>();
            _snapper = GetBehaviour<PositionSnapper>();

            _contactDetector.OnContact
                .Subscribe(OnContact)
                .AddTo(_contactDetector.Container);

            transform.localScale = Vector3.zero;
        }

        private void OnContact(SceneEntity entity)
        {
            if (!entity.Tag.HasFlag(_availableContactWith)) return;

            _exploder.Explode();
            _movement.MoveStop();
        }

        public void ChangeSize(float value)
        {
            _sizeChanger.ChangeSize(value, false);
            _exploder.EngrowRadius(value);
        }

        public void Snap(Transform target, Vector3 offset)
        {
            _snapper.Snap(target, offset);
        }

        public void Activate()
        {
            _movement.MoveStart();
            _snapper.Unsnap();
        }
    }
}