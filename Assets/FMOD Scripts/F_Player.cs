using FMOD.Studio;
using FMODUnity;
using InDevelopment.Mechanics.Player;
using InDevelopment.Mechanics.VisionAbility;
using System.Collections;
using UnityEngine;

public class F_Player : MonoBehaviour
{
    EventInstance amb;
    EventInstance running;
    EventInstance visionAbilitySound;
    EventInstance footsteps;

    PlayerMovement playerMovement;
    PlayerController playerController;

    [SerializeField]
    float walkingSpeed = 0.5f;
    [SerializeField]
    float runningSpeed = 0.3f;
    [SerializeField]
    float walkingBackwardSpeed = 0.6f;

    //float crouchingSpeed = 0.7f;
    bool runningSoundPlayed;
    bool visionSoundPlayed;
    bool finishedRunning;

    EventInstance musicTest;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        amb = RuntimeManager.CreateInstance("event:/Ambience/AmbInterior");
        amb.start();

        running = RuntimeManager.CreateInstance("event:/Player/Running");
        visionAbilitySound = RuntimeManager.CreateInstance("event:/Player/Abilties/Vision");

        footsteps = RuntimeManager.CreateInstance("event:/Player/Footsteps");

        InvokeRepeating("FootstepsWalk", 0, walkingSpeed);
        InvokeRepeating("FootstepsRun", 0, runningSpeed);
        InvokeRepeating("WalkingBackWards", 0, walkingBackwardSpeed);
     
        VisionAbilityController.visionActivation += VisionAbilitySoundPlay;
    }

    private void Update()
    {
        RunningSound();
    }

    public void TurningSound()
    {
        if (playerMovement.isMoving == true)
        {
            EventInstance turning = RuntimeManager.CreateInstance("event:/Player/TurningFast");
            turning.setParameterByName("StandingStill", 1, false);
            turning.start();
            turning.release();
        }
        else
        {
            EventInstance turning = RuntimeManager.CreateInstance("event:/Player/TurningFast");
            turning.setParameterByName("StandingStill", 0, false);
            turning.start();
            turning.release();
        }
    }

    void RunningSound()
    {
        if (playerMovement.isMoving == true && playerMovement.isSprinting == true && playerMovement.isGrounded == true && runningSoundPlayed == false)
        {
            running.start();
            
            runningSoundPlayed = true;
        }
        else if (playerMovement.isSprinting == false)
        {
            running.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            runningSoundPlayed = false;

            if (finishedRunning == true)
            {
                RuntimeManager.PlayOneShot("event:/Player/Skids", default);
                finishedRunning = false;
            }
        }
    }

    void VisionAbilitySoundPlay()
    {
        if (visionSoundPlayed == false)
        {
            visionAbilitySound.start();
            visionAbilitySound.release();
            visionSoundPlayed = true;
        }
        else
        {
            visionAbilitySound.setParameterByName("VisionAbilityOff", 1f, false);
            visionSoundPlayed = false;
        }        
    }

    void FootstepsWalk()
    {
        if (playerMovement.isMoving == true && playerMovement.isSprinting == false && playerMovement.isGrounded == true)
        {
            RuntimeManager.PlayOneShot("event:/Player/Footsteps", default);
            //footsteps.setParameterByName("Run&Walk", 0, false);
            //footsteps.start();
        }
    }
    void FootstepsRun()
    {
        if (playerMovement.isMoving == true && playerMovement.isSprinting == true && playerMovement.isGrounded == true)
        {
            footsteps.setParameterByName("Run&Walk", 1, false);
            footsteps.start();
        }
    }    
    void WalkingBackWards()
    {
        if (playerMovement.isMoving == true && playerMovement.isGrounded == true && playerController.moveDirection.y < 0)
        {
            RuntimeManager.PlayOneShot("event:/Player/Footsteps", default);
        }
    }

    private void OnDestroy()
    {
        running.release();
        running.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        VisionAbilityController.visionActivation -= VisionAbilitySoundPlay;
    }
}
