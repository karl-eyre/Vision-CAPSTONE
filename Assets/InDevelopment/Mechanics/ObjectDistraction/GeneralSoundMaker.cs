using System.Collections;
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
            if (!other.gameObject.GetComponent<EnemyStateMachine>()) return;
            
            var enemyScript = other.gameObject.GetComponent<EnemyStateMachine>();
            enemyScript.targetLastKnownPos = transform.position;
            //TODO: Change to Event Driven system.
            //enemyScript.ChangeState(EnemyModel.States.investigating);
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