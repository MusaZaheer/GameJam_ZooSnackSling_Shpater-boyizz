using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTrajectory : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField]
    [Range(3, 30)]
    private int lineSegments = 20;

    private List<Vector3> points = new List<Vector3>();

    #region Singleton

    public static DrawTrajectory Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public void UpdateTrajectory(Vector3 forceVector, Rigidbody rigidbody, Vector3 startingPoint)
    {
        Vector3 vector3 = (forceVector / rigidbody.mass) * Time.fixedDeltaTime;
        float flightDuration = (2 * vector3.y) / Physics.gravity.y;
        float timeStep = flightDuration / lineSegments;
        points.Clear();

        for (int i = 0; i < lineSegments; i++)
        {
            float stepTime = timeStep * i;
            Vector3 MovementVector = new Vector3(vector3.x * stepTime, vector3.y * stepTime + 0.5f * Physics.gravity.y * stepTime * stepTime, vector3.z * stepTime);
            points.Add(startingPoint + MovementVector);
        }
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    public void HideTrajectory()
    {
        lineRenderer.positionCount = 0;
    }

}
