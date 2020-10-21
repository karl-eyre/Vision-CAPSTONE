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
        public float PickupRange;

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
            // levelLayer = ~pickupObjectLayer;
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
            
            //TODO ALEX ITS HERE have it checked out as to why it doesn't work
            if (Physics.Raycast(ray, out hit, PickupRange, levelLayer))
            {
                return;
            }
            
            isHit = Physics.Raycast(ray, out hit, PickupRange, pickupObjectLayer);

            if (isHit)
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