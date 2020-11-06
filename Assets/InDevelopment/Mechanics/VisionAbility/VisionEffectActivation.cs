using UnityEngine;

namespace InDevelopment.Mechanics.VisionAbility
{
    public class VisionEffectActivation : MonoBehaviour
    {
        private bool visionActivated;

        [Header("Vision Effector Settings")]
        public Material visionMat;

        public Material defaultMat;
        public bool isSelected;

        // public bool useCustomRenderer;

        public void Start()
        {
            VisionAbilityController.visionActivation += UpdateVision;
            GetReferences();
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
            if (!isSelected)
            {
                if (visionActivated)
                {
                    GetComponent<Renderer>().material = visionMat;
                }
                else
                {
                    GetComponent<Renderer>().material = defaultMat;
                }
            }
        }

        public void UpdateVision()
        {
            visionActivated = !visionActivated;
        }
    }
}