using UnityEngine;
using VisionAbility;

namespace Enemy
{
    public class EnemyVisionActivation : MonoBehaviour
    {
        public bool visionActivated;
        public Material visionMat;
        public Material defaultMat;
        
        public void Start()
        {
            VisionAbilityController.visionActivation += UpdateVision;
        }
        
        
        private void Update()
        {
            if(visionActivated)
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
    }
}
