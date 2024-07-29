using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalHunger : MonoBehaviour
{
    public int maxHunger = 3; // Maximum hunger value (e.g., full after eating twice)
    private int currentHunger;
    public Slider hungerBar; // Reference to the hunger bar UI Slider
    public bool full;
    void Start()
    {
        currentHunger = 0;
        full = false;
        UpdateHungerBar();
    }

    public void Eat()
    {
        if (currentHunger < maxHunger)
        {
            currentHunger++;

            Debug.Log("Current Hunger: " + currentHunger);
            UpdateHungerBar();
        }
        else if (currentHunger == maxHunger)
        {
            full = true;
        }
    }

    void UpdateHungerBar()
    {
        // float normalizedHunger = Mathf.Clamp01((float)currentHunger / maxHunger);
        // Debug.Log("Normalized Hunger: " + normalizedHunger);
        // hungerBar.value = normalizedHunger;
        hungerBar.value++;
    }
}