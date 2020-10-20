using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using InDevelopment.Mechanics.Player;

public class F_Player : MonoBehaviour
{
    public static EventInstance musicTrack;
    EventInstance amb;
    EventInstance ambInterior;

    PlayerMovement playerMovement;
    PlayerController playerController;

    [SerializeField]
    float walkingSpeed;
    [SerializeField]
    float runningSpeed;
    [SerializeField]
    float crouchingSpeed;
    [SerializeField]
    float walkingBackwardSpeed;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        amb = RuntimeManager.CreateInstance("event:/Ambience/AmbOutside");
        amb.start();

        backgroundMusic();
   
        InvokeRepeating("FootstepsWalk", 0, walkingSpeed);
        InvokeRepeating("FootstepsRun", 0, runningSpeed);
        InvokeRepeating("CrouchingWalk", 0, crouchingSpeed);
        InvokeRepeating("WalkingBackWards", 0, walkingBackwardSpeed);
    }

    void backgroundMusic()
    {
        musicTrack = RuntimeManager.CreateInstance("event:/Music/AmbientMusic");
        musicTrack.start();
        musicTrack.release();
    }

    void FootstepsWalk()
    {
        if (playerMovement.isMoving == true && playerMovement.isSprinting == false && playerMovement.isGrounded == true)
        {
            RuntimeManager.PlayOneShot("event:/Player/Footsteps", default);
        }
    }
    void FootstepsRun()
    {
        if (playerMovement.isMoving == true && playerMovement.isSprinting == true && playerMovement.isGrounded == true)
        {
            RuntimeManager.PlayOneShot("event:/Player/Footsteps", default);
        }
    }    
    void CrouchingWalk()
    {
        if (playerMovement.isMoving == true && playerMovement.isCrouching == true && playerMovement.isGrounded == true)
        {
            RuntimeManager.PlayOneShot("event:/Player/Footsteps", default);
        }
    }
    void WalkingBackWards()
    {
        if (playerMovement.isMoving == true && playerMovement.isCrouching == true && playerMovement.isGrounded == true && playerController.moveDirection.y < 0)
        {
            RuntimeManager.PlayOneShot("event:/Player/Footsteps", default);
        }
    }
}
