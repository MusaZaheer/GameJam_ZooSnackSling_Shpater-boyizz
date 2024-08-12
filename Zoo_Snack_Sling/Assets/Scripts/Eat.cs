using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Eat : MonoBehaviour
{
    public enum FoodType { Meat, Greenapple, Carrot ,Banana ,Fish ,Pear}
    public FoodType foodType;
    public string wrongAnimal; //

    // Tags for the respective animals
    private Dictionary<FoodType, string> foodToAnimalMap = new Dictionary<FoodType, string>
    {
        { FoodType.Meat, "Tiger" },
        { FoodType.Greenapple, "Deer" },
        { FoodType.Carrot, "Markhor" },
        { FoodType.Banana, "Monkey" },
        { FoodType.Fish, "Pinguin" },
        { FoodType.Pear, "tortoise" }
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
                AudioManager.instance.Play("Full");
            }
        } 
        //Moeez, here we will know about the wrong animal, you can use the wrongAnimal variable for ur implementation. For now, the var is public.
        else if(!other.gameObject.CompareTag("Untagged")){
            ZookeeperPatrol zookeeper = FindObjectOfType<ZookeeperPatrol>();
            wrongAnimal = other.gameObject.tag;
            Debug.Log("Wrong animal: " + wrongAnimal);
            PlayWrongSound(wrongAnimal);
            if (zookeeper != null)
            {
                AudioManager.instance.Play("triggered");
                zookeeper.TriggerAlertByAnimalName(wrongAnimal);
            }
        }
        else{
            AudioManager.instance.Play("Bounce");
        }
    }

    // Play full sound based on the animal's tag
    void PlayWrongSound(string animalTag)
    {
        switch (animalTag)
        {
            case "Deer":
                AudioManager.instance.Play("Deer");
                Debug.Log("Deer dont wanna eat this");
                break;
            case "Markhor":
                AudioManager.instance.Play("Markhor");
                break;
            case "Monkey":
                AudioManager.instance.Play("Monkey");
                break;
            case "Tiger":
                AudioManager.instance.Play("Tiger");
                break;
            case "Pinguin":
                AudioManager.instance.Play("Pinguin");
                break;
            case "tortoise":
                AudioManager.instance.Play("tortoise");
                break;
            default:
                //Debug.Log("No wrong sound found for this animal.");
                break;
        }
    }
}