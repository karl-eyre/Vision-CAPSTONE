using InDevelopment.Mechanics.ObjectDistraction;
using UnityEngine;
using UnityEngine.InputSystem;
using VisionAbility;

namespace InDevelopment.Mechanics.Player
{
    /// <summary>
    /// this is player movement based on the rigidbody
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class PlayerMovementRB : MonoBehaviour
    {
        private GameControls controls;
        private Vector2 moveDirection;

        [SerializeField]
        private Rigidbody rb;

        private Transform rbTransform;
        private Vector3 movement;
        
        private float moveSpeed;
        
        [Header("Movement Settings")]
        public float walkSpeed;
        public float sprintSpeed;
        public float walkNoiseLevel;
        public float sprintNoiseLevel;
        
        private float currentNoiseLevel;
        
        public float maxSlopeAngle = 120;
        private float groundAngle;

        [Header("Jump Settings")]
        public float jumpForce;
        public bool isGrounded;
        public LayerMask playerMask;
        
        [Header("Other Settings")]
        public Camera cam;

        [SerializeField]
        private GameObject fricStub;

        [SerializeField]
        private Collider fricStubCol;

        private bool visionActivated;
        private bool isMoving;

        private float disToGround;

        [SerializeField]
        private PhysicMaterial lowFrictionMat;

        [SerializeField]
        private PhysicMaterial frictionMat;

        private Vector3 forwardTransform;
        private RaycastHit hitInfo;
        
        private bool hasForward;
        private bool isCrouching;

        [SerializeField]
        private GeneralSoundMaker generalSoundMaker;
        
        
        private void OnEnable()
        {
            if (controls != null) controls.Enable();
        }

        private void OnDisable()
        {
            if (controls != null) controls.Disable();
        }

        private void SetUpControls()
        {
            controls = new GameControls();
            controls.Enable();
            controls.InGame.Jump.performed += ReadJumpInput;
            controls.InGame.Movement.performed += ReadMoveInput;
            controls.InGame.Movement.canceled += ReadMoveInput;
            controls.InGame.Crouch.started += ReadCrouch;
            controls.InGame.Crouch.canceled += ReadUnCrouch;
            controls.InGame.Sprint.started += ReadSprint;
            controls.InGame.Sprint.canceled += ReadWalk;
        }
        
        private void Start()
        {
            SetUpControls();
            visionActivated = false;
            moveSpeed = walkSpeed;
            currentNoiseLevel = walkNoiseLevel;
            playerMask = 1 << 13;
            playerMask = ~playerMask;
            GetReferences();
        }
        
        private void GetReferences()
        {
            VisionAbilityController.visionActivation += () => visionActivated = !visionActivated;
            rb = GetComponent<Rigidbody>();
            rbTransform = rb.transform;
            disToGround = GetComponent<Collider>().bounds.extents.y;
            generalSoundMaker = GetComponent<GeneralSoundMaker>();
        }

        private void ReadWalk(InputAction.CallbackContext obj)
        {
            moveSpeed = walkSpeed;
            currentNoiseLevel = walkNoiseLevel;
        }

        private void ReadSprint(InputAction.CallbackContext obj)
        {
            if(isCrouching)
            {
                return;
            }
            moveSpeed = sprintSpeed;
            currentNoiseLevel = sprintNoiseLevel;
        }

        private void ReadUnCrouch(InputAction.CallbackContext obj)
        {
            isCrouching = false;
            transform.localScale = new Vector3(1f, transform.localScale.y * 2, 1f);
        }

        private void ReadCrouch(InputAction.CallbackContext obj)
        {
            isCrouching = true;
            transform.localScale = new Vector3(1f, transform.localScale.y / 2, 1f);
        }

        private void ReadJumpInput(InputAction.CallbackContext obj)
        {
            Jump();
        }

        private void ReadMoveInput(InputAction.CallbackContext obj)
        {
            moveDirection = controls.InGame.Movement.ReadValue<Vector2>();
        }

        private bool IsGrounded()
        {
            Vector3 boxColliderTransform = new Vector3(fricStub.transform.localScale.x/4,fricStub.transform.localScale.y/4,fricStub.transform.localScale.z/4);
            Collider[] cols = Physics.OverlapBox(fricStub.transform.position, boxColliderTransform,Quaternion.identity, playerMask);
            if (cols.Length == 0)
            {
                isGrounded = false;
            }
            else
            {
                isGrounded = true;
            }

            return isGrounded;
        }

        private void Jump()
        {
            if (IsGrounded())
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
        
        private void ApplyMovement()
        {
            //ensures that if the slope is too high then you can't move
            if (groundAngle >= maxSlopeAngle)
            {
                return;
            }

            movement = (moveDirection.y * forwardTransform) + (moveDirection.x * transform.right);
            if (IsGrounded() && hasForward)
            {
                //GroundSpeed
                rb.velocity = movement.normalized * (moveSpeed * Time.deltaTime);
                
            }
            else
            {
                //AirSpeed
                rb.AddForce((movement.normalized * (moveSpeed * Time.deltaTime)) / 2, ForceMode.Acceleration);
            }
        }

        private void CalculateForward()
        {
            if (!hasForward)
            {
                forwardTransform = transform.forward;
                return;
            }

            forwardTransform = Vector3.Cross(hitInfo.normal, -transform.right);
        }

        private void CalculateGroundAngle()
        {
            if (!hasForward)
            {
                groundAngle = 90;
                return;
            }

            groundAngle = Vector3.Angle(transform.forward, hitInfo.normal);
        }

        private void CheckForwardTransform()
        {
            float distance = disToGround + transform.localScale.y;

            Ray ray = new Ray(fricStub.transform.position, -Vector3.up);

            if (Physics.Raycast(ray, out hitInfo, distance, playerMask))
            {
                hasForward = true;
            }
            else
            {
                hasForward = false;
            }
        }

        //TODO:Move into a script on the fric stub
        private bool IsMoving()
        {
            if (moveDirection.x < 0 || moveDirection.x > 0 || moveDirection.y < 0 || moveDirection.y > 0)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }

            return isMoving;
        }

        //TODO: remove debug/gizmos after fully tested
        private void DrawDebugLines()
        {
            Debug.DrawLine(rbTransform.transform.position, rbTransform.transform.position + forwardTransform, Color.blue);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Vector3 gizmoBox = new Vector3(fricStub.transform.localScale.x/4,fricStub.transform.localScale.y/4,fricStub.transform.localScale.z/4);
            Gizmos.DrawWireCube(fricStub.transform.position, gizmoBox);
        }

        private void Update()
        {
            CalculateForward();
            CalculateGroundAngle();
            CheckForwardTransform();
            DrawDebugLines();
        }

        private void FixedUpdate()
        {
            if (!visionActivated)
            {
                ApplyMovement();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
            
            //TODO:Move into a script on the fric stub
            if (IsMoving())
            {
                fricStubCol.sharedMaterial = lowFrictionMat;
                generalSoundMaker.MakeSound(currentNoiseLevel);
            }
            else
            {
                fricStubCol.sharedMaterial = frictionMat;
            }
        }
    }
}