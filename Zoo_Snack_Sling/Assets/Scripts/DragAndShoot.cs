using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DragAndShoot : MonoBehaviour
{
    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;
    // [SerializeField] private LineRenderer lineRenderer;
    // [SerializeField]
    // [Range(3, 30)]
    // private int lineSegments = 20;
    private Rigidbody rb;

    private bool isShoot;

    public float timeStep = 0.1f;   // Time difference between the points
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnMouseDown()
    {
        mousePressDownPos = Input.mousePosition;
        // VisualizeTrajectory(transform.position, rb.velocity);
        //move food along the dragging

    }

    private void OnMouseUp()
    {
        mouseReleasePos = Input.mousePosition;
        Shoot(mousePressDownPos-mouseReleasePos);
    }

    [SerializeField]private float forceMultiplier = 3;
    void Shoot(Vector3 Force)
    {

        if(isShoot)    
            return;
        
        rb.AddForce(new Vector3(Force.x,Force.y,Force.y) * forceMultiplier);
        //VisualizeTrajectory(transform.position, rb.velocity);
        isShoot = true;
        Spawner.Instance.NewSpawnRequest();
    }

    // void VisualizeTrajectory(Vector3 startPosition, Vector3 velocity)
    // {
    //     // Visualize the trajectory using the LineRenderer
    //     lineRenderer.positionCount = lineSegments;

    //     for (int i = 0; i < lineSegments; i++)
    //     {
    //         float t = i * timeStep; // Calculate the time for each point
    //         Vector3 newPosition = CalculatePositionAtTime(startPosition, velocity, t);
    //         lineRenderer.SetPosition(i, newPosition); 
    //     }
    // }

    // Vector3 CalculatePositionAtTime(Vector3 startPosition, Vector3 initialVelocity, float time)
    // {
    //     // Calculate the position at a given time using physics formulas
    //     Vector3 gravity = Physics.gravity;
    //     Vector3 position = startPosition + initialVelocity * time + 0.5f * gravity * time * time;
    //     return position;
    // }
    
}