using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShot : MonoBehaviour
{
    #region public variables
    [SerializeField] public int viusalizerTrimmer = 15; //use this variable to reduce the number of points in the trajectory line
    public Transform slingshotAnchor;
    public Rigidbody projectileRb;  // Food
    public float launchForceMultiplier = 3f;
    public LineRenderer lineRenderer;
    public int numPoints = 10; // Reduced number of points in the trajectory line
    public float timeStep = 0.1f;   // Time difference between the points
    public float seconds = 2f;  // Time to wait before respawning the projectile
    public TrailRenderer trailRenderer;
    public float maxStretch = 0.5f;  // Maximum stretch distance
    public float maxLeftLimit = -0.5f; // Maximum left limit
    public float maxRightLimit = 100f; // Maximum right limit
    public Transform launchStartPoint; // Fixed launch start point
    public Material dottedLineMaterial; // Material for the dotted line
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
        slingshotReload = FindObjectOfType<SlingshotReload>(); // Find the SlingshotReload script in the scene
        SetProjectileToLaunchStartPoint(); // Set projectile to the fixed launch start point

        // Assign the dotted line material to the LineRenderer
        if (lineRenderer != null && dottedLineMaterial != null)
        {
            lineRenderer.material = dottedLineMaterial;
        }
    }

    void Update()
    {
        //food can only be dragged when 1) mouse is clicked 2) food is in the initial position 3) mouse/ finger is on the projectile
        if (Input.GetMouseButtonDown(0) && foodAccess && IsMouseOnProjectile())
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

    //this function is to check if the mouse is on the projectile
    bool IsMouseOnProjectile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Check if the object hit by the raycast is the projectile or its child
            return hit.collider.gameObject == projectileRb.gameObject || hit.collider.transform.IsChildOf(projectileRb.transform);
        }
        return false;
    }

    //start dragging the projectile
    void StartDrag()
    {
        isDragging = true;
        projectileRb.isKinematic = true;
        initialDragPosition = projectileRb.position; // Starting position of the attacker becomes the initial drag position
    }

    //drag the projectile
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

    //throw or launch the projectile
    void ReleaseProjectile()
    {
        isDragging = false;
        foodAccess = false;
        //Debug.Log("Food Can NOT Be Accessed, ReleaseProjectile");

        Vector3 velocity = CalculateVelocity();
        projectileRb.isKinematic = false;
        projectileRb.AddForce(velocity, ForceMode.Impulse);
        trailRenderer.enabled = true;

        // Call the respawn function after the specified seconds
        Invoke(nameof(RespawnProjectile), seconds);
    }

    //this function is to deattach the spring joint
    void DeattachSpringJoint()
    {
        if (springJoint != null)
        {
            springJoint.connectedBody = null;
            Destroy(springJoint);
            springJoint = null;
        }
    }

    //calculating the velocity of the projectile
    Vector3 CalculateVelocity()
    {
        Vector3 releaseDirection = (initialDragPosition - projectileRb.position).normalized;
        float releaseDistance = Vector3.Distance(initialDragPosition, projectileRb.position);
        Vector3 velocity = releaseDirection * releaseDistance * launchForceMultiplier;
        return velocity;
    }

    //drawing the trajectory line
    void VisualizeTrajectory(Vector3 startPosition, Vector3 velocity)
{
    // Determine if there is any object in the path of the trajectory
    if (IsTrajectoryClear(startPosition, velocity))
    {
        // No object detected, use the normal trajectory visualization
        DrawNormalTrajectory(startPosition, velocity);
    }
    else
    {
        // Object detected, use the collision-aware trajectory visualization
        DrawCollisionAwareTrajectory(startPosition, velocity);
    }
}

bool IsTrajectoryClear(Vector3 startPosition, Vector3 velocity)
{
    int midNumPoints = Mathf.RoundToInt(numPoints / viusalizerTrimmer);
    Vector3 previousPosition = startPosition;

    for (int i = 0; i < midNumPoints; i++)
    {
        float t = i * timeStep;
        Vector3 newPosition = CalculatePositionAtTime(startPosition, velocity, t);

        // Check for collision between previousPosition and newPosition
        if (Physics.Raycast(previousPosition, newPosition - previousPosition, out RaycastHit hit, Vector3.Distance(previousPosition, newPosition)))
        {
            return false; // Object detected
        }

        previousPosition = newPosition;
    }

    return true; // No object detected
}

void DrawNormalTrajectory(Vector3 startPosition, Vector3 velocity)
{
    lineRenderer.positionCount = numPoints;

    // Calculate the total flight time
    float totalFlightTime = (2 * velocity.y / -Physics.gravity.y);
    int midNumPoints = Mathf.RoundToInt(numPoints / viusalizerTrimmer);

    for (int i = 0; i < midNumPoints; i++)
    {
        float t = i * timeStep;
        Vector3 newPosition = CalculatePositionAtTime(startPosition, velocity, t);
        lineRenderer.SetPosition(i, newPosition);
    }

    // Update the position count to midNumPoints
    lineRenderer.positionCount = midNumPoints;
}

void DrawCollisionAwareTrajectory(Vector3 startPosition, Vector3 velocity)
{
    int midNumPoints = Mathf.RoundToInt(numPoints / viusalizerTrimmer);
    lineRenderer.positionCount = midNumPoints; // Set initial position count to the maximum possible points

    Vector3 previousPosition = startPosition;
    for (int i = 0; i < midNumPoints; i++)
    {
        float t = i * timeStep;
        Vector3 newPosition = CalculatePositionAtTime(startPosition, velocity, t);

        // Check for collision between previousPosition and newPosition
        if (Physics.Raycast(previousPosition, newPosition - previousPosition, out RaycastHit hit, Vector3.Distance(previousPosition, newPosition)))
        {
            // If a collision is detected, stop drawing the trajectory at the collision point
            lineRenderer.positionCount = i + 1; // Update position count to the actual number of points drawn
            lineRenderer.SetPosition(i, hit.point);
            return;
        }
        else
        {
            lineRenderer.SetPosition(i, newPosition);
            previousPosition = newPosition;
        }
    }
}

Vector3 CalculatePositionAtTime(Vector3 startPosition, Vector3 initialVelocity, float time)
{
    Vector3 gravity = Physics.gravity;
    Vector3 position = startPosition + initialVelocity * time + 0.5f * gravity * time * time;
    return position;
}


    //clear the trajectory line
    void ClearTrajectory()
    {
        lineRenderer.positionCount = 0;
    }

    //this function is to respawn the projectile after the specified seconds
    public void RespawnProjectile()
    {
        slingshotReload.RespawnProjectile();
        trailRenderer.enabled = false;
        foodAccess = true; // Enable food access after respawning the projectile
        //Debug.Log("Food Can Be Accessed, RespawnProjectile");
        SetProjectileToLaunchStartPoint(); // Set projectile to the fixed launch start point
    }

    //this function is to make sure that food is projected only from the fixed starting point i.e. doesnt leave its initial position    
    void SetProjectileToLaunchStartPoint()
    {
        projectileRb.position = launchStartPoint.position;
        projectileRb.rotation = launchStartPoint.rotation;
    }
}
