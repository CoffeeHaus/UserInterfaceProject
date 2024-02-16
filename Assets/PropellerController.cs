using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerController : MonoBehaviour
{
    public float maxRotationSpeed = 2000f; // Maximum rotation speed of the propeller
    public float throttle = 0f; // Throttle value, range between 0 and 1

    void Update()
    {
        // Calculate the current rotation speed based on the throttle
        float currentSpeed = maxRotationSpeed * throttle;

        // Rotate the propeller around its forward axis
        transform.Rotate(Vector3.forward, currentSpeed * Time.deltaTime);
    }

    // Method to set the throttle from outside this script
    public void SetThrottle(float value)
    {
        throttle = Mathf.Clamp01(value); // Ensure the throttle value stays between 0 and 1
    }
}
