using System;
using UnityEngine;

namespace InDevelopment.Mechanics.ObjectDistraction
{
    /// <summary>
    /// will be used when it has to make noise
    /// </summary>
    [RequireComponent(typeof(Rigidbody)),RequireComponent(typeof(SoundMaker))]
    public class ThrowableObject : MonoBehaviour
    {
        public int noiseLoudness;
        private SoundMaker soundMaker;
        private void Start()
        {
            //this is just to make sure that any item that this script is
            //attached to worked without worrying about having to change the layer manually
            //Problem may arise if layer orders are changed
            gameObject.layer = 11;
            soundMaker = GetComponent<SoundMaker>();
        }

        private void OnCollisionEnter(Collision other)
        {
            soundMaker.MakeSound(gameObject.transform.position, noiseLoudness);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position,noiseLoudness);
        }
    }
}
