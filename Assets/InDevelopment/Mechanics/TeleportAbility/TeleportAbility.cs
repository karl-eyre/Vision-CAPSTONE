using System;
using System.Collections;
using InDevelopment.Mechanics.ObjectDistraction;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;


namespace InDevelopment.Mechanics.TeleportAbility
{
    public class TeleportAbility : MonoBehaviour
    {
        private GameControls controls;

        [Header("Camera")]
        [SerializeField]
        private Camera camera = null;

        public float teleportRange;
        public float teleportDelay;
        public float teleportStartUpDelay = 1f;
        public Vector3 teleportAssistRange;
        public LayerMask levelLayer;

        public bool canTeleport;
        public GameObject targetObj;

        [SerializeField]
        private float heightOffset = 0.5f;

        private RaycastHit hitInfo;
        private Ray ray;

        private ObjectSoundMaker objectSoundMaker;

        public float noiseLevel;

        private bool onCooldown;
        public float cooldownTimer;

        public static event Action teleportStarted;
        public static event Action teleportTriggered;


        private void Start()
        {
            if (camera == null)
            {
                camera = Camera.main;
            }
            SetUpControls();
            SetReferences();
        }

        private void OnEnable()
        {
            SetUpControls();
            SetReferences();
        }

        private void SetReferences()
        {
            objectSoundMaker = GetComponent<ObjectSoundMaker>();
            // teleportLayer = LayerMask.GetMask("ThrowableObjects");
            // phaseableLayer = LayerMask.GetMask("Phaseable");
            onCooldown = false;
        }

        private void SetUpControls()
        {
            controls = new GameControls();
            controls.Enable();
            if (controls.InGame.TeleportToItem != null) controls.InGame.TeleportToItem.performed += ReadTeleportInput;
        }

        private void ReadTeleportInput(InputAction.CallbackContext obj)
        {
            if (CanTeleport() && !onCooldown)
            {
                onCooldown = true;
                StartCoroutine(Teleport(targetObj));
            }
        }

        private void Update()
        {
            if (onCooldown && cooldownTimer <= teleportStartUpDelay)
            {
                cooldownTimer = cooldownTimer + Time.deltaTime;
            }
        }

        private IEnumerator Teleport(GameObject targetObject)
        {
            if (teleportStarted != null) teleportStarted?.Invoke();
            yield return new WaitForSeconds(teleportStartUpDelay);
            var tgt = targetObject;
            Vector3 origin = new Vector3(transform.position.x, transform.position.y + heightOffset,
                transform.position.z);
            transform.position = tgt.transform.position;
            tgt.transform.position = origin;
            tgt.GetComponent<Rigidbody>().velocity = Vector3.zero;
            targetObj = null;
            objectSoundMaker.MakeSound(transform.position, noiseLevel);
            if (teleportTriggered != null) teleportTriggered?.Invoke();
            yield return new WaitForSeconds(teleportDelay);
            onCooldown = false;
            cooldownTimer = 0;
        }


        private bool CanTeleport()
        {
            Vector3 mousePosition = controls.InGame.MousePosition.ReadValue<Vector2>();

            if (!(camera is null)) ray = camera.ScreenPointToRay(mousePosition);

            bool isHit = Physics.Raycast(ray, out hitInfo, teleportRange, levelLayer);

            if (isHit)
            {
                Collider[] colliders = Physics.OverlapBox(hitInfo.point, teleportAssistRange, Quaternion.identity);

                foreach (var collider in colliders)
                {
                    if (collider.CompareTag("ThrowableObjects"))
                    {
                        targetObj = collider.gameObject;
                        // if (RoomForTeleport(targetObj))
                        // {
                        canTeleport = true;
                        // }
                        // else
                        // {
                        //     canTeleport = false;
                        //     targetObj = null;
                        // }
                    }
                    else
                    {
                        canTeleport = false;
                        targetObj = null;
                    }
                }
            }

            return canTeleport;
        }

        private bool RoomForTeleport(GameObject targetObject)
        {
            Vector3 newPos = new Vector3(targetObject.transform.position.x, targetObject.transform.position.y + 1.6f,
                targetObject.transform.position.z);

            Collider[] colliders = Physics.OverlapBox(newPos, targetObj.transform.localScale / 2,
                Quaternion.identity);

            if (colliders.Length == 1)
            {
                return true;
            }

            return false;
        }
    }
}