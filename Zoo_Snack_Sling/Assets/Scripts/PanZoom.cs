using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanZoom : MonoBehaviour
{
    #region Private Variables
    //cam move boundary vars
    [SerializeField] private float rightLimit;
    [SerializeField] private float leftLimit;
    [SerializeField] private float topLimit;
    [SerializeField] private float bottomLimit;
    //zoom vars
    [SerializeField] private float minZoom = 1f;
    [SerializeField] private float maxZoom = 8f;
    //other vars
    private Camera cam;
    private bool moveAllowed;
    private Vector3 touchPos;
    private string fruitTag;

    #endregion

    void Awake()
    {
        cam = GetComponent<Camera>();
        fruitTag = "Food";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            if(Input.touchCount == 2)
            {
                //zooming mechanics
                // Touch touchZero = Input.GetTouch(0);
                // Touch touchOne = Input.GetTouch(1);

                // if(EventSystem.current.IsPointerOverGameObject(touchZero.fingerId) || EventSystem.current.IsPointerOverGameObject(touchOne.fingerId))
                // {
                //     return;
                // }
                
            }
            //else if touched the fruit do not move the camera
            else if(IsTouchingFruit(Input.GetTouch(0)))
            {
                moveAllowed = false;
            }
            else
            {
                Touch touch = Input.GetTouch(0);
                //touchPos = cam.ScreenToWorldPoint(touch.position);

                switch(touch.phase)
                {
                    //when touch started
                    case TouchPhase.Began:
                    //check if the touch over the gameobject
                        if(EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        {
                            moveAllowed = false;
                        }
                        else
                        {
                            moveAllowed = true;
                        }
                        //store the touch position
                        touchPos = cam.ScreenToWorldPoint(touch.position);
                        break;
                    //when touch moved
                    case TouchPhase.Moved:
                        //if moveAllowed is true then move the camera
                        if(moveAllowed)
                        {
                            Vector3 direction = touchPos - cam.ScreenToWorldPoint(touch.position);
                            cam.transform.position += direction;
                            //clamp the camera position between the boundaries
                            // cam.transform.position = new Vector3(
                            //     Mathf.Clamp(cam.transform.position.x, leftLimit, rightLimit), 
                            //     Mathf.Clamp(cam.transform.position.y, bottomLimit, topLimit), 
                            //     cam.transform.position.z
                            // );
                        }
                        break;
                }
            }
        }
    }

    private bool IsTouchingFruit(Touch touch)
    {
        Ray ray = cam.ScreenPointToRay(touch.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag(fruitTag))
            {
                return true; // Touch is over a fruit object
            }
        }
        return false; // Touch is not over a fruit object
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(
            (rightLimit - Math.Abs(leftLimit) / 2), 
            (topLimit - Math.Abs(bottomLimit) / 2), 
            0), 
            new Vector3(rightLimit - leftLimit, topLimit - bottomLimit));
    }
}
