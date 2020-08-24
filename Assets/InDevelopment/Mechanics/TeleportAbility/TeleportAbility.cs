using System;
using InDevelopment.Mechanics.ObjectDistraction;
using UnityEditor;
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
        public LayerMask teleportLayer;

        private bool canTeleport;
        private GameObject targetObj;

        [SerializeField]
        private float heightOffset;

        private RaycastHit hitInfo;
        private Ray ray;

        [SerializeField]
        private GeneralSoundMaker generalSoundMaker;

        [HideInInspector]
        public float noiseLevel;

        public LayerMask unphaseableLayer;
        public LayerMask phaseableLayer;

        private void Start()
        {
            SetUpControls();
            SetReferences();
        }

        private void SetReferences()
        {
            generalSoundMaker = GetComponentInChildren<GeneralSoundMaker>();
        }

        private void SetUpControls()
        {
            controls = new GameControls();
            controls.Enable();
            controls.InGame.TeleportToItem.performed += ReadTeleportInput;
        }

        private void ReadTeleportInput(InputAction.CallbackContext obj)
        {
            if (CanTeleport())
            {
                TeleportToPosition(targetObj);
            }
        }

        private void TeleportToPosition(GameObject targetObject)
        {
            var tgt = targetObject;
            //TODO add height offset to other object
            Vector3 origin = new Vector3(transform.position.x, transform.position.y + heightOffset,
                transform.position.z);
            transform.position = tgt.transform.position;
            tgt.transform.position = origin;
            tgt.GetComponent<Rigidbody>().velocity = Vector3.zero;
            targetObj = null;
            generalSoundMaker.MakeSound(noiseLevel);
        }

        private bool CanTeleport()
        {
            //add check for if raycast is valid

            Vector3 mousePosition = controls.InGame.MousePosition.ReadValue<Vector2>();

            ray = camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out hitInfo, teleportRange, unphaseableLayer))
            {
                canTeleport = false;
            }
            else if (Physics.Raycast(ray, out hitInfo, teleportRange, phaseableLayer))
            {
                if (Physics.Raycast(ray, out hitInfo, teleportRange, teleportLayer))
                {
                    targetObj = hitInfo.collider.gameObject;
                    canTeleport = true;
                }
                else
                {
                    canTeleport = false;
                }
            }

            //this is just the part the "picks up" the objects in the level
            //to save on performance, the object that is raycast to if it is a throwable object then turn it off and "add" it
            //to the players hand, however instead it simply turns it off 

            return canTeleport;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawRay(camera.transform.position, ray.direction * teleportRange);
        }
    }
}