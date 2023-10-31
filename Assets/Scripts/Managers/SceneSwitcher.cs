using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
  
    public void Play()
    {
        // Load the play scene
        SceneManager.LoadScene("Play");
    }

    public void GameOver()
    {
        // Load the game over scene
        SceneManager.LoadScene("End");
    }
    public void Quit()
    {
        // Quit the game
        Application.Quit();
    }




}
