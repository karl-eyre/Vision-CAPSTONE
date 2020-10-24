using System;
using UnityEngine;

namespace InDevelopment.Mechanics.Player
{
    public class HeadBobScript : MonoBehaviour
    {
        private float timer = 0f;
        private float bobbingSpeed;
        public float walkingBobbingSpeed = 14f;
        public float sprintingBobbingSpeed = 20f;
        public float bobbingAmount = 0.1f;
        private float defaultPosY;

        private PlayerMovement playerMovement;
        
        private void Start()
        {
            playerMovement = GetComponentInParent<PlayerMovement>();
            //Ensure this pos is the cameras current y pos
            defaultPosY = 3.1f;
        }

        private void Update()
        {
            UpdateBobbingSpeed();
            HeadBob();
        }

        private void UpdateBobbingSpeed()
        {
            if (playerMovement.isMoving)
            {
                if (playerMovement.isSprinting)
                {
                    bobbingSpeed = sprintingBobbingSpeed;
                }
                else
                {
                    bobbingSpeed = walkingBobbingSpeed;
                }
            }
        }

        private void HeadBob()
        {
            if(playerMovement.isMoving)
            {
                //Player is moving
                timer += Time.deltaTime * bobbingSpeed;
                transform.localPosition = new Vector3(transform.localPosition.x, defaultPosY + Mathf.Sin(timer) * bobbingAmount, transform.localPosition.z);
            }
            else
            {
                //Idle
                timer = 0;
                transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * bobbingSpeed), transform.localPosition.z);
            }
        }
    }
}
