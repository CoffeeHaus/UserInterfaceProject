using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public PropellerController propellerSpinScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float throttleInput = Input.GetAxis("Vertical"); // Assuming vertical input controls throttle
        propellerSpinScript.SetThrottle(throttleInput);
    }
}
