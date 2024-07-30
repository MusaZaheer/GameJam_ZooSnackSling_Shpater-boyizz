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

public class ZookeeperPatrol : MonoBehaviour
{
    public Transform[] fixedWaypoints; 
    public Transform[] alertWaypoints; 
    public float normalSpeed = 3.5f;
    public float alertSpeedMultiplier = 2.5f;

    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private Coroutine alertCoroutine;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = normalSpeed;
        if (fixedWaypoints.Length > 0)
        {
            agent.SetDestination(fixedWaypoints[currentWaypointIndex].position);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TriggerAlert(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TriggerAlert(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TriggerAlert(2);
        }

        if (alertCoroutine == null && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % fixedWaypoints.Length;
            agent.SetDestination(fixedWaypoints[currentWaypointIndex].position);
        }
    }

    public void TriggerAlert(int waypointIndex)
    {
        if (waypointIndex >= 0 && waypointIndex < alertWaypoints.Length)
        {
            if (alertCoroutine != null)
            {
                StopCoroutine(alertCoroutine);
            }
            alertCoroutine = StartCoroutine(CheckAlertWaypoint(waypointIndex));
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
        if (other.CompareTag("Food"))
        {
            
            Debug.Log("Game Over: Zookeeper found food.");
           
            Time.timeScale = 0; 
        }
    }
}
