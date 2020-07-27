using UnityEngine;
using UnityEngine.InputSystem;

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


        [Header("Movement Settings")] public float moveSpeed;

        [Header("Jump Settings")] public float jumpForce;
        public LayerMask groundMask;
        
        private bool isJumping;
        private float disToGround;
        
        private void OnEnable()
        {
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }

        private void Awake()
        {
            controls = new GameControls();
            controls.Enable();
            controls.InGame.Jump.performed += JumpInput;
            rb = GetComponent<Rigidbody>();
            disToGround = GetComponent<Collider>().bounds.extents.y;
        }

        private void JumpInput(InputAction.CallbackContext obj)
        {
            if (!isJumping && IsGrounded())
            {
                isJumping = true;
                Jump();
            }
        }

        private bool IsGrounded()
        {
            return Physics.Raycast(transform.position, -Vector3.up, disToGround + 0.1f,groundMask);
        }

        private void Jump()
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private void Move()
        {
            moveDirection = controls.InGame.Movement.ReadValue<Vector2>();
            Vector3 movement = (moveDirection.y * transform.forward) + (moveDirection.x * transform.right);
            rb.AddForce(movement.normalized * moveSpeed);
        }

        private void FixedUpdate()
        {
            //add in bool to stop movement when vision active
            Move();
        }
    }
}