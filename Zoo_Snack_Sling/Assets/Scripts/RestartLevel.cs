using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    public void Restart()
    {
        EventManager.ClearAllEvents(); // Unsubscribe all events
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
