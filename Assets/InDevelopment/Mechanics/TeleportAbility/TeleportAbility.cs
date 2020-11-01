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
        public float teleportStartUpDelay;
        public LayerMask teleportLayer;

        private bool canTeleport;
        private GameObject targetObj;

        [SerializeField]
        private float heightOffset = 0.5f;

        private RaycastHit hitInfo;
        private Ray ray;

        private ObjectSoundMaker objectSoundMaker;

        public float noiseLevel;

        public LayerMask unphaseableLayer;
        public LayerMask phaseableLayer;
        private bool onCooldown;
        public float teleportAssistDistance = 2f;

        public static event Action teleportTrigger;

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
            teleportLayer = LayerMask.GetMask("ThrowableObjects");
            phaseableLayer = LayerMask.GetMask("Phaseable");
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

        private IEnumerator Teleport(GameObject targetObject)
        {
            //todo add delay to teleport
            yield return new WaitForSeconds(teleportStartUpDelay);
            var tgt = targetObject;
            Vector3 origin = new Vector3(transform.position.x, transform.position.y + heightOffset,
                transform.position.z);
            transform.position = tgt.transform.position;
            tgt.transform.position = origin;
            tgt.GetComponent<Rigidbody>().velocity = Vector3.zero;
            targetObj = null;
            objectSoundMaker.MakeSound(transform.position, noiseLevel);
            teleportTrigger?.Invoke();
            yield return new WaitForSeconds(teleportDelay);
            onCooldown = false;
        }

        private bool CanTeleport()
        {
            Vector3 mousePosition = controls.InGame.MousePosition.ReadValue<Vector2>();

            ray = camera.ScreenPointToRay(mousePosition);

            //todo look at sphere cast for little leeway on the area u need to aim at

            // bool isHit = Physics.SphereCast(ray, teleportAssistDistance, out hitInfo, teleportRange);
            bool isHit = Physics.Raycast(ray, out hitInfo, teleportRange);

            if (!hitInfo.collider.CompareTag("ThrowableObjects"))
            {
                canTeleport = false;
            }
            else
            {
                targetObj = hitInfo.collider.gameObject;
                canTeleport = true;
            }

            return canTeleport;
        }
    }
}