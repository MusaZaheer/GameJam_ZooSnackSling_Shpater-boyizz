using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Eat : MonoBehaviour
{
    public enum FoodType { Meat, Greenapple, Apple ,Banana}
    public FoodType foodType;
    public string wrongAnimal;

    // Tags for the respective animals
    private Dictionary<FoodType, string> foodToAnimalMap = new Dictionary<FoodType, string>
    {
        { FoodType.Meat, "Tiger" },
        { FoodType.Greenapple, "Deer" },
        { FoodType.Apple, "Markhor" },
        { FoodType.Banana, "Monkey" }
    };

    private void OnTriggerEnter(Collider other)
    {
        string targetAnimalTag = foodToAnimalMap[foodType];

        if (other.gameObject.CompareTag(targetAnimalTag))
        {
            // Set the Rigidbody to be kinematic so it doesn't move or affect the animal physically
            GetComponent<Rigidbody>().isKinematic = true;

            // Find the AnimalHunger component and update its hunger
            AnimalHunger animalHunger = other.gameObject.GetComponent<AnimalHunger>();
            if (animalHunger != null && !animalHunger.full)
            {
                animalHunger.Eat();
                // Deactivate the food object
                gameObject.SetActive(false);
                Debug.Log("Eaten by " + targetAnimalTag);
            }
            else if (animalHunger.full)
            {
                Debug.Log("Animal is full");
            }
        } 
        //Moeez, here we will know about the wrong animal, you can use the wrongAnimal variable for ur implementation. For now, the var is public.
        else if(!other.gameObject.CompareTag("Untagged")){
            ZookeeperPatrol zookeeper = FindObjectOfType<ZookeeperPatrol>();
            wrongAnimal = other.gameObject.tag;
            Debug.Log("Wrong animal: " + wrongAnimal);
            if (zookeeper != null)
            {
                zookeeper.TriggerAlertByAnimalName(wrongAnimal);
            }
        }
    }
}