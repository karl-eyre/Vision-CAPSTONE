using UnityEngine;

namespace InDevelopment.Mechanics.Player
{
    public class MouseLook : MonoBehaviour
    {
        private GameControls controls;
        private Vector2 look;
        private float xRotation;

        public UnityEngine.Events.UnityEvent playerTurnFast;

        [Header("Mouse Settings")]
        public float mouseSensitivity = 100f;

        [SerializeField]
        [Tooltip("The lower this is the higher you can look")]
        private float minUpLookAngle = -50f;

        [SerializeField]
        [Tooltip("The higher this is the lower you can look")]
        private float maxDownLookAngle = 60f;

        [Header("Player Body")]
        [SerializeField]
        private Transform playerBody;

        [Header("Debug Settings")]
        public bool mouseLocked;

        public bool InvertMouseY;

        private void SetUpControls()
        {
            controls = new GameControls();
            controls.Enable();
        }

        private void Start()
        {
            SetUpControls();
            if (mouseLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            if (playerBody == null)
            {
                playerBody = GetComponentInParent<Transform>();
            }
        }

        private void Update()
        {
            look = controls.InGame.MouseLook.ReadValue<Vector2>();

            var mouseX = look.x * mouseSensitivity * Time.deltaTime;
            var mouseY = look.y * mouseSensitivity * Time.deltaTime;

            if (mouseX < -18 || mouseX > 18)
            {
                playerTurnFast.Invoke(); 
            }
            
            if (InvertMouseY)
            {
                xRotation += mouseY;
            }
            else
            {
                xRotation -= mouseY;
            }

            xRotation = Mathf.Clamp(xRotation, minUpLookAngle, maxDownLookAngle);

            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            playerBody.Rotate(Vector3.up * mouseX);
        }


        private void OnEnable()
        {
            if (controls != null) controls.Enable();
        }

        private void OnDisable()
        {
            if (controls != null) controls.Disable();
        }
    }
}