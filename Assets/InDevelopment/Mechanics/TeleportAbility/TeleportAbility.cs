using System;
using System.Collections;
using InDevelopment.Mechanics.ObjectDistraction;
using UnityEngine;
using UnityEngine.InputSystem;


namespace InDevelopment.Mechanics.TeleportAbility
{
    public class TeleportAbility : MonoBehaviour
    {
        private GameControls controls;

        [Header("Camera")]
        [SerializeField]
        private Camera camera;

        public float teleportRange;
        public float teleportDelay;
        public float teleportStartUpDelay = 1f;
        public LayerMask levelLayer;

        private bool canTeleport;
        private GameObject targetObj;

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
            SetUpControls();
            SetReferences();
        }

        // private void OnEnable()
        // {
        //     SetUpControls();
        //     SetReferences();
        // }

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
            teleportStarted?.Invoke();
            yield return new WaitForSeconds(teleportStartUpDelay);
            var tgt = targetObject;
            Vector3 origin = new Vector3(transform.position.x, transform.position.y + heightOffset,
                transform.position.z);
            transform.position = tgt.transform.position;
            tgt.transform.position = origin;
            tgt.GetComponent<Rigidbody>().velocity = Vector3.zero;
            targetObj = null;
            objectSoundMaker.MakeSound(transform.position, noiseLevel);
            teleportTriggered?.Invoke();
            yield return new WaitForSeconds(teleportDelay);
            onCooldown = false;
            cooldownTimer = 0;
        }

        private bool CanTeleport()
        {
            Vector3 mousePosition = controls.InGame.MousePosition.ReadValue<Vector2>();

            ray = camera.ScreenPointToRay(mousePosition);

            //TODO make raycast bigger to allow for leway on teleport
            // bool isHit = Physics.SphereCast(ray, teleportAssistDistance, out hitInfo, teleportRange);
            bool isHit = Physics.Raycast(ray, out hitInfo, teleportRange, levelLayer);

            if (hitInfo.collider.CompareTag("ThrowableObjects"))
            {
                targetObj = hitInfo.collider.gameObject;
                // if (RoomForTeleport())
                // {
                    canTeleport = true;  
                // }
                // else
                // {
                //     canTeleport = false;
                // }
            }
            else
            {
                canTeleport = false;
            }

            return canTeleport;
        }

        private bool RoomForTeleport()
        {
            
            Collider[] colliders = Physics.OverlapBox(targetObj.transform.position, targetObj.transform.localScale / 2,
                Quaternion.identity);
            if (colliders.Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}