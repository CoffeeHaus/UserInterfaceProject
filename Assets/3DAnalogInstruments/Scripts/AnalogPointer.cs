using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MGAssets
{
    public class AnalogPointer : MonoBehaviour
    {
        public bool isActive = true;
        public Transform transf;

        [Space]
        [Header("Value Configuration")]
        [Space]
        public float value;
        float _value;

        [Space]
        public float valueFactor = 1;
        public float valueOffSet = 0;
        public float maxValue = 1, minValue = -1;

        [Space] [Header("Movement Configuration")]
        [Space(15)]
        public bool moveX = false;
        public bool rotateX = false;
        public float currentX = 0;

        [Space]
        public float factorX = 1;
        public float offSetX = 0;
        public bool clampX = true;
        public float maxX = 45, minX = -45;

        [Space(15)]
        public bool moveY = false;
        public bool rotateY = false;
        public float currentY = 0;

        [Space]
        public float factorY = 1;
        public float offSetY = 0;
        public bool clampY = true;
        public float maxY = 45, minY = -45;

        [Space(15)]
        public bool moveZ = false;
        public bool rotateZ = false;
        public float currentZ = 0;

        [Space]
        public float factorZ = 1;
        public float offSetZ = 0;
        public bool clampZ = true;
        public float maxZ = 45, minZ = -45;



        // Context Menu
        [ContextMenu("Update Value")]
        void contextUpdateValue() { setValue(value); }
        //

        ////// Auto-Update Mode
        ////[Space(30)] public bool autoUpdate = true;
        ////void Update() { if (!autoUpdate) return; setValue(value); }
        //////



        //Update to Inicial Value
        void OnEnable() 
        {
            if (!transf) transf = GetComponent<Transform>();
            if (isActive && gameObject.activeInHierarchy) setValue(value); 
        }
        //


        //External call to set _value
        public void setValue(float newValue)
        {
            if (!isActive) return;

            //Update Value
            _value = valueOffSet + valueFactor * newValue;

            ///////////////// Rotation 
            //Rotate X
            if (rotateX)
            {
                if (!clampX) currentX = Mathf.MoveTowardsAngle(currentX, minX + offSetX + factorX * _value * (maxX - minX) / (maxValue - minValue), 360);
                else
                {
                    currentX = Mathf.Clamp(
                        Mathf.MoveTowardsAngle(currentX, minX + offSetX + factorX * _value * (maxX - minX) / (maxValue - minValue), 360),
                        (minX <= maxX) ? minX : maxX, (maxX >= minX) ? maxX : minX
                        );
                }

                if (transf != null && !float.IsNaN(currentX)) transf.localRotation = Quaternion.Euler(currentX, transf.localRotation.eulerAngles.y, transf.localRotation.eulerAngles.z);
            }
            //

            //Rotate Y
            if (rotateY)
            {
                if (!clampY) currentY = Mathf.MoveTowardsAngle(currentY, minY + offSetY + factorY * _value * (maxY - minY) / (maxValue - minValue), 360);
                else
                {
                    currentY = Mathf.Clamp(
                        Mathf.MoveTowardsAngle(currentY, minY + offSetY + factorY * _value * (maxY - minY) / (maxValue - minValue), 360),
                        (minY <= maxY) ? minY : maxY, (maxY >= minY) ? maxY : minY
                        );
                }

                if (transf != null && !float.IsNaN(currentY)) transf.localRotation = Quaternion.Euler(transf.localRotation.eulerAngles.x, currentY, transf.localRotation.eulerAngles.z);
            }
            //

            //Rotate Z
            if (rotateZ)
            {
                if (!clampZ) currentZ = Mathf.MoveTowardsAngle(currentZ, minZ + offSetZ + factorZ * _value * (maxZ - minZ) / (maxValue - minValue), 360);
                else
                {
                    currentZ = Mathf.Clamp(
                        Mathf.MoveTowardsAngle(currentZ, minZ + offSetZ + factorZ * _value * (maxZ - minZ) / (maxValue - minValue), 360),
                        (minZ <= maxZ) ? minZ : maxZ, (maxZ >= minZ) ? maxZ : minZ
                        );
                }

                if (transf != null && !float.IsNaN(currentZ)) transf.localRotation = Quaternion.Euler(transf.localRotation.eulerAngles.x, transf.localRotation.eulerAngles.y, currentZ);
            }
            //
            ///////////////// Rotation 


            ///////////////// Translation 
            //Translate X
            if (moveX)
            {
                currentX = offSetX + factorX * _value * (maxX - minX) / (maxValue - minValue);
                if (clampX) currentX = Mathf.Clamp(currentX, minX, maxX);

                //Set Arrow X position
                if (transf != null && !float.IsNaN(currentX)) transf.localPosition = new Vector3(currentX, transf.localPosition.y, transf.localPosition.z);
            }
            //

            //Translate Y
            if (moveY)
            {
                currentY = offSetY + factorY * _value * (maxY - minY) / (maxValue - minValue);
                if (clampY) currentY = Mathf.Clamp(currentY, minY, maxY);

                //Set Arrow Y position
                if (transf != null && !float.IsNaN(currentY)) transf.localPosition = new Vector3(transf.localPosition.x, currentY, transf.localPosition.z);
            }
            //

            //Translate Z
            if (moveZ)
            {
                currentZ = offSetZ + factorZ * _value * (maxZ - minZ) / (maxValue - minValue);
                if (clampZ) currentZ = Mathf.Clamp(currentZ, minZ, maxZ);

                //Set Arrow Y position
                if (transf != null && !float.IsNaN(currentZ)) transf.localPosition = new Vector3(transf.localPosition.x, transf.localPosition.y, currentZ);
            }
            //
            ///////////////// Translation 



        }
        //

    }
}