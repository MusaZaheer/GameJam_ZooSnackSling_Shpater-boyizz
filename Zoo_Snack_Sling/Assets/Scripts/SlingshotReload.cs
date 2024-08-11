using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class FoodPool
{
    public GameObject foodPrefab;
    public int numberOfFood = 1;
    public enum FoodType { Meat, Greenapple, Carrot, Banana, Fish, Pear }
    public FoodType foodType;
}

public class SlingshotReload : MonoBehaviour
{
    public static SlingshotReload mypool;   // making obj of this calss to be used in any other script
    public List<FoodPool> foodPool;         // all public data from user saved in this onj
    //public GameObject Bananaprefab;
    //public GameObject Steakprefab;
    //public GameObject Appleprefab;
    //public GameObject Carrotprefab;
    private List<GameObject> pooledfood;     // food currently present in the pool
    private List<GameObject> newPooledfood;
    private GameObject currentFood;          // Keep track of the current active food
    //private int destroyed = 0;               // Number of foods destroyed 
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Steak Selected");
            SpawnSpecificFood(SpecificFoodIndex(PrefabToNameConverter(FoodPool.FoodType.Meat)));
            //RefereshPool(SpecificFoodIndex(PrefabToNameConverter(FoodPool.FoodType.Meat)));
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Banana Selected");
            SpawnSpecificFood(SpecificFoodIndex(PrefabToNameConverter(FoodPool.FoodType.Banana)));
            //RefereshPool(SpecificFoodIndex(PrefabToNameConverter(FoodPool.FoodType.Banana)));
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Apple Selected");
            SpawnSpecificFood(SpecificFoodIndex(PrefabToNameConverter(FoodPool.FoodType.Greenapple)));
            //RefereshPool(SpecificFoodIndex(PrefabToNameConverter(FoodPool.FoodType.Greenapple)));
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Carrot Selected");
            SpawnSpecificFood(SpecificFoodIndex(PrefabToNameConverter(FoodPool.FoodType.Carrot)));
            //RefereshPool(SpecificFoodIndex(PrefabToNameConverter(FoodPool.FoodType.Carrot)));
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Carrot Selected");
            SpawnSpecificFood(SpecificFoodIndex(PrefabToNameConverter(FoodPool.FoodType.Pear)));
            //RefereshPool(SpecificFoodIndex(PrefabToNameConverter(FoodPool.FoodType.Carrot)));
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Carrot Selected");
            SpawnSpecificFood(SpecificFoodIndex(PrefabToNameConverter(FoodPool.FoodType.Fish)));
            //RefereshPool(SpecificFoodIndex(PrefabToNameConverter(FoodPool.FoodType.Carrot)));
        }

    }
    public string PrefabToNameConverter(FoodPool.FoodType foodtype)
    {
        if(foodtype == FoodPool.FoodType.Greenapple)
        {
            return "apple_001(Clone)";
        }
        else if (foodtype == FoodPool.FoodType.Banana)
        {
            return "banana_001(Clone)";
        }
        else if (foodtype == FoodPool.FoodType.Meat)
        {
            return "Steak(Clone)";
        }
        else if (foodtype == FoodPool.FoodType.Carrot)
        {
            return "carrot_001(Clone)";
        }
        else if (foodtype == FoodPool.FoodType.Fish)
        {
            return "fish_001(Clone)";
        }
        else if (foodtype == FoodPool.FoodType.Pear)
        {
            return "Pear(Clone)";
        }
        else { return "Unknown"; }
    }
    public void DeleteCurrentFood() { Destroy(currentFood);}
    public  int SpecificFoodIndex(string foodname) 
    {
        int foodIndex = 0;
        for (int i = 0; i < pooledfood.Count; i++)
        {
            if (pooledfood[i].name == foodname ) { break; }
            if (pooledfood[i].name != foodname)
            {
                foodIndex += 1;
            }
        }
        Debug.Log("FoodIndex is "+ foodIndex);
        return foodIndex;
    }
    public void RefereshPool(int foodIndex)
    {
        newPooledfood = new List<GameObject>();

        // Add elements up to the specific food index (skipping the one at foodIndex)
        for (int i = 0; i < pooledfood.Count; i++)
        {
            if (i != foodIndex)
            {
                newPooledfood.Add(pooledfood[i]);
            }
        }

        // Replace the old pool with the new one
        pooledfood = newPooledfood;
        Debug.Log("Pool Refreshed");
    }


    public void SpawnSpecificFood(int foodIndex)
    {
        if (currentFood != null)
        {
            currentFood.SetActive(false); // Deactivate the current food
            Debug.Log("In SpawnSpecificFood: Current food deactivated");
        }


        bool foodFound = false; // Flag to check if the specific food is found in the pool

        pooledfood[foodIndex].transform.position = transform.position; // Move to slingshot position
        pooledfood[foodIndex].SetActive(true); // Activate the specific food
        currentFood = pooledfood[foodIndex]; // Set as the current active food
        currentFood.GetComponent<Rigidbody>().isKinematic = true; // Ensure it's kinematic
        Debug.Log("In SpawnSpecificFood: Specific food activated");
        
        foodFound = true;

        //for (int i = 0; i < pooledfood.Count; i++)
        //{
        //    Debug.Log("Checking pooled food: " + pooledfood[i].name + " (active: " + pooledfood[i].activeInHierarchy + ")");

        //    if (pooledfood[i].gameObject == foodPrefab && !pooledfood[i].activeInHierarchy)
        //    {
        //        pooledfood[i].transform.position = transform.position; // Move to slingshot position
        //        pooledfood[i].SetActive(true); // Activate the specific food
        //        currentFood = pooledfood[i]; // Set as the current active food
        //        currentFood.GetComponent<Rigidbody>().isKinematic = true; // Ensure it's kinematic
        //        Debug.Log("In SpawnSpecificFood: Specific food activated");
        //        foodFound = true;
        //        break;
        //    }
        //}

        if (!foodFound)
        {
            Debug.LogError("In SpawnSpecificFood: Specific food not found in the pool!");
        }
    }
    public void RespawnProjectile()
    {
        int currentFoodIndex = 0;

        //Debug.Log("Respawn Called");
        if (currentFood != null)
        {
            currentFoodIndex = SpecificFoodIndex(currentFood.name);
            DeleteCurrentFood();
            RefereshPool(currentFoodIndex);
        }
        
        for (int i=currentFoodIndex ; i<pooledfood.Count; i++)
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
