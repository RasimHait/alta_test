using System;
using UniRx;
using UnityEngine;

namespace AltaTestWork
{
    public class Finish: MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private void Awake()
        {
            MessageBroker.Default.Receive<Messages.Player.PlayerIsNearFinish>()
                .Subscribe(x => OpenGate())
                .AddTo(this);
        }

        private void OpenGate()
        {
            _animator.enabled = true;
        }
    }
}