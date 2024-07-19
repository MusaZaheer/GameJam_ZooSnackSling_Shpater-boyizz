using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline : MonoBehaviour
{
    public Transform a, b, c, d;
    public Transform ab;
    public Transform bc;
    public Transform cd;
    public Transform ab_bc;
    public Transform bc_cd;
    public Transform result;
    public float interpolateAmount;

    public Vector3 velocity = Vector3.zero;
    private Vector3 velocityAB;
    private Vector3 velocityBC;
    private Vector3 velocityCD;
    private Vector3 velocityAB_BC;
    private Vector3 velocityBC_CD;
    private Vector3 velocityResult;
    public float smoothTime = 1f;

    public Transform fromRotation, toRotation;
    public float rotationSpeed = 0.5f;
    //private float timeCount = 0f;

    // Update is called once per frame
    void Update()
    {
        //using LERP
        //interpolateAmount = (interpolateAmount + Time.deltaTime) % 1f;
        // ab.position = Vector3.Lerp(a.position, b.position, interpolateAmount);  //a to b
        // bc.position = Vector3.Lerp(b.position, c.position, interpolateAmount);  //b to c
        // cd.position = Vector3.Lerp(c.position, d.position, interpolateAmount);  //c to d
        // ab_bc.position = Vector3.Lerp(ab.position, bc.position, interpolateAmount); //a to c
        // bc_cd.position = Vector3.Lerp(bc.position, cd.position, interpolateAmount); //b to d
        // result.position = Vector3.Lerp(ab_bc.position, bc_cd.position, interpolateAmount);  //a to d

        //For Smooth Damp
        interpolateAmount = Mathf.Lerp(interpolateAmount, 1f, Time.deltaTime / smoothTime);

        ab.position = Vector3.Lerp(a.position, b.position, interpolateAmount);  //a to b
        bc.position = Vector3.Lerp(b.position, c.position, interpolateAmount);  //b to c
        cd.position = Vector3.Lerp(c.position, d.position, interpolateAmount);  //c to d
        ab_bc.position = Vector3.Lerp(ab.position, bc.position, interpolateAmount); //a to c
        bc_cd.position = Vector3.Lerp(bc.position, cd.position, interpolateAmount); //b to d
        result.position = Vector3.Lerp(ab_bc.position, bc_cd.position, interpolateAmount);  //a to d

        // Reset interpolateAmount to 0 once it reaches 1 to continuously interpolate
        if (interpolateAmount >= 0.99f)
        {
            interpolateAmount = 0f;
        }

        //Lerp rotation
        result.transform.rotation = Quaternion.Lerp(result.transform.rotation, toRotation.rotation, Time.deltaTime * rotationSpeed);
        //reset when reach to 1
        if (result.transform.rotation == toRotation.rotation)
        {
            result.transform.rotation = fromRotation.rotation;
        }        

    }
}
