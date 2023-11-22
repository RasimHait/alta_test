using UnityEngine;

namespace AltaTestWork
{
    public interface IUpdateObserver
    {
        void Update(){}
        void FixedUpdate(){}
        void LateUpdate(){}
    }
    
    public interface ITriggerObserver
    {
        void OnTriggerEnter(Collider other){}
        void OnTriggerExit(Collider other){}
        void OnTriggerStay(Collider other){}
    }
    
    public interface ICollisionObserver
    {
        void OnCollisionEnter(Collision other){}
        void OnCollisionExit(Collision other){}
        void OnCollisionStay(Collision other){}
    }
}