using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FirstPersonCamera : MonoBehaviour
{
    //public Transform player;
    public Transform orientation;

    public float mouseSensitivity = 2f;

    float xRotation = 0;
    float yRotation = 0;
    private void Awake()
    {
        
    }
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;    
        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


        //Rotate camera around its local Y axis (vertical)
        yRotation += inputX;

        //Rotate player around its X axis (horizontal)
        xRotation -= inputY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
