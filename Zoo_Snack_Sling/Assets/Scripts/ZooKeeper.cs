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
    public float speedBoostMultiplier = 2f;
    public int maxLives = 3;

    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private Coroutine alertCoroutine;
    private Coroutine speedBoostCoroutine;
    private int currentLives;
    private bool isCooldown = false; 
    public Image[] hearts;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = normalSpeed;
        currentLives = maxLives;
        if (fixedWaypoints.Length > 0)
        {
            agent.SetDestination(fixedWaypoints[currentWaypointIndex].position);
            animator.SetBool("isWalking", true);
        }
        UpdateHeartsUI();
    }

    void Update()
    {
        if (alertCoroutine == null && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % fixedWaypoints.Length;
            agent.SetDestination(fixedWaypoints[currentWaypointIndex].position);
            animator.SetBool("isWalking", true);
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
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", true);

        while (agent.remainingDistance > agent.stoppingDistance || agent.pathPending)
        {
            yield return null;
        }

        agent.isStopped = true;
        animator.SetBool("isRunning", false);
        animator.SetTrigger("HeadShake");

        yield return new WaitForSeconds(5);

        agent.isStopped = false;
        animator.SetBool("isWalking", true);
        agent.speed = normalSpeed;
        alertCoroutine = null;
        agent.SetDestination(fixedWaypoints[currentWaypointIndex].position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("tester") && !isCooldown)
        {
            StartCoroutine(LoseLifeWithCooldown());

            if (currentLives > 0 && speedBoostCoroutine == null)
            {
                speedBoostCoroutine = StartCoroutine(SpeedBoost());
            }
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

    private IEnumerator SpeedBoost()
    {
        agent.isStopped = true;
        animator.SetBool("isWalking", false); 
        animator.SetTrigger("HeadShake");

        yield return new WaitForSeconds(2);

        agent.isStopped = false;
        float originalSpeed = agent.speed;
        agent.speed = normalSpeed * speedBoostMultiplier;

        animator.SetBool("isRunning", true);
        if (fixedWaypoints.Length > 0)
        {
            agent.SetDestination(fixedWaypoints[currentWaypointIndex].position);
        }
    
        yield return new WaitForSeconds(10);

        agent.speed = originalSpeed;
        speedBoostCoroutine = null;
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", true); 
    }
}
