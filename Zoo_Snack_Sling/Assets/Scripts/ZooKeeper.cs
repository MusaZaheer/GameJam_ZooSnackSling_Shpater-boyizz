/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 


public class RandomMovement : MonoBehaviour 
{
    public Transform[] waypoints; 
    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

}*/
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ZookeeperPatrol : MonoBehaviour
{
    public Transform[] fixedWaypoints; 
    public Transform[] alertWaypoints; 
    public float normalSpeed = 3.5f;
    public float alertSpeedMultiplier = 2.5f;
    public int maxLives = 3;

    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private Coroutine alertCoroutine;
    private int currentLives;
    private bool isCooldown = false; 
    public Image[] hearts;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = normalSpeed;
        currentLives = maxLives;
        if (fixedWaypoints.Length > 0)
        {
            agent.SetDestination(fixedWaypoints[currentWaypointIndex].position);
        }
        UpdateHeartsUI();
    }

    void Update()
    {
        if (alertCoroutine == null && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % fixedWaypoints.Length;
            agent.SetDestination(fixedWaypoints[currentWaypointIndex].position);
        }
    }

    public void TriggerAlertByAnimalName(string animalName)
    {
        for (int i = 0; i < alertWaypoints.Length; i++)
        {
            if (alertWaypoints[i].tag == animalName)
            {
                if (alertCoroutine != null)
                {
                    StopCoroutine(alertCoroutine);
                }
                alertCoroutine = StartCoroutine(CheckAlertWaypoint(i));
                break;
            }
        }
    }

    private IEnumerator CheckAlertWaypoint(int waypointIndex)
    {
        agent.speed = normalSpeed * alertSpeedMultiplier;
        agent.SetDestination(alertWaypoints[waypointIndex].position);

        while (agent.remainingDistance > agent.stoppingDistance || agent.pathPending)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2);

        agent.speed = normalSpeed;
        alertCoroutine = null;
        agent.SetDestination(fixedWaypoints[currentWaypointIndex].position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("tester") && !isCooldown)
        {
            StartCoroutine(LoseLifeWithCooldown());
        }
    }

    private IEnumerator LoseLifeWithCooldown()
    {
        isCooldown = true;

        LoseLife();
        Debug.Log("Lost Life.");
        yield return new WaitForSeconds(1); 
        Debug.Log("After Cooldown.");
        isCooldown = false;
    }

    private void LoseLife()
    {
        if (currentLives >= 0)
        {
            currentLives--;
            Debug.Log("Remaining Lives: " + currentLives);
            UpdateHeartsUI();
            //Debug.Log("Remaining Lives: " + currentLives);
            if (currentLives <= 0)
            {
                Debug.Log("Game Over: Zookeeper ran out of lives.");
                Time.timeScale = 0; 
            }
        }
    }

    private void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < currentLives;
        }
    }
}
