using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using InDevelopment.Mechanics.Player;
using InDevelopment.Mechanics.VisionAbility;

public class F_Player : MonoBehaviour
{
    public static EventInstance musicTrack;
    EventInstance amb;
    EventInstance ambInterior;
    EventInstance running;
    EventInstance visionAbilitySound;

    PlayerMovement playerMovement;
    PlayerController playerController;
    VisionAbilityController visionAbility;

    [SerializeField]
    float walkingSpeed;
    [SerializeField]
    float runningSpeed;
    [SerializeField]
    float crouchingSpeed;
    [SerializeField]
    float walkingBackwardSpeed;
    bool runningSoundPlayed;
    bool visionSoundPlayed;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        visionAbility = GetComponent<VisionAbilityController>();

        amb = RuntimeManager.CreateInstance("event:/Ambience/AmbOutside");
        amb.start();

        running = RuntimeManager.CreateInstance("event:/Player/Running");
        visionAbilitySound = RuntimeManager.CreateInstance("event:/Player/Abilties/Vision");

        backgroundMusic();
   
        InvokeRepeating("FootstepsWalk", 0, walkingSpeed);
        InvokeRepeating("FootstepsRun", 0, runningSpeed);
        InvokeRepeating("CrouchingWalk", 0, crouchingSpeed);
        InvokeRepeating("WalkingBackWards", 0, walkingBackwardSpeed);
    }

    private void Update()
    {
        if (playerMovement.isMoving == true && playerMovement.isSprinting == true && playerMovement.isGrounded == true)
        {
            StartCoroutine(isRunning());
        }
        else if (playerMovement.isSprinting == false)
        {
            running.setParameterByName("Running", 1f, false);
            runningSoundPlayed = false;
        }

        Abilities();
    }

    void Abilities()
    {
        if (visionAbility.isActive && visionSoundPlayed == false)
        {
            visionAbilitySound.start();
            visionAbilitySound.release();
            visionSoundPlayed = true;
        }
        else if (visionAbility.isActive == false)
        {
            visionAbilitySound.setParameterByName("VisionAbilityOff", 1f, false);
            visionSoundPlayed = false;
        }
    }

    IEnumerator isRunning()
    {
        running.setParameterByName("Running", 0f, false);
        yield return new WaitForSeconds(2.9f);

        if (playerMovement.isSprinting == true && runningSoundPlayed == false)
        {
            running.start();
            runningSoundPlayed = true;
        }
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

    private void OnDestroy()
    {
        running.release();
    }
}
