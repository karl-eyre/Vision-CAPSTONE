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

        public bool isHit;
        public RaycastHit hit;
        private Ray ray;

        [Header("Throw Settings")]
        [SerializeField]
        private float PickupRange;
        public float defaultPickupRange = 5f;

        public LayerMask pickupObjectLayer;
        public LayerMask levelLayer;

        [SerializeField]
        private Material highlightMat;

        [SerializeField]
        private Material defaultMat;

        public GameObject selectedObject;

        private void Start()
        {
            SetUpControls();
            PickupRange = defaultPickupRange;
        }

        private void FixedUpdate()
        {
            HighlightObject();
        }

        private void SetUpControls()
        {
            controls = new GameControls();
            controls.Enable();
        }

        private void HighlightObject()
        {
            if (selectedObject != null)
            {
                selectedObject.GetComponent<VisionEffectActivation>().isSelected = false;
                selectedObject = null;
            }

            Vector3 mousePosition = controls.InGame.MousePosition.ReadValue<Vector2>();
            ray = camera.ScreenPointToRay(mousePosition);
            
            isHit = Physics.Raycast(ray, out hit, PickupRange);
            
            if (isHit)
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
                        selectionRenderer.material = highlightMat;
                    }

                    selectedObject = selection;
                }
                
                
            }
        }
    }
}