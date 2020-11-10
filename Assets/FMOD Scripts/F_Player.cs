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
    EventInstance musicTest;
    //EventInstance footsteps;
    PlayerMovement playerMovement;
    private PlayerController playerController;
    [SerializeField]
    float walkingSpeed = 0.5f;
    [SerializeField]
    float runningSpeed = 0.3f;
    [SerializeField]
    float walkingBackwardSpeed = 0.6f;
    [SerializeField]
    float crouchingSpeed = 0.6f;
    bool runningSoundPlayed;
    bool visionSoundPlayed;
    bool finishedRunning;
    bool crouched;

    void Start()
    {
        FmodEventInstances();
        InvokeRepeating("FootstepsWalk", 0, walkingSpeed);
        InvokeRepeating("FootstepsRun", 0, runningSpeed);
        InvokeRepeating("WalkingBackWards", 0, walkingBackwardSpeed);
        InvokeRepeating("CrouchingWalk", 0, crouchingSpeed);
        playerMovement = GetComponent<PlayerMovement>();
        playerController = GetComponent<PlayerController>();
        VisionAbilityController.visionActivation += VisionAbilitySoundPlay;
    }
    void FmodEventInstances()
    {
        running = RuntimeManager.CreateInstance("event:/Player/Running");
        visionAbilitySound = RuntimeManager.CreateInstance("event:/Player/Abilties/Vision");
    }
    private void Update()
    {
        RunningSound();
        //Debug.Log(playerMovement.isCrouching);
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
            running.release();
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
        if (playerMovement.isMoving == true && playerMovement.isCrouching == false && playerMovement.isSprinting == false && playerMovement.isGrounded == true)
        {
            EventInstance footsteps = RuntimeManager.CreateInstance("event:/Player/Footsteps");
            footsteps.start();
            footsteps.release();
        }
    }
    void FootstepsRun()
    {
        if (playerMovement.isMoving == true && playerMovement.isSprinting == true && playerMovement.isGrounded == true)
        {           
            EventInstance footsteps = RuntimeManager.CreateInstance("event:/Player/Footsteps");
            footsteps.setParameterByName("Run&Walk", 1, false);
            footsteps.start();
            footsteps.release();
        }
    }    
    void WalkingBackWards()
    {
        if (playerMovement.isMoving == true && playerMovement.isGrounded == true && playerController.moveDirection.y < 0)
        {
            EventInstance footsteps = RuntimeManager.CreateInstance("event:/Player/Footsteps");
            footsteps.start();
            footsteps.release();
        }
    }
    void CrouchingWalk()
    {
        if (playerMovement.isMoving == true && playerMovement.isGrounded == true && playerMovement.isCrouching == true)
        {
            EventInstance footsteps = RuntimeManager.CreateInstance("event:/Player/Footsteps");
            footsteps.setParameterByName("Crouching", 1, false);
            footsteps.setParameterByName("Run&Walk", 3, false);
            footsteps.start();
            footsteps.release();
        }
    }
    private void OnDestroy()
    {
        running.release();
        running.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        VisionAbilityController.visionActivation -= VisionAbilitySoundPlay;
    }
}
