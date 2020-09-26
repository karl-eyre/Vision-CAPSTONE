using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Mechanics.ObjectDistraction
{
    public class ObjectSoundMaker : MonoBehaviour
    {
        //possible other way, have a collider on everything and just change the size of that
        //depending on a noise level variable that is changed
        //depending on what the player is currently doing
        //when the collider is entered then something heard the sound get those collider's that are off type enemy
        //and call their investigate sound function and pass in the player's collider's position.
        //just use game control events to read when something is happening and set noise accordingly
        
        public LayerMask enemyLayer;
        public LayerMask obstacleLayer;
        
        public void MakeSound(Vector3 soundLocation, float loudnessOfSound)
        {
            Collider[] enemiesInRange = Physics.OverlapSphere(soundLocation, loudnessOfSound, enemyLayer);
            if (enemiesInRange.Length <= 0) return;
            
            //the sphere cast and if their still in then tell them to investigate
            //use raycast to determine who should've heard the sound
            
            foreach (var enemy in enemiesInRange)
            {
                bool actuallyHeardSound = !Physics.Linecast(soundLocation, enemy.transform.position, obstacleLayer);
                
                //var actuallyHeardSound = Physics.RaycastAll(soundLocation, enemy.transform.position, loudnessOfSound);
                
                if (actuallyHeardSound)
                {
                    EnemyStateMachine enemyScript = enemy.GetComponent<EnemyStateMachine>();
                    //call notify enemy of sound and pass in sound location for position
                    if (!enemyScript.lineOfSight.isDetecting)
                    {
                        enemyScript.targetLastKnownPos = soundLocation;
                        enemyScript.ChangeState(EnemyStateMachine.States.investigating);
                    }
                }
            }
        }
    }
}
