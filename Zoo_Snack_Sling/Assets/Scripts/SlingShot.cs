using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for implementation of Slingshot for projectile motion mechanics
/// </summary>
public class SlingShot : MonoBehaviour
{
    public Transform slingshotAnchor;
    public Rigidbody projectileRb;  // Food
    public float launchForceMultiplier = 3f;
    public LineRenderer lineRenderer;
    public int numPoints = 100; // Number of points in the trajectory line
    public float timeStep = 0.1f;   // Time difference between the points
    public float seconds = 2f;  // Time to wait before respawning the projectile

    private SpringJoint springJoint;
    private bool isDragging = false;
    private Vector3 initialDragPosition;
    public TrailRenderer trailRenderer;

    private SlingshotReload slingshotReload;

    void Start()
    {
        springJoint = projectileRb.GetComponent<SpringJoint>(); // Getting spring joint component
        springJoint.connectedAnchor = slingshotAnchor.position; // Setting anchor position of spring joint
        trailRenderer.enabled = false;

        slingshotReload = FindObjectOfType<SlingshotReload>(); // Find the SlingshotReload script in the scene
    }

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
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.z - slingshotAnchor.position.z);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        projectileRb.position = worldPosition;

        Vector3 velocity = CalculateVelocity();
        VisualizeTrajectory(projectileRb.position, velocity);
        Debug.Log("Dragging");
    }

    void ReleaseProjectile()
    {
        isDragging = false;

        Vector3 velocity = CalculateVelocity();
        projectileRb.isKinematic = false;
        projectileRb.AddForce(velocity, ForceMode.Impulse);
        trailRenderer.enabled = true;
        Debug.Log("Projectile released");

        // Call the respawn function after 5 seconds
        slingshotReload.Invoke("RespawnProjectile", seconds);
    }

    void DeattachSpringJoint()
    {
        if (springJoint != null)
        {
            springJoint.connectedBody = null;
            Destroy(springJoint);
            springJoint = null;
            Debug.Log("Spring joint detached");
        }
    }

    Vector3 CalculateVelocity()
    {
        Vector3 releaseDirection = (initialDragPosition - projectileRb.position).normalized;
        float releaseDistance = Vector3.Distance(initialDragPosition, projectileRb.position);

        Vector3 velocity = releaseDirection * releaseDistance * launchForceMultiplier;
        return velocity;
    }

    void VisualizeTrajectory(Vector3 startPosition, Vector3 velocity)
    {
        lineRenderer.positionCount = numPoints;

        for (int i = 0; i < numPoints; i++)
        {
            float t = i * timeStep;
            Vector3 newPosition = CalculatePositionAtTime(startPosition, velocity, t);
            lineRenderer.SetPosition(i, newPosition);
        }
    }

    Vector3 CalculatePositionAtTime(Vector3 startPosition, Vector3 initialVelocity, float time)
    {
        Vector3 gravity = Physics.gravity;
        Vector3 position = startPosition + initialVelocity * time + 0.5f * gravity * time * time;
        return position;
    }

    void ClearTrajectory()
    {
        lineRenderer.positionCount = 0;
    }
}
