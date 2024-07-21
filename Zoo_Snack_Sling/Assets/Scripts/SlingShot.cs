using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for implementation of Slingshot for projectile motion mechanices
/// </summary>
public class SlingShot : MonoBehaviour
{

    public Transform slingshotAnchor;
    public Rigidbody projectileRb;  //food
    public float launchForceMultiplier = 10f;

    // Line Renderer components
    public LineRenderer lineRenderer;
    public int numPoints = 100; // Number of points in the trajectory line
    public float timeStep = 0.1f;   // Time difference between the points

    // Private variables
    private SpringJoint springJoint;
    private bool isDragging = false;
    private Vector3 initialDragPosition;
    public TrailRenderer trailRenderer;
    

    void Start()
    {
        springJoint = projectileRb.GetComponent<SpringJoint>(); // Getting spring joint component
        springJoint.connectedAnchor = slingshotAnchor.position; // Setting anchor position of spring joint
        trailRenderer.enabled = false;
    }
    // void SpawnNewProjectile()
    // {
    //     // Instantiate a new projectile and set it up
    //     currentProjectileRb = Instantiate(projectilePrefab, slingshotAnchor.position, Quaternion.identity);
    //     currentProjectileRb.isKinematic = true;

    //     // Attach a new Spring Joint to the new projectile
    //     springJoint = currentProjectileRb.gameObject.AddComponent<SpringJoint>();
    //     springJoint.connectedAnchor = slingshotAnchor.position;
    //     trailRenderer.enabled = false;
    // }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrag();
        }
        if (Input.GetMouseButton(0) && isDragging)
        {
            DragProjectile();
        }
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            DeattachSpringJoint();
            ClearTrajectory();
            ReleaseProjectile();
        }
    }

    void StartDrag()
    {
        isDragging = true;
        projectileRb.isKinematic = true;

        initialDragPosition = projectileRb.position; // Starting position of the attacker becomes the initial drag position
        Debug.Log("Drag started");
    }

    void DragProjectile()
    {
        // Convert mouse position to world position
        Vector3 mousePosition = Input.mousePosition;    // Get X, Y coordinates of mouse, Z considered to be 0
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.z - slingshotAnchor.position.z); // Calculate Z position based on the distance from the camera to the slingshot anchor.
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);  // Converting the mouse position to world position

        projectileRb.position = worldPosition;  // Set the projectile position to the world position

        Vector3 velocity = CalculateVelocity();
        // Visualizing the trajectory
        VisualizeTrajectory(projectileRb.position, velocity);
        Debug.Log("Dragging");
    }

    void ReleaseProjectile()
    {
        isDragging = false;

        Vector3 velocity = CalculateVelocity();
        // Apply force to the projectile
        projectileRb.isKinematic = false;
        projectileRb.AddForce(velocity, ForceMode.Impulse);
        trailRenderer.enabled = true;
        Debug.Log("Projectile released");
    }

    void DeattachSpringJoint()
    {
        // Detach the spring joint if not null
        if (springJoint != null)
        {
            springJoint.connectedBody = null;
            Destroy(springJoint); // Destroy the component
            springJoint = null;
            Debug.Log("Spring joint detached");
        }
    }

    Vector3 CalculateVelocity()
    {
        // Calculate the release direction and distance
        Vector3 releaseDirection = (initialDragPosition - projectileRb.position).normalized;
        float releaseDistance = Vector3.Distance(initialDragPosition, projectileRb.position);

        // Calculate the velocity based on direction, distance, and multiplier
        Vector3 velocity = releaseDirection * releaseDistance * launchForceMultiplier;
        return velocity;
    }

    void VisualizeTrajectory(Vector3 startPosition, Vector3 velocity)
    {
        // Visualize the trajectory using the LineRenderer
        lineRenderer.positionCount = numPoints;

        for (int i = 0; i < numPoints; i++)
        {
            float t = i * timeStep; // Calculate the time for each point
            Vector3 newPosition = CalculatePositionAtTime(startPosition, velocity, t);
            lineRenderer.SetPosition(i, newPosition); 
        }
    }

    Vector3 CalculatePositionAtTime(Vector3 startPosition, Vector3 initialVelocity, float time)
    {
        // Calculate the position at a given time using physics formulas
        Vector3 gravity = Physics.gravity;
        Vector3 position = startPosition + initialVelocity * time + 0.5f * gravity * time * time;
        return position;
    }

    void ClearTrajectory()
    {
        // Clear the trajectory line
        lineRenderer.positionCount = 0;
    }
}