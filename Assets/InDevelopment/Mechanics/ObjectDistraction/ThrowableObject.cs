using System;
using InDevelopment.Mechanics.VisionAbility;
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
        private int startingNoiseLoudness;
        private ObjectSoundMaker objectSoundMaker;
        
        private void Start()
        {
            //this is just to make sure that any item that this script is
            //attached to worked without worrying about having to change the layer manually
            //Problem may arise if layer orders are changed
            gameObject.layer = LayerMask.NameToLayer("ThrowableObjects");
            objectSoundMaker = GetComponent<ObjectSoundMaker>();
            startingNoiseLoudness = noiseLoudness;
        }

        private void OnCollisionEnter(Collision other)
        {
            //TODO:add in a transform check so that it has to be more than a certain distance for it to work,
            //so that enemies that push the objects around don't re-trigger the sound again for themselves or other enemies nearby
            //TODO:add in speed check so that when level starts they don't automatically trigger enemies
            
            if (other.collider.CompareTag("Enemy")) return;
            
            //TODO:Add in some check for how this function is called
            
            
            noiseLoudness += noiseLoudness * (Mathf.RoundToInt(GetComponent<Rigidbody>().velocity.magnitude) / 3);
            if (noiseLoudness < 6)
            {
              Debug.Log("thing happened");
              return;
            }
            //TODO:ensure that the sound is made at the x and z positions not the y
            objectSoundMaker.MakeSound(gameObject.transform.position, noiseLoudness);
        }

        private void OnCollisionExit(Collision other)
        {
            noiseLoudness = startingNoiseLoudness;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position,noiseLoudness);
        }
    }
}
