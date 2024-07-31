using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FoodPool
{
    public GameObject foodPrefab;
    public int numberOfFood = 1;
}

public class SlingshotReload : MonoBehaviour
{
    public static SlingshotReload mypool;   // making obj of this calss to be used in any other script
    public List<FoodPool> foodPool;         // all public data from user saved in this onj
    private List<GameObject> pooledfood;     // food currently present in the pool
    private GameObject currentFood;          // Keep track of the current active food
    private int destroyed = 0;               // Number of foods destroyed 
    private void Awake()
    {
        mypool = this;
    }

    void Start()
    {
        pooledfood = new List<GameObject>();

        foreach (FoodPool food in foodPool)
        {
            for (int i = 0; i < food.numberOfFood; i++)
            {
                GameObject obj = Instantiate(food.foodPrefab, transform.position, Quaternion.identity, transform);
                obj.SetActive(false);
                pooledfood.Add(obj);
            }
        }

        RespawnProjectile(); // Spawn the first food at the start
    }

    public void RespawnProjectile()
    {
        //Debug.Log("Respawn Called");
        if (currentFood != null)
        {
            Destroy(currentFood); // Deactivate the current food
            destroyed++;
        }

        for (int i=destroyed ; i<pooledfood.Count; i++)
        {
            if (!pooledfood[i].activeInHierarchy)
            {
                pooledfood[i].transform.position = transform.position;
                pooledfood[i].SetActive(true);
                currentFood = pooledfood[i]; // Set the new food as the current active food
                currentFood.GetComponent<Rigidbody>().isKinematic = true; // Ensure the food is kinematic
                break;
            }
        }

    }
}
