using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 1000f;
    public Transform playerBody;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private Quaternion startingRotation;

    public Quaternion GetStartingRotation()
    {
        return startingRotation;
    }
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor to the center of the screen
        startingRotation = transform.rotation;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevents over-rotation

        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f); // Prevents over-rotation

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        //playerBody.Rotate(Vector3.up * mouseX);
    }
}
