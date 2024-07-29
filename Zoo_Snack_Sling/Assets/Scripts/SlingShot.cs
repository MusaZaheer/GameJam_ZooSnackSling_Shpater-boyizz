using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShot : MonoBehaviour
{
    #region public variables
    public Transform slingshotAnchor;
    public Rigidbody projectileRb;  // Food
    public float launchForceMultiplier = 3f;
    public LineRenderer lineRenderer;
    public int numPoints = 100; // Number of points in the trajectory line
    public float timeStep = 0.1f;   // Time difference between the points
    public float seconds = 2f;  // Time to wait before respawning the projectile
    public TrailRenderer trailRenderer;
    public float maxStretch = 0.5f;  // Maximum stretch distance
    public float maxLeftLimit = -0.5f; // Maximum left limit
    public float maxRightLimit = 100f; // Maximum right limit
    public Transform launchStartPoint; // Fixed launch start point
    #endregion

    #region private variables
    private SpringJoint springJoint;
    private bool isDragging = false;
    private Vector3 initialDragPosition;
    private bool foodAccess; // Flag to access the food object when in the initial position only
    private SlingshotReload slingshotReload;
    #endregion

    void Start()
    {
        springJoint = projectileRb.GetComponent<SpringJoint>(); // Getting spring joint component
        springJoint.connectedAnchor = slingshotAnchor.position; // Setting anchor position of spring joint
        trailRenderer.enabled = false;
        foodAccess = true;
        Debug.Log("Food Can Be Accessed, Start");
        slingshotReload = FindObjectOfType<SlingshotReload>(); // Find the SlingshotReload script in the scene
        SetProjectileToLaunchStartPoint(); // Set projectile to the fixed launch start point
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && foodAccess)
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
        //Setting the position of the projectile to the mouse position
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.z - slingshotAnchor.position.z);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Calculate the distance and direction from the slingshot anchor to the mouse position in the world
        Vector3 direction = (worldPosition - slingshotAnchor.position).normalized;
        //now the distance between the anchor and the mouse position is calculated so that the projectile does not go beyond the maximum stretch
        float distance = Vector3.Distance(worldPosition, slingshotAnchor.position);

        // If the distance is greater than the maximum stretch, clamp it to the maximum stretch
        if (distance > maxStretch)
        {
            worldPosition = slingshotAnchor.position + direction * maxStretch;
            //worldPosition = slingshotAnchor.position + direction;
        }

        // Clamp the x-coordinate within the left and right limits
        float clampedX = Mathf.Clamp(worldPosition.x, slingshotAnchor.position.x + maxLeftLimit, slingshotAnchor.position.x + maxRightLimit);
        // Ensure the projectile cannot be dragged behind the slingshot anchor
        // float clampedZ = Mathf.Min(worldPosition.z, slingshotAnchor.position.z);
        //worldPosition = new Vector3(clampedX, worldPosition.y, clampedZ);


        projectileRb.position = worldPosition;

        Vector3 velocity = CalculateVelocity();
        VisualizeTrajectory(projectileRb.position, velocity);
    }

    void ReleaseProjectile()
    {
        isDragging = false;
        foodAccess = false;
        Debug.Log("Food Can NOT Be Accessed, ReleaseProjectile");

        Vector3 velocity = CalculateVelocity();
        projectileRb.isKinematic = false;
        projectileRb.AddForce(velocity, ForceMode.Impulse);
        trailRenderer.enabled = true;

        // Call the respawn function after the specified seconds
        Invoke(nameof(RespawnProjectile), seconds);
    }

    void DeattachSpringJoint()
    {
        if (springJoint != null)
        {
            springJoint.connectedBody = null;
            Destroy(springJoint);
            springJoint = null;
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

    public void RespawnProjectile()
    {
        slingshotReload.RespawnProjectile();
        trailRenderer.enabled = false;
        foodAccess = true; // Enable food access after respawning the projectile
        Debug.Log("Food Can Be Accessed, RespawnProjectile");
        SetProjectileToLaunchStartPoint(); // Set projectile to the fixed launch start point
    }

    void SetProjectileToLaunchStartPoint()
    {
        projectileRb.position = launchStartPoint.position;
        projectileRb.rotation = launchStartPoint.rotation;
    }

}