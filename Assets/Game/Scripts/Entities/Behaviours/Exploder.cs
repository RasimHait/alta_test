using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AltaTestWork
{
    public class Exploder : SceneEntityBehaviour
    {
        [SerializeField] private ParticleSystem _explosionEffect;
        [SerializeField] private GameObject[] _hideAfterExplosion = Array.Empty<GameObject>();
        [SerializeField] private Component[] _disableAfterExplosion = Array.Empty<Component>();
        [SerializeField] private SceneEntityTag _affectionTag;
        [SerializeField] private float _delay;
        [SerializeField] private float _explisionRadius;
        private float _radiusMultiplier = 1;
        private bool _isUsed;

        public override void Ready()
        {
        }

        public void EngrowRadius(float value)
        {
            _radiusMultiplier += value;
        }

        public void Explode()
        {
            if (_isUsed) return;

            Root.StartCoroutine(Explosion());

            _isUsed = true;
        }

        private void ExplodeInRadius()
        {
            var hitColliders = new Collider[100];
            var size = Physics.OverlapSphereNonAlloc(Root.transform.position, _explisionRadius * _radiusMultiplier, hitColliders);

            for (var i = 0; i < size; i++)
            {
                var target = hitColliders[i];

                if (target.TryGetComponent(out SceneEntity entity) && entity.TryGetBehaviour(out Exploder exploder) && entity.Tag.HasFlag(_affectionTag))
                {
                    exploder.Explode();
                }
            }
        }

        private IEnumerator Explosion()
        {
            yield return new WaitForSeconds(_delay);

            _explosionEffect.Play();

            foreach (var obj in _hideAfterExplosion)
            {
                obj.SetActive(false);
            }
            foreach (var component in _disableAfterExplosion)
            {
                Object.Destroy(component);
            }

            ExplodeInRadius();
        }
    }
}