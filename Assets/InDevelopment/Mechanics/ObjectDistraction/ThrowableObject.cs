using UnityEngine;

namespace InDevelopment.Mechanics.ObjectDistraction
{
    /// <summary>
    /// will be used when it has to make noise
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class ThrowableObject : MonoBehaviour
    {
        public LayerMask objectLayer;
        
        private void Start()
        {
            //this is just to make sure that any item that this script is
            //attached to worked without worrying about having to change the layer manually
            //Problem may arise if layer orders are changed
            gameObject.layer = 11;
        }
        
        private void GetPickedUp()
        {
            
        }
    }
}
