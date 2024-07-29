using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Eat : MonoBehaviour
{
    public enum FoodType { Meat, Leafs, Grass }
    public FoodType foodType;

    // Tags for the respective animals
    private Dictionary<FoodType, string> foodToAnimalMap = new Dictionary<FoodType, string>
    {
        { FoodType.Meat, "Tiger" },
        { FoodType.Leafs, "Deer" },
        { FoodType.Grass, "Markhor" }
    };

    private void OnCollisionEnter(Collision other)
    {
        string targetAnimalTag = foodToAnimalMap[foodType];

        if (other.gameObject.CompareTag(targetAnimalTag))
        {
            // Set the Rigidbody to be kinematic so it doesn't move or affect the animal physically
            GetComponent<Rigidbody>().isKinematic = true;
            
            // Find the AnimalHunger component and update its hunger
            AnimalHunger animalHunger = other.gameObject.GetComponent<AnimalHunger>();
            if (animalHunger != null && animalHunger.full == false)
            {
                animalHunger.Eat();
                // Deactivate the food object
                gameObject.SetActive(false);
                Debug.Log("Eaten by " + targetAnimalTag);
            }
            else if(animalHunger.full == true)
            {
                Debug.Log("Animal is full");
            }
            else{
                Debug.Log("Wrong animal");
            }

            
        }
    }
}
