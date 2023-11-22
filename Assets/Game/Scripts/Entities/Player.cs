using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace AltaTestWork
{
    [RequireBehaviour(typeof(InputDetector), typeof(MoveForward), typeof(ContactDetector), typeof(BulletShooter), typeof(SizeChanger), typeof(TargetDistanceCalculator))]
    public class Player : SceneEntity
    {
        [SerializeField, PropertyOrder(-100)]
        private SceneEntityTag _availableContactWith;
        [SerializeField, PropertyOrder(-100)]
        private float _completeDistance;
        [SerializeField, PropertyOrder(-100)]
        private float _openGateDistance;
        private MoveForward _movement;
        private InputDetector _input;
        private BulletShooter _bulletShooter;
        private ContactDetector _contactDetector;
        private TargetDistanceCalculator _targetDistanceCalculator;
        private SizeChanger _sizeChanger;
        private IDisposable _playerIsNearFinishObserver;

        protected override void OnReady()
        {
            Init();
            Subscribe();
            _movement.MoveStart();
        }

        private void Init()
        {
            _movement = GetBehaviour<MoveForward>();
            _input = GetBehaviour<InputDetector>();
            _bulletShooter = GetBehaviour<BulletShooter>();
            _sizeChanger = GetBehaviour<SizeChanger>();
            _contactDetector = GetBehaviour<ContactDetector>();
            _targetDistanceCalculator = GetBehaviour<TargetDistanceCalculator>();
        }

        private void Subscribe()
        {
            _input.OnClick
                .Subscribe(x => PrepareBullet())
                .AddTo(_bulletShooter.Container);

            _input.OnHold
                .Subscribe(x => TransferMassToBullet())
                .AddTo(_bulletShooter.Container);

            _input.OnRelease
                .Subscribe(x => Shoot())
                .AddTo(_bulletShooter.Container);
            
            _contactDetector.OnContact
                .Subscribe(OnContact)
                .AddTo(_contactDetector.Container);

            _targetDistanceCalculator.CurrentDistance
                .Where(x => x <= _completeDistance)
                .Subscribe(x => NoticePlayerHasWon())
                .AddTo(_targetDistanceCalculator.Container);
            
            _playerIsNearFinishObserver = _targetDistanceCalculator.CurrentDistance
                .Where(x => x <= _openGateDistance)
                .Subscribe(x => NoticePlayerIsNearFinish())
                .AddTo(_targetDistanceCalculator.Container);
        }
        
        private void OnContact(SceneEntity entity)
        {
            if (!entity.Tag.HasFlag(_availableContactWith)) return;

            NoticePlayerHasFailed();
        }

        private void PrepareBullet()
        {
            if (!_sizeChanger.SizeCanBeChanged()) return;
            _bulletShooter.SpawnBullet();
        }

        private void TransferMassToBullet()
        {
            var factor = 1f * Time.deltaTime;

            if (_sizeChanger.ChangeSize(-factor))
            {
                _bulletShooter.Grow(factor);
                NoticePlayerHasChangedSize();
            }
            else if (_bulletShooter.HasBullet())
            {
                NoticePlayerHasFailed();
                Shoot();
            }
        }

        private void NoticePlayerHasFailed()
        {
            _input.Dispose();
            _movement.MoveStop();
            _targetDistanceCalculator.Dispose();
            MessageBroker.Default.Publish(new Messages.Player.PlayerFail());
        }
        
        private void NoticePlayerIsNearFinish()
        {
            _playerIsNearFinishObserver.Dispose();
            MessageBroker.Default.Publish(new Messages.Player.PlayerIsNearFinish());
        }
        
        private void NoticePlayerHasWon()
        {
            _input.Dispose();
            _movement.MoveStop();
            _targetDistanceCalculator.Dispose();
            MessageBroker.Default.Publish(new Messages.Player.PlayerHasEnteredFinish());
        }

        private void NoticePlayerHasChangedSize()
        {
            MessageBroker.Default.Publish(new Messages.Player.PlayerSizePercentChanged(_sizeChanger.MinimalSizePercent, _sizeChanger.GetPercentToMinimum()));
        }

        private void Shoot()
        {
            _bulletShooter.Release();
        }
    }
}