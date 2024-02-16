using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MGAssets
{
    public class AnalogWheel : MonoBehaviour
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
        float maxValue = 10f;
        float fraction, rollOverFactor;

        [Space]
        public float masterFactor = 1;

        [Space]
        [Header("Wheel Configuration")]
        [Space]
        public bool continuousMode = false;
        public bool sendValueToNext = true;
        public AnalogWheel nextWheel;
        public bool sendRaw = true;


        [Space]
        [Header("Movement Configuration")]
        [Space(15)]
        public bool rotateX = false;
        public float currentX = 0;

        [Space]
        public float factorX = 1;
        public float offSetX = 0;
        //public bool clampX = true;
        //public float maxX = 360, minX = -360;

        [Space(15)]
        public bool rotateY = false;
        public float currentY = 0;

        [Space]
        public float factorY = 1;
        public float offSetY = 0;
        //public bool clampY = true;
        //public float maxY = 360, minY = -360;

        [Space(15)]
        public bool rotateZ = false;
        public float currentZ = 0;

        [Space]
        public float factorZ = 1;
        public float offSetZ = 0;
        //public bool clampZ = true;
        //public float maxZ = 360, minZ = -360;





        // Context Menu
        [ContextMenu("Update Value")]
        void contextUpdateValue() { setValue(value); }
        //

        ////// Auto-Update Mode
        ////[Space(30)] public bool autoUpdate = true;
        ////void Update() { if (!autoUpdate) return; setValue(value); }
        //////



        // Update to Inicial Value
        void OnEnable()
        {
            if (!transf) transf = GetComponent<Transform>();
            if (isActive && gameObject.activeInHierarchy) setValue(value);
        }
        //


        // External call to set _value
        public void setValue(float newValue)
        {
            if (!isActive) return;

            // Update Value and Calculate fraction
            value = newValue;
            _value = Mathf.Abs(masterFactor * (valueOffSet + valueFactor * value) % maxValue);

            if (!continuousMode) 
            {
                _value = Mathf.Round( (1f/ valueFactor) * _value) / (1f / valueFactor);
                if (rollOverFactor <= 0.5f) _value = Mathf.Floor(_value); else _value = Mathf.Ceil(_value) - 1;
            }

            fraction = Mathf.Abs((_value / maxValue) % 1);
            //

            // Send RollOver data to next Wheel
            if (sendValueToNext && nextWheel != null)
            {
                if (continuousMode && _value >= 9f) nextWheel.rollOverFactor = (fraction - 0.9f) * 10f;
                else if (!continuousMode && _value >= 9f) nextWheel.rollOverFactor = rollOverFactor;
                else nextWheel.rollOverFactor = 0;

                nextWheel.setValue((sendRaw) ? value : _value);
            }
            //

            ///////////////// Rotation 
            //
            //Rotate X
            if (rotateX)
            {
                currentX = offSetX + factorX * (fraction + (continuousMode ? 0 : rollOverFactor / 10f)) * 360f;
                //if (clampX) currentX = Mathf.Clamp(currentX, minX, maxX);
                if (transf != null && !float.IsNaN(currentX)) transf.localRotation = Quaternion.Euler(currentX, transf.localRotation.y, transf.localRotation.z);
            }
            //

            //Rotate Y
            if (rotateY)
            {
                currentY = offSetY + factorY * (fraction + (continuousMode ? 0 : rollOverFactor / 10f)) * 360f;
                //////if (continuousMode) currentY = offSetY + factorY * fraction * 360f; else currentY = offSetY + factorY * (fraction + rollOverFactor / 10f) * 360f;
                //if (clampY) currentY = Mathf.Clamp(currentY, minY, maxY);
                if (transf != null && !float.IsNaN(currentY)) transf.localRotation = Quaternion.Euler(transf.localRotation.x, currentY, transf.localRotation.z);
            }
            //

            //Rotate Z
            if (rotateZ)
            {
                currentZ = offSetZ + factorZ * (fraction + (continuousMode ? 0 : rollOverFactor / 10f)) * 360f;
                //if (clampZ) currentZ = Mathf.Clamp(currentZ, minZ, maxZ);
                if (transf != null && !float.IsNaN(currentZ)) transf.localRotation = Quaternion.Euler(transf.localRotation.x, transf.localRotation.y, currentZ);
            }
            //
            ///////////////// Rotation 

        }
        //




    }
}