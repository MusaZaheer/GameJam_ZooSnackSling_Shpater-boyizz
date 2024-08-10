using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelTransition : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
