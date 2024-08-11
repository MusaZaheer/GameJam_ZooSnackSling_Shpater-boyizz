using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalHunger : MonoBehaviour
{
    public int maxHunger = 3;   // Maximum hunger value (e.g., full after eating twice)
    public int currentHunger;   // Initialize currentHunger to 0
    public Slider hungerBar;    // Reference to the hunger bar UI Slider
    public bool full;

    void Start()
    {
        hungerBar.maxValue = maxHunger;     // Set max value of hungerBar
        hungerBar.value = currentHunger;    // Set initial value of hungerBar
        full = false;                       // Initialize full flag to false
        UpdateHungerBar();
    }

    public void Eat()
    {
        if (!full)  // Check if the animal is not full
        {
            currentHunger++;  // Increment currentHunger
            if (currentHunger >= maxHunger)
            {
                full = true;  // Set full flag to true if currentHunger reaches maxHunger
            }
            Debug.Log("Current Hunger: " + currentHunger);
            UpdateHungerBar();  // Update hunger bar
            AudioManager.instance.Play("Munch");  // Play the "Eat" sound
        }
        else
        {
            //Debug.Log("Animal is already full.");
            //PlayFullSound();  // Play the "Full" sound
        }
    }

    void UpdateHungerBar()
    {
        hungerBar.value = currentHunger;  // Set the hunger bar value to currentHunger
    }

    
}