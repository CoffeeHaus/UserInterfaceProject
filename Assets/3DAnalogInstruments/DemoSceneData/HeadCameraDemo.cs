using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MGAssets
{
    public class HeadCameraDemo : MonoBehaviour
    {
        public Transform cameraHead;
        public bool cursorStartLocked = false;

        [Space]
        public float mouseSensitivity = 1f;
        public float zoomSensitivity = 1f;


        void Awake() { if (cameraHead == null) cameraHead = Camera.main.transform; }
        void Start() { if (cursorStartLocked) Cursor.lockState = CursorLockMode.Locked; else Cursor.lockState = CursorLockMode.None; }
        void Update()
        {
            // Mouse Head Movement (Only if cursor is locked)
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                // Rotation
                //transform.Rotate(-Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity, Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity, 0, Space.Self);
                if (!Input.GetMouseButton(0)) cameraHead.localRotation = Quaternion.Euler(cameraHead.localRotation.eulerAngles + new Vector3(- Input.GetAxis("Mouse Y"),Input.GetAxis("Mouse X"), 0) * mouseSensitivity * Time.deltaTime);
                //

                // Translation
                if (Input.GetMouseButton(0))
                {
                    cameraHead.localPosition = cameraHead.localPosition + mouseSensitivity * 0.01f * Time.deltaTime * new Vector3( 
                        Input.GetAxis("Mouse X"),
                        Input.GetMouseButton(1) ? 0 : Input.GetAxis("Mouse Y"),
                        Input.GetMouseButton(1) ? Input.GetAxis("Mouse Y") : 0
                        );
                }
                //

                // Reset
                if (Input.GetKey(KeyCode.H) || Input.GetMouseButtonDown(2))
                {
                    if (!Input.GetMouseButton(0)) cameraHead.localRotation = Quaternion.identity; else cameraHead.localPosition = Vector3.zero;
                }
                //
            }
            //

            // Camera Zoom
            if (cameraHead != null)
            {
                Camera.main.fieldOfView += Input.mouseScrollDelta.y * zoomSensitivity;
                if (Input.GetMouseButtonDown(3)) Camera.main.fieldOfView = 60;
            }
            //
        }
    }
}