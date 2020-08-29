using System;
using Enemy;
using UnityEngine;

namespace InDevelopment.Mechanics.ObjectDistraction
{
    /// <summary>
    /// will be used when it has to make noise
    /// </summary>
    [RequireComponent(typeof(Rigidbody)),RequireComponent(typeof(ObjectSoundMaker)),RequireComponent(typeof(VisionEffectActivation))]
    public class ThrowableObject : MonoBehaviour
    {
        public int noiseLoudness;
        private ObjectSoundMaker _objectSoundMaker;
        private void Start()
        {
            //this is just to make sure that any item that this script is
            //attached to worked without worrying about having to change the layer manually
            //Problem may arise if layer orders are changed
            gameObject.layer = LayerMask.NameToLayer("ThrowableObjects");
            _objectSoundMaker = GetComponent<ObjectSoundMaker>();
        }

        private void OnCollisionEnter(Collision other)
        {
            //Add in some check for how this function is called
            //maybe depending on the objects rb's relative velocity?
            _objectSoundMaker.MakeSound(gameObject.transform.position, noiseLoudness);
        }

        //TODO: Remove after testing
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position,noiseLoudness);
        }
    }
}
