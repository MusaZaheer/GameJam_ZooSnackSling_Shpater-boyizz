using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Eat : MonoBehaviour
{
    public string StuckObjectTag = "Dog"; // Let say thr food is a bone , or a meat, or a fish, or a dog snack....bhai kuch bhi smjh lo filhal k liye

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(StuckObjectTag))
        {
            // Set the Rigidbody to be kinematic so it doesn't move or affect the animal physically
            GetComponent<Rigidbody>().isKinematic = true;
            
            // Deactivate the food object
            gameObject.SetActive(false);

            Debug.Log("Mnay kha lia");
        }
    }
}
