using System;
using InDevelopment.Mechanics.Enemy;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InDevelopment.Mechanics.ObjectDistraction
{
    public class SoundMaker : MonoBehaviour
    {
        //possible other way, have a collider on everything and just change the size of that
        //depending on a noise level variable that is changed
        //depending on what the player is currently doing
        //when the collider is entered then something heard the sound get those collider's that are off type enemy
        //and call their investigate sound function and pass in the player's collider's position.
        
        public LayerMask enemyLayer;

        public void MakeSound(Vector3 soundLocation, float loudnessOfSound)
        {
            Collider[] enemiesInRange = Physics.OverlapSphere(soundLocation, loudnessOfSound, enemyLayer);
            if (enemiesInRange.Length > 0)
            {
                foreach (var enemy in enemiesInRange)
                {
                    EnemyMovement enemyScript = enemy.GetComponent<EnemyMovement>();
                    //call notify enemy of sound and pass in sound location for position
                    enemyScript.targetLastKnownPos = soundLocation;
                    enemyScript.ChangeState(EnemyMovement.States.investigating);
                }
            }
        }
    }
}
