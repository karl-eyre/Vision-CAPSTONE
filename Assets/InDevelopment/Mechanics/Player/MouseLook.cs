using UnityEngine;

namespace Player
{
    public class MouseLook : MonoBehaviour
    {
        private GameControls controls;
        private Vector2 look;
        private float xRotation;

        [Header("Mouse Settings")]
        [SerializeField]
        private float mouseSensitivity = 100f;


        [Header("Debug Settings")]
        public bool mouseLocked;


        [SerializeField]
        private float minLookAngle = -50f;

        [SerializeField]
        private float maxLookAngle = 50f;

        [Header("Player Body")]
        [SerializeField]
        private Transform playerBody;

        public bool InvertMouseY;

        private void SetUpControls()
        {
            controls = new GameControls();
            controls.Enable();
        }

        private void Start()
        {
            SetUpControls();
            Cursor.lockState = CursorLockMode.Locked;

            if (playerBody == null)
            {
                playerBody = GetComponentInParent<Transform>();
            }
        }

        private void Update()
        {
            look = controls.InGame.MouseLook.ReadValue<Vector2>();

            var MouseX = look.x * mouseSensitivity * Time.deltaTime;
            var MouseY = look.y * mouseSensitivity * Time.deltaTime;

            if (InvertMouseY)
            {
                xRotation += MouseY;
            }
            else
            {
                xRotation -= MouseY;
            }

            xRotation = Mathf.Clamp(xRotation, minLookAngle, maxLookAngle);

            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            playerBody.Rotate(Vector3.up * MouseX);
        }


        private void OnEnable()
        {
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }
    }
}