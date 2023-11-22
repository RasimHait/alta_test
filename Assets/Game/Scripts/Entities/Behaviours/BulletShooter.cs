using UnityEngine;

namespace AltaTestWork
{
    public class BulletShooter : SceneEntityBehaviour
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Vector3 _spawnOffset;
        private Bullet _activeBullet;

        public override void Ready()
        {
        }

        public void SpawnBullet()
        {
            _activeBullet = Object.Instantiate(_bulletPrefab);
            _activeBullet.Snap(Root.transform, _spawnOffset);
        }

        public void Grow(float factor)
        {
            if (!HasBullet()) return;

            _activeBullet.ChangeSize(factor);
        }

        public void Release()
        {
            if (!HasBullet()) return;

            _activeBullet.Activate();
            _activeBullet = null;
        }

        public bool HasBullet()
        {
            return _activeBullet != null;
        }
    }
}