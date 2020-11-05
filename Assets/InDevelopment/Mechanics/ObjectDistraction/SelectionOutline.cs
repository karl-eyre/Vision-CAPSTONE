using System;
using InDevelopment.Mechanics.VisionAbility;
using UnityEngine;

namespace InDevelopment.Mechanics.ObjectDistraction
{
    public class SelectionOutline : MonoBehaviour
    {
        private GameControls controls;

        [SerializeField]
        private Camera camera;

        public bool pickupIsHit;
        public RaycastHit hit;
        private Ray ray;
        private TeleportAbility.TeleportAbility teleportAbility;

        [Header("Throw Settings")]
        [SerializeField]
        private float PickupRange;

        private float teleportRange;

        private bool pickupSelected;

        [SerializeField]
        private Material pickupHighlightMat;

        [SerializeField]
        private Material teleportHighlightMat;

        [SerializeField]
        private Material defaultMat;

        public GameObject selectedObject;
        private bool teleportIsHit;
        private GameObject teleportObject;

        private void Start()
        {
            SetUpControls();
            teleportAbility = GetComponent<TeleportAbility.TeleportAbility>();
            teleportRange = teleportAbility.teleportRange;
        }

        private void FixedUpdate()
        {
            HighlightPickupObject();
            if (!pickupSelected)
            {
                HighlightTeleportObjects();
            }
        }

        private void SetUpControls()
        {
            controls = new GameControls();
            controls.Enable();
        }

        private void HighlightTeleportObjects()
        {
            if (teleportObject != null)
            {
                teleportObject.GetComponent<VisionEffectActivation>().isSelected = false;
                teleportObject = null;
            }

            Vector3 mousePosition = controls.InGame.MousePosition.ReadValue<Vector2>();
            ray = camera.ScreenPointToRay(mousePosition);

            teleportIsHit = Physics.Raycast(ray, out hit, teleportRange, teleportAbility.levelLayer);

            if (teleportIsHit)
            {
                if (!hit.collider.CompareTag("ThrowableObjects"))
                {
                    
                    // return;
                }
                else
                {
                    var selection = hit.collider.gameObject;
                    selection.GetComponent<VisionEffectActivation>().isSelected = true;
                    var selectionRenderer = hit.collider.gameObject.GetComponent<Renderer>();
                    if (selectionRenderer != null)
                    {
                        selectionRenderer.material = teleportHighlightMat;
                        pickupSelected = false;
                    }
                    teleportObject = selection;
                }
            }
        }

        private void HighlightPickupObject()
        {
            if (selectedObject != null)
            {
                selectedObject.GetComponent<VisionEffectActivation>().isSelected = false;
                selectedObject = null;
            }

            Vector3 mousePosition = controls.InGame.MousePosition.ReadValue<Vector2>();
            ray = camera.ScreenPointToRay(mousePosition);

            pickupIsHit = Physics.Raycast(ray, out hit, PickupRange);

            if (pickupIsHit)
            {
                if (!hit.collider.CompareTag("ThrowableObjects"))
                {
                    pickupSelected = false;
                    // return;
                }
                else
                {
                    var selection = hit.collider.gameObject;
                    selection.GetComponent<VisionEffectActivation>().isSelected = true;
                    var selectionRenderer = hit.collider.gameObject.GetComponent<Renderer>();
                    if (selectionRenderer != null)
                    {
                        selectionRenderer.material = pickupHighlightMat;
                        pickupSelected = true;
                    }

                    selectedObject = selection;
                }
            }
            else
            {
                pickupSelected = false;
            }
        }
    }
}