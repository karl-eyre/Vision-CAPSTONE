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
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        amb = RuntimeManager.CreateInstance("event:/Ambience/AmbOutside");
        amb.start();

        running = RuntimeManager.CreateInstance("event:/Player/Running");
        visionAbilitySound = RuntimeManager.CreateInstance("event:/Player/Abilties/Vision");
   
        InvokeRepeating("FootstepsWalk", 0, walkingSpeed);
        InvokeRepeating("FootstepsRun", 0, runningSpeed);
        InvokeRepeating("WalkingBackWards", 0, walkingBackwardSpeed);

        VisionAbilityController.visionActivation += VisionAbilitySoundPlay;
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
        VisionAbilityController.visionActivation -= VisionAbilitySoundPlay;
    }
}
