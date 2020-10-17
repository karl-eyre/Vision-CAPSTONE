using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;


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
        
        public void Escape(InputAction.CallbackContext obj)
        {
            if(SceneManager.GetActiveScene().name == "MainMenu")
            {
                ExitGame();
            }
            else
            {
                MainMenu();
            }
        }

        public void ExitGame()
        {        
            Application.Quit();
            Debug.Log("We Finished");
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void Credits()
        {
            SceneManager.LoadScene("Credits");
        }

        public void Settings()
        {
            SceneManager.LoadScene("Settings");
        }

        public void StartGame()
        {
            SceneManager.LoadScene("1_TutorialRooms");
        }
    }   
}
