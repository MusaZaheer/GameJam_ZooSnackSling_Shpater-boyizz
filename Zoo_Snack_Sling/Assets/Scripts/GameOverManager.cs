// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class GameOverManager : MonoBehaviour
// {
//     public GameObject gameOverPanel;    // Reference to the Game Over panel
//     public GameObject levelCompletePanel; // Reference to the Level Complete panel

//     //private ZookeeperPatrol zookeeperPatrol;

//     void Start()
//     {
//         zookeeperPatrol = FindObjectOfType<ZookeeperPatrol>();  // Find the ZookeeperPatrol script in the scene
//     }

//     void Update()
//     {
//         CheckZookeeperLives();
//     }

//     void CheckZookeeperLives()
//     {
//         if (zookeeperPatrol != null && zookeeperPatrol.currentLives <= 0)
//         {
//             LevelLost();
//         }
//     }

//     private void LevelLost()
//     {
//         Debug.Log("Level Lost!");
//         AudioManager.instance.Play("GameOver");

//         // Deactivate the Level Complete panel if it's active
//         if (levelCompletePanel.activeSelf)
//         {
//             levelCompletePanel.SetActive(false);
//         }

//         // Activate the Game Over panel
//         gameOverPanel.SetActive(true);

//         // Pause the game
//         Time.timeScale = 0;
//     }
// }
