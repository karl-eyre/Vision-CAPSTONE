using System;
using System.Collections;
using InDevelopment.Mechanics.VisionAbility;
using UnityEngine;

namespace InDevelopment.Mechanics.ObjectDistraction
{
    /// <summary>
    /// will be used when it has to make noise
    /// </summary>
    [RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(ObjectSoundMaker)),
     RequireComponent(typeof(VisionEffectActivation))]
    public class ThrowableObject : MonoBehaviour
    {
        public int noiseLoudness;
        private int defaultNoiseLoudness = 10;
        private ObjectSoundMaker objectSoundMaker;
        private Rigidbody rb;
        public float respawnDelay = 5f;

        [SerializeField]
        private float duration;

        public float reduceRate = 0.1f;
        public bool thrown;

        private Vector3 startingPos;

        private void Start()
        {
            //this is just to make sure that any item that this script is
            //attached to worked without worrying about having to change the layer manually
            //Problem may arise if layer orders are changed
            gameObject.layer = LayerMask.NameToLayer("ThrowableObjects");
            objectSoundMaker = GetComponent<ObjectSoundMaker>();
            rb = GetComponent<Rigidbody>();
            startingPos = transform.position;
            StartCoroutine(ResetLoudness());
        }


        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Enemy")) return;
            if (other.collider.CompareTag("Player"))
            {
                thrown = true;
            }

            objectSoundMaker.MakeSound(gameObject.transform.position, noiseLoudness);
        }

        private void Update()
        {
            if (duration <= 0)
            {
                ResetPos();
            }

            if (thrown)
            {
                duration -= reduceRate;
            }
            else
            {
                duration = respawnDelay;
            }
        }

        private void ResetPos()
        {
            transform.position = startingPos;
            transform.rotation = Quaternion.identity;
            rb.velocity = Vector3.zero;
            thrown = false;
        }

        private IEnumerator ResetLoudness()
        {
            noiseLoudness = 0;
            yield return new WaitForSeconds(2f);
            noiseLoudness = defaultNoiseLoudness;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, noiseLoudness);
        }
    }
}