using System;
using System.Collections;
using System.Collections.Generic;
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

    #endregion

    void Awake()
    {
        cam = GetComponent<Camera>();
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
                        Vector3 Direction = touchPos - cam.ScreenToWorldPoint(touch.position);
                        cam.transform.position += Direction;
                        ////clamp the camera position between the boundaries
                        transform.position = new Vector3(
                            Mathf.Clamp(transform.position.x, leftLimit, rightLimit), 
                            Mathf.Clamp(transform.position.y, bottomLimit, topLimit), 
                            transform.position.z
                        );
                        break;
                }

            }
            
        }
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
