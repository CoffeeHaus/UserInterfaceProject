using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MGAssets
{
    public class AnalogInstruments : MonoBehaviour
    {
        public static AnalogInstruments current;

        public bool isActive = true;
        [Tooltip("Link your Aircraft's DataCenter here to distribute all calculated values to the instruments.")]
        public AnalogDataCenter dataCenter;


        [Space(10)]
        [Header("Roll")]
        public bool useRoll = true;
        public Text horizonRollTxt;
        public AnalogPointer[] horizonRollPointers;


        [Space(10)]
        [Header("Pitch")]
        public bool usePitch = true;
        public Text horizonPitchTxt;
        public AnalogPointer[] horizonPitchPointers;
        public Material horizonPitchMaterial;

        [Space(10)]
        [Header("Heading & TurnRate")]
        public bool useHeading = true;
        public Text headingTxt;
        public AnalogPointer[] compassPointers;
        public AnalogWheel[] compassWheels;


        [Space]
        public bool useTurnRate = true;
        public Text turnRateTxt;
        public AnalogPointer[] turnRatePointers;


        [Space(10)]
        [Header("Altitude")]
        public bool useAltitude = true;
        public Text altitudeTxt;
        public AnalogPointer[] altitudePointers;
        public AnalogWheel[] altitudeWheels;


        [Space]
        public bool useRadarAltitude = false;
        public bool useSeparatedRadarAltitude = true;
        public Text radarAltitudeTxt;
        public AnalogPointer[] radarAltPointers;
        public AnalogWheel[] radarAltWheels;


        [Space(10)]
        [Header("Speed")]
        public bool useSpeed = true;
        public bool nonNegativeSpeed = false;
        public Text speedTxt;
        public AnalogPointer[] speedPointers;
        public AnalogWheel[] speedWheels;

        [Space]
        public Text absSpeedTxt;
        public AnalogPointer[] absSpeedPointers;
        public AnalogWheel[] absSpeedWheels;


        [Space(10)]
        [Header("Vertical Velocity")]
        public bool useVV = true;
        public Text verticalSpeedTxt;
        public bool roundVV = true, showDecimalVV = true;
        public float roundFactorVV = 0.1f;

        [Space]
        public AnalogPointer[] vvPointers;
        public AnalogWheel[] vvWheels;


        [Space(10)]
        [Header("Horizontal Velocity")]
        public bool useHV = true;
        public Text horizontalSpeedTxt;
        public bool roundHV = true, showDecimalHV = true;
        public float roundFactorHV = 0.1f;

        [Space]
        public AnalogPointer[] hvPointers;
        public AnalogWheel[] hvWheels;


        [Space(10)]
        [Header("G-Force")]
        public bool useGForce = true;
        public Text gForceTxt, maxGForceTxt, minGForceTxt;
        public AnalogPointer[] gForcePointers;


        [Space(10)]
        [Header("AOA, AOS and GlidePath")]
        public bool useAlphaBeta = true;
        public Text alphaTxt;
        public AnalogPointer[] alphaPointers;

        [Space]
        public Text betaTxt;
        public AnalogPointer[] betaPointers;

        [Space]
        public AnalogPointer[] ballPointers;

        [Space]
        public bool useGlidePath = false;
        public float glideXDeltaClamp = 600f, glideYDeltaClamp = 700f;
        public RectTransform glidePath;


        [Space(10)]
        [Header("Engine, Fuel and Temperature")]
        public bool useEngine = true;
        public Text engineTxt;
        public AnalogPointer[] enginePointers;
        public AnalogWheel[] engineWheels;


        [Space]
        public bool useFuel = true;
        public Text fuelTxt;
        public AnalogPointer[] fuelPointers;
        public AnalogWheel[] fuelWheels;

        [Space]
        public Text fuelFlowTxt;
        public AnalogPointer[] fuelFlowPointers;
        public AnalogWheel[] fuelFlowWheels;

        [Space]
        public bool useTemperature = true;
        public Text temperatureTxt;
        public AnalogPointer[] temperaturePointers;
        public AnalogWheel[] temperatureWheels;


        [Space(10)]
        [Header("Clock")]
        public bool useClock = true;
        
        public Text hourTxt;
        public AnalogPointer[] hourPointers;

        [Space]
        public Text minuteTxt;
        public AnalogPointer[] minutePointers;
        //


        // Context Menu
        [ContextMenu("SetCage")] void SetCage() { LoopCageChilds("Cage"); }
        [ContextMenu("SetCageShort")] void SetCageShort() { LoopCageChilds("Cage (short)"); }
        [ContextMenu("SetBebel")] void SetBebel() { LoopCageChilds("Bevel"); }
        [ContextMenu("SetBebelShort")] void SetBebelShort() { LoopCageChilds("Bevel (short)"); }
        [ContextMenu("SetNone")] void SetNone() { LoopCageChilds(""); }
        void LoopCageChilds(string activateString)
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                if(child.name == "Cage" || child.name == "Cage (short)" || child.name == "Bevel" || child.name == "Bevel (short)")
                {
                    if( string.IsNullOrWhiteSpace(activateString)) child.gameObject.SetActive(false);
                    else child.gameObject.SetActive(child.name == activateString);
                }                
            }

            print("Cage/Bebel Updated: " + (string.IsNullOrWhiteSpace(activateString) ? "None" : activateString));
        }
        [ContextMenu("SetFlatGlass")] void SetFlatGlass() { LoopGlassChilds("GlassFlat"); }
        [ContextMenu("SetBubbleGlass")] void SetBubbleGlass() { LoopGlassChilds("GlassBubble"); }
        [ContextMenu("SetNoGlass")] void SetNoGlass() { LoopGlassChilds(""); }

        void LoopGlassChilds(string activateString)
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                if (child.name == "GlassFlat" || child.name == "GlassBubble")
                {
                    if (string.IsNullOrWhiteSpace(activateString)) child.gameObject.SetActive(false);
                    else child.gameObject.SetActive(child.name == activateString);
                }
            }

            print("Glass Updated: " + (string.IsNullOrWhiteSpace(activateString) ? "None" : activateString));
        }

        // Context Menu


        // Current Instance
        void OnEnable() { current = this; }
        //


        //////////////////////////////////////////// Update
        void Update()
        {
            if (!isActive || !dataCenter ) return;



            //////////////////////////////////////////// Compass, Heading and/or HSI + Turn Rate
            if (useHeading)
            {
                if (headingTxt != null) { if (dataCenter.heading < 0) headingTxt.text = (dataCenter.heading + 360f).ToString("000"); else headingTxt.text = dataCenter.heading.ToString("000"); }
                for (int i = 0; i < compassWheels.Length; i++) if (compassWheels[i] != null) compassWheels[i].setValue( (dataCenter.heading < 0) ? (dataCenter.heading + 360f) : dataCenter.heading );
                for (int i = 0; i < compassPointers.Length; i++) if (compassPointers[i] != null) compassPointers[i].setValue(dataCenter.heading);
            }
            //
            if (useTurnRate)
            {
                if (turnRateTxt != null) { turnRateTxt.text = dataCenter.turnRate.ToString("0"); }
                for (int i = 0; i < turnRatePointers.Length; i++) if (turnRatePointers[i] != null) turnRatePointers[i].setValue(dataCenter.turnRate);
            }
            //////////////////////////////////////////// Compass, Heading and/or HSI + Turn Rate


            //////////////////////////////////////////// Roll
            if (useRoll)
            {
                if (horizonRollTxt != null)
                {
                    //horizonRollTxt.text = dataCenter.roll.ToString("##");
                    if (dataCenter.roll > 180) horizonRollTxt.text = (dataCenter.roll - 360).ToString("00");
                    else if (dataCenter.roll < -180) horizonRollTxt.text = (dataCenter.roll + 360).ToString("00");
                    else horizonRollTxt.text = dataCenter.roll.ToString("00");
                }
                for (int i = 0; i < horizonRollPointers.Length; i++) if (horizonRollPointers[i] != null) horizonRollPointers[i].setValue(dataCenter.rollAmplitude * dataCenter.roll);
            }
            //////////////////////////////////////////// Roll


            //////////////////////////////////////////// Pitch
            if (usePitch)
            {
                if (horizonPitchTxt != null) horizonPitchTxt.text = dataCenter.pitch.ToString("0");
                for (int i = 0; i < horizonPitchPointers.Length; i++) if (horizonPitchPointers[i] != null) horizonPitchPointers[i].setValue(dataCenter.pitchAmplitude * dataCenter.pitch);
                if (horizonPitchMaterial != null) horizonPitchMaterial.mainTextureOffset = new Vector2(horizonPitchMaterial.mainTextureOffset.x, (dataCenter.pitchAmplitude * dataCenter.pitch) / (90f * 2f));
            }
            //////////////////////////////////////////// Pitch


            //////////////////////////////////////////// Altitude + RadarAltitude
            if (useAltitude)
            {
                if (altitudeTxt != null) altitudeTxt.text = dataCenter.altitude.ToString("0").PadLeft(5);
                for (int i = 0; i < altitudeWheels.Length; i++) if (altitudeWheels[i] != null) altitudeWheels[i].setValue(dataCenter.altitude);
                for (int i = 0; i < altitudePointers.Length; i++) if (altitudePointers[i] != null) altitudePointers[i].setValue(dataCenter.altitude);
            }

            //RadarAltitude as a Separeted Instrument
            if (useRadarAltitude && useSeparatedRadarAltitude)
            {
                if (radarAltitudeTxt != null) radarAltitudeTxt.text = dataCenter.radarAltitude.ToString("0").PadLeft(5);
                for (int i = 0; i < radarAltWheels.Length; i++) if (radarAltWheels[i] != null) radarAltWheels[i].setValue(dataCenter.radarAltitude);
                for (int i = 0; i < radarAltPointers.Length; i++) if (radarAltPointers[i] != null) radarAltPointers[i].setValue(dataCenter.radarAltitude);
            }
            //////////////////////////////////////////// Altitude + RadarAltitude


            //////////////////////////////////////////// Speed
            if (useSpeed)
            {
                if (speedTxt != null) speedTxt.text = (nonNegativeSpeed) ? Mathf.Abs(dataCenter.speed).ToString("0").PadLeft(5) : dataCenter.speed.ToString("0").PadLeft(5);//.ToString("##0");
                for (int i = 0; i < speedWheels.Length; i++) if (speedWheels[i] != null) speedWheels[i].setValue( (nonNegativeSpeed) ? Mathf.Abs(dataCenter.speed) : dataCenter.speed);
                for (int i = 0; i < speedPointers.Length; i++) if (speedPointers[i] != null) speedPointers[i].setValue((nonNegativeSpeed) ? Mathf.Abs(dataCenter.speed) : dataCenter.speed);

                if (absSpeedTxt != null) absSpeedTxt.text = dataCenter.absSpeed.ToString("0").PadLeft(5);//.ToString("##0");
                for (int i = 0; i < absSpeedWheels.Length; i++) if (absSpeedWheels[i] != null) absSpeedWheels[i].setValue(Mathf.Abs(dataCenter.absSpeed));
                for (int i = 0; i < absSpeedPointers.Length; i++) if (absSpeedPointers[i] != null) absSpeedPointers[i].setValue(Mathf.Abs(dataCenter.absSpeed));
            }
            //////////////////////////////////////////// Speed


            //////////////////////////////////////////// Vertical Velocity - VV
            if (useVV)
            {
                if (verticalSpeedTxt != null)
                {
                    if (roundVV)
                    {
                        if (showDecimalVV) verticalSpeedTxt.text = (System.Math.Round(dataCenter.vv / roundFactorVV, System.MidpointRounding.AwayFromZero) * roundFactorVV).ToString("0.0").PadLeft(4);
                        else verticalSpeedTxt.text = (System.Math.Round(dataCenter.vv / roundFactorVV, System.MidpointRounding.AwayFromZero) * roundFactorVV).ToString("0").PadLeft(3);
                    }
                    else
                    {
                        if (showDecimalVV) verticalSpeedTxt.text = (dataCenter.vv).ToString("0.0").PadLeft(4);
                        else verticalSpeedTxt.text = (dataCenter.vv).ToString("0").PadLeft(3);
                    }
                }
                for (int i = 0; i < vvWheels.Length; i++) if (vvWheels[i] != null) vvWheels[i].setValue(dataCenter.vv);
                for (int i = 0; i < vvPointers.Length; i++) if (vvPointers[i] != null) vvPointers[i].setValue(dataCenter.vv);
            }
            //////////////////////////////////////////// Vertical Velocity - VV


            //////////////////////////////////////////// Horizontal Velocity - HV
            if (useHV)
            {
                if (horizontalSpeedTxt != null)
                {
                    if (roundHV)
                    {
                        if (showDecimalHV) horizontalSpeedTxt.text = (System.Math.Round(dataCenter.hv / roundFactorHV, System.MidpointRounding.AwayFromZero) * roundFactorHV).ToString("0.0").PadLeft(4);
                        else horizontalSpeedTxt.text = (System.Math.Round(dataCenter.hv / roundFactorHV, System.MidpointRounding.AwayFromZero) * roundFactorHV).ToString("0").PadLeft(3);
                    }
                    else
                    {
                        if (showDecimalHV) horizontalSpeedTxt.text = (dataCenter.hv).ToString("0.0").PadLeft(4);
                        else horizontalSpeedTxt.text = (dataCenter.hv).ToString("0").PadLeft(3);
                    }
                }
                for (int i = 0; i < hvWheels.Length; i++) if (hvWheels[i] != null) hvWheels[i].setValue(dataCenter.hv);
                for (int i = 0; i < hvPointers.Length; i++) if (hvPointers[i] != null) hvPointers[i].setValue(dataCenter.hv);
            }
            //////////////////////////////////////////// Horizontal Velocity - HV


            //////////////////////////////////////////// Vertical G-Force 
            if (useGForce)
            {
                if (gForceTxt != null) gForceTxt.text = dataCenter.gForce.ToString("0.0").PadLeft(3);
                if (dataCenter.gForce > dataCenter.maxGForce)
                {
                    dataCenter.maxGForce = dataCenter.gForce;
                    if (maxGForceTxt != null) maxGForceTxt.text = dataCenter.maxGForce.ToString("0.0").PadLeft(3);
                }
                if (dataCenter.gForce < dataCenter.minGForce)
                {
                    dataCenter.minGForce = dataCenter.gForce;
                    if (minGForceTxt != null) minGForceTxt.text = dataCenter.minGForce.ToString("0.0").PadLeft(3);
                }

                for (int i = 0; i < gForcePointers.Length; i++) if (gForcePointers[i] != null) gForcePointers[i].setValue(dataCenter.gForce);

                //// Lateral G-Force (Under Testing)
                //if (  betaArrow != null) betaArrow.setValue((gForceAmplitude * (relativeAccel.x - angularSpeed.y * Mathf.Abs(relativeSpeed.z))) / Physics.gravity.magnitude);
                ////

            }
            ////////////////////////////////////////////  Vertical G-Force 


            //////////////////////////////////////////////// AOA (Alpha) + AOS (Beta) + GlidePath (Velocity Vector)
            if (useAlphaBeta || useGlidePath)
            {
                if (useAlphaBeta)
                {
                    if (alphaTxt != null) alphaTxt.text = dataCenter.alpha.ToString("0").PadLeft(3);
                    for (int i = 0; i < alphaPointers.Length; i++) if (alphaPointers[i] != null) alphaPointers[i].setValue(dataCenter.alpha);

                    if (betaTxt != null) betaTxt.text = dataCenter.beta.ToString("0").PadLeft(3);
                    for (int i = 0; i < betaPointers.Length; i++) if (betaPointers[i] != null) betaPointers[i].setValue(dataCenter.beta);

                    for (int i = 0; i < ballPointers.Length; i++) if (ballPointers[i] != null) ballPointers[i].setValue(dataCenter.ball);                    
                }
            }
            //////////////////////////////////////////////// AOA (Alpha) + AOS (Beta)


            //////////////////////////////////////////// Engine & Fuel & Temperature
            if (useEngine)
            {
                if (engineTxt != null) engineTxt.text = dataCenter.engine.ToString("##0");
                for (int i = 0; i < engineWheels.Length; i++) if (engineWheels[i] != null) engineWheels[i].setValue(dataCenter.engine);
                for (int i = 0; i < enginePointers.Length; i++) if (enginePointers[i] != null) enginePointers[i].setValue(dataCenter.engine);
            }
            //
            if (useFuel)
            {
                if (fuelTxt != null) fuelTxt.text = dataCenter.fuel.ToString("##0");
                for (int i = 0; i < fuelWheels.Length; i++) if (fuelWheels[i] != null) fuelWheels[i].setValue(dataCenter.fuel);
                for (int i = 0; i < fuelPointers.Length; i++) if (fuelPointers[i] != null) fuelPointers[i].setValue(dataCenter.fuel);

                if (fuelFlowTxt != null) fuelFlowTxt.text = (dataCenter.fuelAmplitude * dataCenter.fuelFlow).ToString("##0.0");//.ToString("0.0").PadLeft(4);  //.ToString("##0"); 
                for (int i = 0; i < fuelFlowWheels.Length; i++) if (fuelFlowWheels[i] != null) fuelFlowWheels[i].setValue(dataCenter.fuelFlow);
                for (int i = 0; i < fuelFlowPointers.Length; i++) if (fuelFlowPointers[i] != null) fuelFlowPointers[i].setValue(dataCenter.fuelFlow);
            }
            //
            if (useTemperature)
            {
                if (temperatureTxt != null) temperatureTxt.text = dataCenter.temperature.ToString("##0");
                for (int i = 0; i < temperatureWheels.Length; i++) if (temperatureWheels[i] != null) temperatureWheels[i].setValue(dataCenter.temperature);
                for (int i = 0; i < temperaturePointers.Length; i++) if (temperaturePointers[i] != null) temperaturePointers[i].setValue(dataCenter.temperature / dataCenter.maxTemperature);
            }
            //////////////////////////////////////////// Engine & Fuel & Temperature



            ////////////////////////////////////////////// Clock
            if (useClock)
            {
                if (hourTxt != null) hourTxt.text = dataCenter.hour.ToString("#0");
                for (int i = 0; i < hourPointers.Length; i++) if (hourPointers[i] != null) hourPointers[i].setValue(dataCenter.hour + dataCenter.minute / 60f);

                if (minuteTxt != null) minuteTxt.text = dataCenter.minute.ToString("#0");
                for (int i = 0; i < minutePointers.Length; i++) if (minutePointers[i] != null) minutePointers[i].setValue(dataCenter.minute);                
            }
            ////////////////////////////////////////////// Clock


        }
        //////////////////////////////////////////// Update
    }
}
