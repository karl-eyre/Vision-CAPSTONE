using System.Collections;
using InDevelopment.Alex;
using UnityEngine;
using InDevelopment.Mechanics.Enemy;

namespace InDevelopment.Mechanics.ObjectDistraction
{
    public class GeneralSoundMaker : MonoBehaviour
    {
        public Collider noiseLevelCollider;

        public LayerMask enemyLayer;

        private float noiseLevelDebug;
        private bool waiting;

        //simply call this function and pass in a noise level and it changes the colliders size to be the same as the noise level
        //can be used for things like sprinting or walking
        public void MakeSound(float noiseLevel)
        {
            noiseLevelDebug = noiseLevel;
            noiseLevelCollider.transform.localScale = new Vector3(noiseLevel, noiseLevel, noiseLevel);

            if (waiting) return;
            waiting = true;
            StartCoroutine(WaitTime());
        }

        public void OnCollisionEnter(Collision other)
        {
            //maybe raycast to any enemy in range to see if they heard it, if hit they did else it hit a wall and they didn't 
            if (!other.gameObject.GetComponent<EnemyStateBase>()) return;
            //TODO:uncomment below and test
            var enemyScript = other.gameObject.GetComponent<EnemyStateBase>();
            // enemyScript.GetDistracted(transform.position);
            
        }

        private IEnumerator WaitTime()
        {
            yield return new WaitForSeconds(0.4f);
            MakeSound(0.1f);
            waiting = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(noiseLevelCollider.transform.position, noiseLevelDebug);
        }
    }
}