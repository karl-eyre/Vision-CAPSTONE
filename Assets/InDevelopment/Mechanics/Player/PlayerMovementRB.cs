﻿using UnityEngine;
using UnityEngine.InputSystem;
using VisionAbility;

namespace Player
{
    /// <summary>
    /// this is player movement based on the rigidbody
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class PlayerMovementRB : MonoBehaviour
    {
        private GameControls controls;
        private Vector2 moveDirection;
        private Rigidbody rb;
        private Transform rbT;
        private Vector3 movement;

        [Header("Movement Settings")]
        public float moveSpeed;

        [Header("Jump Settings")]
        public float jumpForce;

        public LayerMask groundMask;
        public Camera cam;
        public Transform fricStub;

        private bool visionActivated;

        private float disToGround;

        private void OnEnable()
        {
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }

        private void SetUpControls()
        {
            controls = new GameControls();
            controls.Enable();
            controls.InGame.Jump.performed += JumpInput;
            controls.InGame.Movement.performed += MoveInput;
            controls.InGame.Movement.canceled += MoveInput;
            controls.InGame.Crouch.started += Crouch;
            controls.InGame.Crouch.canceled += UnCrouch;
        }

        private void GetReferences()
        {
            VisionAbilityController.visionActivation += () => visionActivated = !visionActivated;
            rb = GetComponent<Rigidbody>();
            rbT = rb.transform;
            disToGround = GetComponent<Collider>().bounds.extents.y;
        }

        private void Start()
        {
            SetUpControls();
            visionActivated = false;
            GetReferences();
        }

        private void UnCrouch(InputAction.CallbackContext obj)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        private void Crouch(InputAction.CallbackContext obj)
        {
            transform.localScale = new Vector3(1f, 0.5f, 1f);
        }

        /// <summary>
        /// Currently the movement works.. but it wont continute to move forward when you change direction while still holding down W
        /// Will continute to look into this but for now its too late and im gonna finish up for the night!
        /// </summary>
        /// <param name="obj"></param>
        private void JumpInput(InputAction.CallbackContext obj)
        {
            Jump();
        }

        private bool IsGrounded()
        {
            Debug.DrawRay(transform.position, -Vector3.up * (disToGround - 0.9f), Color.red, 0.2f);
            //Debug.DrawLine(transform.position, -Vector3.up * disToGround, Color.red, 0.2f);
            //Physics.OverlapSphere(fricStub.position, 1.1f, groundMask);
            Collider[] cols = Physics.OverlapSphere(fricStub.position, 0.1f, groundMask);
            if (cols.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

            //return Physics.Raycast(transform.position, -Vector3.up, (disToGround - 0.9f), groundMask);
        }

        private void Jump()
        {
            if (IsGrounded())
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        /// <summary>
        /// Currently the movement works.. but it wont continute to move forward when you change direction while still holding down W
        /// Will continute to look into this but for now its too late and im gonna finish up for the night!
        /// </summary>
        /// <param name="obj"></param>
        private void MoveInput(InputAction.CallbackContext obj)
        {
            moveDirection = controls.InGame.Movement.ReadValue<Vector2>();
        }

        void ApplyMovement()
        {
            movement = (moveDirection.y * rbT.forward) + (moveDirection.x * rbT.right);
            if (IsGrounded())
            {
                //GroundSpeed
                //rb.AddForce(movement.normalized * moveSpeed);
                rb.velocity = movement.normalized * (moveSpeed * Time.deltaTime);
            }
            else
            {
                //AirSpeed
                rb.AddForce((movement.normalized * (moveSpeed * Time.deltaTime)) / 2);
            }
        }

        private void FixedUpdate()
        {
            //add in bool to stop movement when vision active

            if (!visionActivated)
            {
                ApplyMovement();
            }
            else
            {
                //comment out if you want to slide a little when vision is activated
                rb.velocity = Vector3.zero;
            }
        }
    }
}