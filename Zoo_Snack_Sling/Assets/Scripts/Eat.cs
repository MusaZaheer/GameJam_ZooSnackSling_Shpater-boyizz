using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
//destroy the food
[RequireComponent(typeof(GameObject))]
public class Eat : MonoBehaviour
{

    public string StuckObjectTag = "Food";
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(StuckObjectTag))
        {
            //Kinematic on so it doesn't move or affect the animal physically
            GetComponent<Rigidbody>().isKinematic = true;
            //dmoving the food to CANADA LOL
            other.gameObject.transform.position = new Vector3(1000, 1000, 1000);
        }
    }
    
}
