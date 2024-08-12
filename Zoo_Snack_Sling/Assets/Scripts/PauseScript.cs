using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    bool paused = false;

    public void pausegame()
    {
        if (paused)
        {
            Time.timeScale = 1;
            paused = false;
        }
        else
        {
            Time.timeScale = 0;
            paused = true;
        }
    }
    public void Resume()
    {
        if (paused)
        {
            Time.timeScale = 1;
            paused = false;
        }
    }
}
