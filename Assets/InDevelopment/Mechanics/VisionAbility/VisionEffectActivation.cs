using InDevelopment.Mechanics.Player;
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

        private GameObject player;
        private Renderer rendererMat;
        public float fadeDistance;

        // public bool useCustomRenderer;

        public void Start()
        {
            rendererMat = GetComponent<Renderer>();
            fadeDistance = 30f;
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
                    // rendererMat.material = visionMat;
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
            //TODO add in extra ifs for different levels
            if (Vector3.Distance(transform.position, player.transform.position) > fadeDistance)
            {
                rendererMat.material.color = new Color(defaultMat.color.r,defaultMat.color.g,defaultMat.color.b,0);
            }
            else
            {
                rendererMat.material = visionMat;
            }
        }
    }
}