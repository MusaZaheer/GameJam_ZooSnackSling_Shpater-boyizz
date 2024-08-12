using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletionManager : MonoBehaviour
{
    public GameObject levelCompletePanel;  // Reference to the Level Complete panel

    private AnimalHunger[] animalHungerScripts;

    void Start()
    {
        // Find all AnimalHunger scripts in the scene
        animalHungerScripts = FindObjectsOfType<AnimalHunger>();
    }

    void Update()
    {
        CheckAllAnimalsFull();
    }

    void CheckAllAnimalsFull()
    {
        foreach (AnimalHunger animalHunger in animalHungerScripts)
        {
            if (!animalHunger.full)  // If any animal is not full, return early
            {
                return;
            }
        }

        // If all animals are full, trigger level completion
        LevelWon();
    }

    private void LevelWon()
    {
        Debug.Log("Level Won!");
        AudioManager.instance.Play("LevelComplete");

        // Set the Level Complete panel active
        levelCompletePanel.SetActive(true);

        // Pause the game
        Time.timeScale = 0;
    }
}
