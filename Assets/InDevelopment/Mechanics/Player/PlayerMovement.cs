using System.Threading;
using InDevelopment.Mechanics.ObjectDistraction;
using InDevelopment.Mechanics.VisionAbility;
using UnityEngine;
using UnityEngine.InputSystem;


namespace InDevelopment.Mechanics.Player
{
    /// <summary>
    /// this is player movement based on the rigidbody
    /// </summary>
    [RequireComponent(typeof(PlayerController))]
    public class PlayerMovement : MonoBehaviour
    {
        //TODO remake player prefab to see if it fixes object pick up bug in backstreet level
        private PlayerController controller;
        private Vector2 moveDirection;

        [Header("Misc")]
        [SerializeField]
        private Rigidbody rb;

        [SerializeField]
        private float playerHeight = 3.2f;

        public LayerMask obstacleLayerMask;

        private Vector3 movement;

        [SerializeField]
        private float moveSpeed;

        [Header("Movement Settings")]
        public float walkSpeed;

        public float sprintSpeed;

        public float sprintEnergy = 100f;
        public float sprintDrainRate = 2f;
        public float sprintRefillRate = 2f;
        public float maxSprintEnergy = 100f;

        [HideInInspector]
        public bool isSprinting;

        public float maxSlopeAngle = 120f;
        private float groundAngle;

        [Header("Jump Settings")]
        public float jumpForce;

        [HideInInspector]
        public bool isGrounded;

        private LayerMask playerMask;

        [Header("Other Settings")]
        public Camera cam;

        [SerializeField]
        private GameObject fricStub;

        [SerializeField]
        private Collider fricStubCol;

        private bool visionActivated;

        [HideInInspector]
        public bool isMoving;

        private float disToGround;

        [SerializeField]
        private PhysicMaterial lowFrictionMat;

        [SerializeField]
        private PhysicMaterial highFrictionMat;

        private Vector3 forwardTransform;
        private RaycastHit hitInfo;

        private bool hasForward;

        [HideInInspector]
        public bool isCrouching;

        // [SerializeField]
        private GeneralSoundMaker generalSoundMaker;

        [Header("Noise Settings")]
        public float walkNoiseLevel;

        public float sprintNoiseLevel;

        private float currentNoiseLevel;

        private void Start()
        {
            SetupVariables();
            GetReferences();
        }

        private void SetupVariables()
        {
            visionActivated = false;
            isCrouching = false;
            moveSpeed = walkSpeed;
            currentNoiseLevel = walkNoiseLevel;
            playerMask = 1 << 13;
            playerMask = ~playerMask;
        }

        private void GetReferences()
        {
            controller = GetComponent<PlayerController>();
            VisionAbilityController.visionActivation += () => visionActivated = !visionActivated;
            rb = GetComponent<Rigidbody>();
            disToGround = GetComponent<BoxCollider>().bounds.extents.y;
            generalSoundMaker = GetComponentInChildren<GeneralSoundMaker>();
        }

        public void Walk(InputAction.CallbackContext obj)
        {
            if (isCrouching)
            {
                return;
            }

            moveSpeed = walkSpeed;
            currentNoiseLevel = walkNoiseLevel;
            isSprinting = false;
        }

        public void Sprint(InputAction.CallbackContext obj)
        {
            if (isCrouching || sprintEnergy <= 0)
            {
                return;
            }

            isSprinting = true;
            moveSpeed = sprintSpeed;
            currentNoiseLevel = sprintNoiseLevel;
        }

        //works but hacky because it isn't using events
        private void CrouchCheck()
        {
            if (Keyboard.current.cKey.isPressed)
            {
                Crouch();
            }
            else
            {
                StandUp();
            }
        }

        public void Crouch()
        {
            //TODO: Change how the crouch works. Less snapping, and move legs up instead of head down? or something?...we gotta figure it out
            if (!isCrouching)
            {
                isCrouching = true;
                moveSpeed = walkSpeed / 2;
                transform.localScale = new Vector3(1, transform.localScale.y / 2f, 1);
            }
        }

        public void StandUp()
        {
            if (isCrouching)
            {
                bool canStand =
                    !Physics.Raycast(transform.position, Vector3.up, playerHeight + 0.3f, obstacleLayerMask);
                //TODO if you press sprint while crouching it keeps player crouched permanently
                if (canStand && !isSprinting)
                {
                    isCrouching = false;
                    moveSpeed = walkSpeed;
                    transform.localScale = new Vector3(1, transform.localScale.y * 2f, 1);
                }
            }
        }

        public void MoveInput(InputAction.CallbackContext obj)
        {
            controller.moveDirection = obj.ReadValue<Vector2>();
        }

        private bool IsGrounded()
        {
            Vector3 boxColliderTransform = new Vector3(fricStub.transform.localScale.x / 4,
                fricStub.transform.localScale.y / 4, fricStub.transform.localScale.z / 4);
            Collider[] cols = Physics.OverlapBox(fricStub.transform.position, boxColliderTransform, Quaternion.identity,
                playerMask);
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


        public void Jump(InputAction.CallbackContext obj)
        {
            if (IsGrounded() && !isCrouching)
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

            movement = (controller.moveDirection.y * forwardTransform) + (controller.moveDirection.x * transform.right);
            if (IsGrounded() && hasForward)
            {
                //GroundSpeed
                rb.velocity = movement.normalized * (moveSpeed * Time.deltaTime);
            }
            else
            {
                //AirSpeed
                //TODO:Test the new air control, adjust later if needed.
                // rb.AddForce((movement.normalized * (moveSpeed * Time.deltaTime)) / 2, ForceMode.Acceleration);
                rb.AddForce((movement.normalized * (moveSpeed * Time.deltaTime)), ForceMode.Acceleration);
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

        private bool IsMoving()
        {
            if (controller.moveDirection.x < 0 || controller.moveDirection.x > 0 || controller.moveDirection.y < 0 ||
                controller.moveDirection.y > 0)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }

            return isMoving;
        }

        private void CheckSprint()
        {
            if (isSprinting && isMoving && !visionActivated && !isCrouching)
            {
                sprintEnergy -= Time.deltaTime * sprintDrainRate;
            }
            else
            {
                sprintEnergy += Time.deltaTime * sprintRefillRate;
            }

            if (sprintEnergy >= maxSprintEnergy)
            {
                sprintEnergy = maxSprintEnergy;
            }

            if (sprintEnergy <= 0f)
            {
                moveSpeed = walkSpeed;
                sprintEnergy = 0f;
            }
        }

        private void SwapPhysicsMats()
        {
            if (IsMoving() && !visionActivated)
            {
                fricStubCol.sharedMaterial = lowFrictionMat;
                generalSoundMaker.MakeSound(currentNoiseLevel);
            }
            else
            {
                fricStubCol.sharedMaterial = highFrictionMat;
            }
        }

        private void VisionActivated()
        {
            if (!visionActivated)
            {
                ApplyMovement();
            }
            else
            {
                if (isGrounded)
                {
                    rb.velocity = Vector3.zero;
                    //moveSpeed = 0;
                }
            }
        }

        private void Update()
        {
            CalculateForward();
            CalculateGroundAngle();
            CheckForwardTransform();
            CrouchCheck();
        }

        private void FixedUpdate()
        {
            VisionActivated();
            SwapPhysicsMats();
            CheckSprint();
        }
    }
}