using InDevelopment.Mechanics.Player;
using UnityEngine;

namespace InDevelopment.Mechanics.VisionAbility
{
    public class VisionEffectActivation : MonoBehaviour
    {
        private bool visionActivated;

        [Header("Vision Effector Settings")]
        public Material closeVisionMat;

        public Material middleVisionMat;
        public Material farVisionMat;
        public Material defaultMat;
        public bool isSelected;

        private GameObject player;
        private Renderer rendererMat;
        public float maxFadeDistance;
        public float minFadeDistance;

        // public bool useCustomRenderer;

        public void Start()
        {
            rendererMat = GetComponent<Renderer>();
            // maxFadeDistance = 50f;
            // minFadeDistance = 25f;
            VisionAbilityController.visionActivation += UpdateVision;
            GetReferences();
        }

        private void GetReferences()
        {
            if (defaultMat == null)
            {
                defaultMat = GetComponent<Renderer>().material;
            }

            if (player == null)
            {
                player = FindObjectOfType<PlayerController>().gameObject;
            }
        }

        private void Update()
        {
            if (!isSelected)
            {
                if (visionActivated)
                {
                    UpdateAlpha();
                }
                else
                {
                    rendererMat.material = defaultMat;
                }
            }
        }

        private void UpdateVision()
        {
            visionActivated = !visionActivated;
        }

        private void UpdateAlpha()
        {
            if (Vector3.Distance(transform.position, player.transform.position) > maxFadeDistance)
            {
                rendererMat.material = farVisionMat;
            }
            else if (Vector3.Distance(transform.position, player.transform.position) > minFadeDistance && Vector3.Distance(transform.position, player.transform.position) < maxFadeDistance)
            {
                rendererMat.material = middleVisionMat;
            }
            else
            {
                rendererMat.material = closeVisionMat;
            }
        }
    }
}