
using UnityEngine;
using UnityEngine.InputSystem;

namespace InDevelopment.Mechanics.Player
{
    public class PlayerController : MonoBehaviour
    {
        private GameControls controls;
        private PlayerMovement playerMovement;
        private TeleportAbilitiesUI teleportAbilitiesUI;

        [HideInInspector]
        public Vector2 moveDirection;

        private void Awake()
        {
            //Get references must always be called first
            GetReferences();
            SetUpControls();
        }

        private void GetReferences()
        {
            playerMovement = GetComponent<PlayerMovement>();
            teleportAbilitiesUI = GetComponentInChildren<TeleportAbilitiesUI>();
        }

        //maybe add other abilities here
        private void SetUpControls()
        {
            controls = new GameControls();
            controls.Enable();
            controls.InGame.Jump.performed += playerMovement.Jump;
            controls.InGame.Movement.performed += playerMovement.MoveInput;
            controls.InGame.Movement.canceled += playerMovement.MoveInput;
            // controls.InGame.Crouch.started += playerMovement.Crouch;
            // controls.InGame.Crouch.canceled += playerMovement.StandUp;
            controls.InGame.Sprint.started += playerMovement.Sprint;
            controls.InGame.Sprint.canceled += playerMovement.Walk;
        }

        private void OnEnable()
        {
            if (controls != null) controls.Enable();
        }

        private void OnDisable()
        {
            if (controls != null) controls.Disable();
        }
    }
}