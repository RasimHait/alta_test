using System;
using System.Collections.Generic;
using UnityEngine;

namespace AltaTestWork
{
    [Serializable]
    public abstract class SceneEntityBehaviour : IDisposable
    {
        [field: SerializeField] public string BehaviourName { get; private set; }
        protected SceneEntity Root { get; private set; }
        public List<IDisposable> Container { get; private set; }

        public virtual void Initialize(SceneEntity root)
        {
            Root = root;
            Container = new List<IDisposable>();
        }

        public abstract void Ready();

        public void Dispose()
        {
            foreach (var disposable in Container)
            {
                disposable?.Dispose();
            }

            Root.DisposeBehaviour(this);
        }
    }
}