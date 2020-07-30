using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// CC stands for character controller
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class PlayerMovementCC : MonoBehaviour
    {
        private CharacterController characterController;
        private GameControls controls;
        private Vector2 move;

        [Header("Player Settings")] [SerializeField]
        private int movementSpeed;

        [Header("Gravity Settings")] [SerializeField]
        private float gravity = -9.81f;

        [Header("Jump Settings")] [SerializeField]
        private AnimationCurve jumpFallOff;

        [SerializeField] private float jumpMultipler;

        private bool isJumping;
        private Vector3 velocity;

        private void Awake()
        {
            controls = new GameControls();
            controls.Enable();
            controls.InGame.Jump.performed += JumpInput;
            characterController = GetComponent<CharacterController>();
        }

        private void JumpInput(InputAction.CallbackContext obj)
        {
            if (!isJumping)
            {
                isJumping = true;
                StartCoroutine(JumpEvent());
            }
        }

        private IEnumerator JumpEvent()
        {
            characterController.slopeLimit = 90f;
            float timeInAir = 0.0f;

            do
            {
                float jumpForce = jumpFallOff.Evaluate(timeInAir);
                characterController.Move(Vector3.up * jumpForce * jumpMultipler * Time.deltaTime);
                timeInAir += Time.deltaTime;
                yield return null;
            } while (!characterController.isGrounded && characterController.collisionFlags != CollisionFlags.Above);

            characterController.slopeLimit = 45f;
            isJumping = false;
        }

        private void Update()
        {
            //add in bool to stop movement when vision active
            PlayerMovement();
        }

        private void PlayerMovement()
        {
            move = controls.InGame.Movement.ReadValue<Vector2>();
            Vector3 moveDirection = new Vector3(move.x, 0, move.y);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection.y -= gravity * Time.deltaTime;
            characterController.Move(moveDirection * movementSpeed * Time.deltaTime);
        }

        private void OnEnable()
        {
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }
    }
}