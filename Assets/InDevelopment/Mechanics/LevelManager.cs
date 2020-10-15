using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InDevelopment.Mechanics
{
    public class LevelManager : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.buildIndex + 1);
            }
        }
    }
}
