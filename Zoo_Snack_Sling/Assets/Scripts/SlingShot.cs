using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for implementation of Slingshot for projectile motion mechanices
/// </summary>
public class SlingShot : MonoBehaviour
{
    //public variables
    public Transform slingshotAnchor;   
    public Rigidbody projectileRb;  //attacker
    public float launchForceMultiplier = 10f;

    //Line Renderer components
    public LineRenderer lineRenderer;
    //number of steps
    public int numPoints = 100; //no. of points in the trajectory line
    public float timeStep = 0.1f;   //time diff between the points
    //private variables
    private SpringJoint springJoint;
    private bool isDragging = false;    
    private Vector3 initialDragPosition;
    public TrailRenderer trailRenderer;

    // Start is called before the first frame update
    void Start()
    {
        springJoint = projectileRb.GetComponent<SpringJoint>(); //getting spring joint component
        springJoint.connectedAnchor = slingshotAnchor.position; //setting anchor position of spring joing
        trailRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //simple if conditions
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
        //flags
        isDragging = true;
        projectileRb.isKinematic = true;

        initialDragPosition = projectileRb.position; // starting position of the attacker becomes the initial drag pos
        Debug.Log("Drag started");
    }

    void DragProjectile()
    {
        // Convert mouse position to world position
        Vector3 mousePosition = Input.mousePosition;    //get X,Y coordinates of mouse, Z considered to be 0
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.z - slingshotAnchor.position.z); // Calculate Z position based on the distance from the camera to the slingshot anchor.
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);  //converting the mouse position to world position
        //setting projectile
        worldPosition.z = slingshotAnchor.position.z; // Constrain the XY plane, Z constant
        projectileRb.position = worldPosition;  // Set the projectile position to the world position

        Vector3 velocity = CalculateVelocity();
        //visualizing line
        VisualizeTrajectory(projectileRb.position, velocity);
        Debug.Log("Dragging");
    }

    void ReleaseProjectile()
    {
        // flag off and calculating the direction and distance of the release
        isDragging = false;

        Vector3 velocity = CalculateVelocity();
        // Flag off & Apply force to the projectile
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

    Vector3 CalculateVelocity(){
        //release direction
        Vector3 releaseDirection = (initialDragPosition - projectileRb.position).normalized;
        //release distance
        float releaseDistance = Vector3.Distance(initialDragPosition, projectileRb.position);
        //velocity calculation
        Vector3 velocity = releaseDirection * releaseDistance * launchForceMultiplier;
        return velocity;
    }
    
    void VisualizeTrajectory(Vector3 startPosition, Vector3 velocity)
    {
        lineRenderer.positionCount = numPoints; //points

        for (int i = 0; i < numPoints; i++)
        {
            float t = i * timeStep; //time = iter * 0.1
            Vector3 newPosition = CalculatePositionAtTime(startPosition, velocity, t);
            lineRenderer.SetPosition(i, newPosition); 
        }
    }

    
    Vector3 CalculatePositionAtTime(Vector3 startPosition, Vector3 initialVelocity, float time)
    {
        // Physics formula: position = start + velocity * time + 0.5 * gravity * time^2 (v0 + vf * t + 1/2 * g * t^2)
        Vector3 gravity = Physics.gravity;
        Vector3 position = startPosition + initialVelocity * time + 0.5f * gravity * time * time;
        return position;
    }

    void ClearTrajectory()
    {
        //clear the trajectory line
        lineRenderer.positionCount = 0;
    }
}
