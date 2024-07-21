using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotReload : MonoBehaviour
{
    public GameObject foodPrefab; // The food GameObject to spawn
    public int numberOfFood = 10; // Number of food objects to create

    private List<GameObject> foodPool;
    private GameObject currentFood; // Keep track of the current active food

    void Start()
    {
        foodPool = new List<GameObject>();

        for (int i = 0; i < numberOfFood; i++)
        {
            GameObject food = Instantiate(foodPrefab, transform.position, Quaternion.identity, transform);
            food.SetActive(false);
            foodPool.Add(food);
        }

        RespawnProjectile(); // Spawn the first food at the start
    }

    public void RespawnProjectile()
    {
        if (currentFood != null)
        {
            currentFood.SetActive(false); // Deactivate the current food
        }

        foreach (GameObject food in foodPool)
        {
            if (!food.activeInHierarchy)
            {
                food.transform.position = transform.position;
                food.SetActive(true);
                currentFood = food; // Set the new food as the current active food
                currentFood.GetComponent<Rigidbody>().isKinematic = true; // Ensure the food is kinematic
                break;
            }
        }
    }
}
