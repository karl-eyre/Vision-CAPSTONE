using UnityEngine;
using UnityEngine.InputSystem;

namespace InDevelopment.Alex
{
    public class PlayerRB : MonoBehaviour
    {

        [Tooltip("Vertical Rotation object (generally a neck)")]
        public Transform neck;
        [Tooltip("Horizontal rotation object (preferably the upper most parent of the player object this is on)")]
        public Transform body;
        public float sensitivity;
        public float speed;
        
        private Vector2 _mouseRotation;
        private Vector2 _mousePosition;
        private Vector3 _dir;
        private Vector3 _movement;
        public GameControls gameControls;
        public float maxAngle = 80;
        private Rigidbody RB;
        
        public enum mouseLock
        {
            Locked,
            Unlocked
        }

        public mouseLock mouselock = mouseLock.Locked;
        
        private void Awake()
        {
            MouseLock();
            gameControls = new GameControls();
            gameControls.InGame.Enable();
            gameControls.InGame.Movement.performed += Movement;
            gameControls.InGame.Movement.canceled += Movement;
            RB = gameObject.GetComponent<Rigidbody>();
        }


        void Movement(InputAction.CallbackContext context)
        {
            _dir = context.ReadValue<Vector2>();
            _dir = new Vector3(_dir.x, 0 ,_dir.y);
            //_dir = gameControls.InGame.Movement.ReadValue<Vector2>();
            var t = transform;
            //_movement = (_dir.y * t.forward) + (_dir.x * t.right);
            _movement = (_dir.y * RB.transform.right) + (_dir.x * RB.transform.forward);
            // RB.AddForce(_movement.normalized * speed);
        }

        void Move()
        {
            RB.AddForce(_movement.normalized * speed);
        }
        
        void MouseLook()
        {
           
            _mousePosition.x = (InputSystem.GetDevice<Mouse>().delta.x.ReadValue() * sensitivity) * Time.deltaTime;
            _mousePosition.y = (InputSystem.GetDevice<Mouse>().delta.y.ReadValue() * sensitivity) * Time.deltaTime;
			
            _mouseRotation.y = _mousePosition.x;

            Quaternion yQuat = Quaternion.AngleAxis(_mousePosition.y, -Vector3.right).normalized;
            Quaternion temp  = neck.rotation * yQuat;
			
            if (Quaternion.Angle(body.rotation, temp) < maxAngle)
            {
                neck.rotation = temp;
            }
			
            body.Rotate(0, _mouseRotation.y, 0);
            
        }

        private void MouseLock()
        {
            if (mouselock == mouseLock.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else if (mouselock == mouseLock.Unlocked)
            {
                Cursor.lockState = CursorLockMode.None;   
            }
        }

        private void FixedUpdate()
        {
            MouseLook();
            Move();
        }
    }
}