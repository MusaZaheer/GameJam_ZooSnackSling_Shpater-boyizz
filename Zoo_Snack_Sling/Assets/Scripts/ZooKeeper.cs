using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 


public class RandomMovement : MonoBehaviour 
{
    /*public NavMeshAgent agent;
    public float walkRadius = 10f;
    public float waitTime = 3f;
    
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = waitTime;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= waitTime)
        {
            Vector3 newPos = RandomNavSphere(transform.position, walkRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;
        randomDirection += origin;
        
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, dist, layermask);

        return navHit.position;
    }*/

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

}
