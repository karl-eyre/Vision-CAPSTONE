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
        private Vector3 movement;

        [Header("Movement Settings")] public float moveSpeed;

        [Header("Jump Settings")] public float jumpForce;
        public LayerMask groundMask;
        public Camera cam;
        
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
            controls.InGame.Movement.performed += Move;
            controls.InGame.Movement.canceled += Move;
            // controls.InGame.Movement.canceled += MoveStop;
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

        /// <summary>
        /// Currently the movement works.. but it wont continute to move forward when you change direction while still holding down W
        /// Will continute to look into this but for now its too late and im gonna finish up for the night!
        /// </summary>
        /// <param name="obj"></param>
        private void Move(InputAction.CallbackContext obj)
        {
            Transform ct = cam.transform;
            moveDirection = controls.InGame.Movement.ReadValue<Vector2>();
            movement = (moveDirection.y * ct.forward) + (moveDirection.x * ct.right);
            //rb.AddForce(movement.normalized * moveSpeed);
        }

        // private void MoveStop(InputAction.CallbackContext context)
        // {
        //     Vector2 vel = rb.velocity;
        //     Vector2 slowDown;
        //     slowDown.x = Mathf.Lerp(vel.x, 0, 0.5f);
        //     slowDown.y = Mathf.Lerp(vel.y, 0, 0.5f);
        //     
        //     rb.velocity = new Vector3(slowDown.x, 0, slowDown.y);
        // }

        private void FixedUpdate()
        {
            //add in bool to stop movement when vision active
            rb.AddForce(movement.normalized * moveSpeed);
        }
    }
}