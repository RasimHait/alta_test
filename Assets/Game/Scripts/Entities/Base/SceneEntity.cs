using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AltaTestWork
{
    public abstract class SceneEntity : MonoBehaviour
    {
        [field: SerializeField] public SceneEntityTag Tag { get; private set; }
        [SerializeReference, SerializeField] private List<SceneEntityBehaviour> _behaviours = new();
        private List<IUpdateObserver> _updateObservers = new();
        private List<ITriggerObserver> _triggerObservers = new();
        private List<ICollisionObserver> _collisionObservers = new();
        private readonly List<(object obj, IList list)> _disposeList = new();

        private void Awake()
        {
            _behaviours.ForEach(x => x.Initialize(this));
            _updateObservers = _behaviours.OfType<IUpdateObserver>().ToList();
            _triggerObservers = _behaviours.OfType<ITriggerObserver>().ToList();
            _collisionObservers = _behaviours.OfType<ICollisionObserver>().ToList();

            _behaviours.ForEach(x => x?.Ready());
            OnReady();
        }

        private void Update()
        {
            Clear();
            _updateObservers.ForEach(x => x?.Update());
        }

        private void FixedUpdate()
        {
            Clear();
            _updateObservers.ForEach(x => x?.FixedUpdate());
        }

        private void LateUpdate()
        {
            Clear();
            _updateObservers.ForEach(x => x?.LateUpdate());
        }

        private void OnTriggerEnter(Collider other)
        {
            Clear();
            _triggerObservers.ForEach(x => x?.OnTriggerEnter(other));
        }

        private void OnTriggerExit(Collider other)
        {
            Clear();
            _triggerObservers.ForEach(x => x?.OnTriggerExit(other));
        }

        private void OnTriggerStay(Collider other)
        {
            Clear();
            _triggerObservers.ForEach(x => x?.OnTriggerStay(other));
        }

        private void OnCollisionEnter(Collision collision)
        {
            Clear();
            _collisionObservers.ForEach(x => x?.OnCollisionEnter(collision));
        }

        private void OnCollisionExit(Collision collision)
        {
            Clear();
            _collisionObservers.ForEach(x => x?.OnCollisionExit(collision));
        }

        private void OnCollisionStay(Collision collision)
        {
            Clear();
            _collisionObservers.ForEach(x => x?.OnCollisionStay(collision));
        }

        private void Clear()
        {
            foreach (var disposed in _disposeList)
            {
                disposed.list.Remove(disposed.obj);
            }

            _disposeList.Clear();
        }

        public T GetBehaviour<T>() where T : SceneEntityBehaviour
        {
            var result = _behaviours.FirstOrDefault(x => x.GetType() == typeof(T)) as T;
            return result;
        }

        public T GetBehaviour<T>(string behaviourName) where T : SceneEntityBehaviour
        {
            var result = _behaviours.FirstOrDefault(x => x.GetType() == typeof(T) && x.BehaviourName == behaviourName) as T;
            return result;
        }

        public bool TryGetBehaviour<T>(out T result) where T : SceneEntityBehaviour
        {
            result = GetBehaviour<T>();
            return result != default;
        }

        public bool TryGetBehaviour<T>(string behaviourName, out T result) where T : SceneEntityBehaviour
        {
            result = GetBehaviour<T>(behaviourName);
            return result != default;
        }

        public void DisposeBehaviour(SceneEntityBehaviour behaviour)
        {
            if (!_behaviours.Contains(behaviour) || _disposeList.Any(x => x.obj == behaviour)) return;

            if (behaviour is IUpdateObserver updateObserver)
            {
                _disposeList.Add((updateObserver, _updateObservers));
            }

            if (behaviour is ITriggerObserver triggerObserver)
            {
                _disposeList.Add((triggerObserver, _triggerObservers));
            }

            if (behaviour is ICollisionObserver collisionObserver)
            {
                _disposeList.Add((collisionObserver, _collisionObservers));
            }

            _disposeList.Add((behaviour, _behaviours));
            behaviour.Dispose();
        }

        protected abstract void OnReady();

        private void OnDestroy()
        {
            var count = _behaviours.Count;
            for (var i = 0; i < count; i++)
            {
                DisposeBehaviour(_behaviours[i]);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            ValidateRequiredBehaviours();
        }

        private void Reset()
        {
            ValidateRequiredBehaviours();
        }

        private void ValidateRequiredBehaviours()
        {
            if (Application.isPlaying) return;

            var attribute = Attribute.GetCustomAttribute(GetType(), typeof(RequireBehaviour));

            if (attribute == null) return;

            foreach (var type in ((RequireBehaviour)attribute).Types)
            {
                if (_behaviours.Any(x => x.GetType() == type)) continue;
                if (Activator.CreateInstance(type) is SceneEntityBehaviour newBehaviour)
                {
                    _behaviours.Add(newBehaviour);
                }
            }
        }
#endif
    }
}