using UnityEngine;
using VisionAbility;

namespace Enemy
{
    public class VisionEffectActivation : MonoBehaviour
    {
        private bool visionActivated;

        [Header("Vision Effector Settings")]
        public Material visionMat;

        public Material defaultMat;

        // public bool useCustomRenderer;

        public void Start()
        {
            VisionAbilityController.visionActivation += UpdateVision;
            GetReferences();
            // if (useCustomRenderer)
            // {
            //     VisionAbilityController.visionActivation += ChangeLayer;
            // }
        }

        private void GetReferences()
        {
            if (defaultMat == null)
            {
                defaultMat = GetComponent<Renderer>().material;
            }
        }

        private void Update()
        {
            // && !useCustomRenderer
            if (visionActivated)
            {
                GetComponent<Renderer>().material = visionMat;
            }
            else
            {
                GetComponent<Renderer>().material = defaultMat;
            }
        }

        public void UpdateVision()
        {
            visionActivated = !visionActivated;
        }

        public void ChangeLayer()
        {
            //have to use layer numbers 
            //for some reason it errors out when trying to use a public layer mask,
            //even if you want to pass it the int value the thing wants
            if (visionActivated)
            {
                gameObject.layer = 10;
            }
            else
            {
                gameObject.layer = 9;
                // LayerMask.NameToLayer(enemyFrontLayer.ToString());
            }
        }
    }
}