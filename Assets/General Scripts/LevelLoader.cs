using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    /// <summary>
    /// Loads a scene by given name
    /// </summary>
    /// <param name="levelName"></param> the level's name
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    /// <summary>
    /// exits the game
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
