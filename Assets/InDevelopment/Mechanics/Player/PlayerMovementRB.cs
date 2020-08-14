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

        private Transform rbT;
        private Vector3 movement;

        [Header("Movement Settings")]
        public float moveSpeed;

        public float maxGroundAngle = 120;

        [SerializeField]
        private float groundAngle;

        [Header("Jump Settings")]
        public float jumpForce;

        public LayerMask groundMask;
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

        public bool isGrounded;
        private bool hasForward;

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
            controls.InGame.Jump.performed += JumpInput;
            controls.InGame.Movement.performed += MoveInput;
            controls.InGame.Movement.canceled += MoveInput;
            controls.InGame.Crouch.started += Crouch;
            controls.InGame.Crouch.canceled += UnCrouch;
            controls.InGame.Sprint.started += Sprint;
            controls.InGame.Sprint.canceled += Walk;
        }
        
        private void Start()
        {
            SetUpControls();
            visionActivated = false;
            GetReferences();
        }
        
        private void GetReferences()
        {
            VisionAbilityController.visionActivation += () => visionActivated = !visionActivated;
            rb = GetComponent<Rigidbody>();
            rbT = rb.transform;
            disToGround = GetComponent<Collider>().bounds.extents.y;
        }


        private void Walk(InputAction.CallbackContext obj)
        {
            moveSpeed = 335;
        }

        private void Sprint(InputAction.CallbackContext obj)
        {
            moveSpeed = 460;
        }

        private void UnCrouch(InputAction.CallbackContext obj)
        {
            transform.localScale = new Vector3(1f, transform.localScale.y * 2, 1f);
        }

        private void Crouch(InputAction.CallbackContext obj)
        {
            transform.localScale = new Vector3(1f, transform.localScale.y / 2, 1f);
        }

        private void JumpInput(InputAction.CallbackContext obj)
        {
            Jump();
        }

        private void MoveInput(InputAction.CallbackContext obj)
        {
            moveDirection = controls.InGame.Movement.ReadValue<Vector2>();
        }

        private bool IsGrounded()
        {
            Vector3 boxColliderTransform = new Vector3(fricStub.transform.localScale.x/4,fricStub.transform.localScale.y/4,fricStub.transform.localScale.z/4);
            Collider[] cols = Physics.OverlapBox(fricStub.transform.position, boxColliderTransform,Quaternion.identity, groundMask);
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
        
        void ApplyMovement()
        {
            //ensures that if the slope is too high then you can't move
            if (groundAngle >= maxGroundAngle)
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

            if (Physics.Raycast(ray, out hitInfo, distance, groundMask))
            {
                hasForward = true;
            }
            else
            {
                hasForward = false;
            }
        }

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

        private void DrawDebugLines()
        {
            Debug.DrawLine(rbT.transform.position, rbT.transform.position + forwardTransform, Color.blue);
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

            if (IsMoving())
            {
                fricStubCol.sharedMaterial = lowFrictionMat;
            }
            else
            {
                fricStubCol.sharedMaterial = frictionMat;
            }
        }
    }
}