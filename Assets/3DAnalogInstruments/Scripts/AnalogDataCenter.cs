using UnityEngine;

namespace MGAssets
{
    public class AnalogDataCenter : MonoBehaviour
    {
        public static AnalogDataCenter current;

        [Header("--- Data Config ---")]
        public bool isActive = true;
        [Tooltip("Link your Aircraft Transform here (CG recommended) or leave it empty for tracking the MainCamera's Transform movement instead.")] public Transform aircraft;
                     
        //
        [Space(5)]
        [Header("Roll")]
        public bool useRoll = true;
        public float rollAmplitude = -1, rollOffSet = 0;
        [Range(0, 1)] public float rollFilterFactor = 0.25f;

        [Space(5)]
        [Header("Pitch")]
        public bool usePitch = true;
        public float pitchAmplitude = 1, pitchOffSet = 0;
        [Range(0, 1)] public float pitchFilterFactor = 0.125f;

        [Space(5)]
        [Header("Heading & TurnRate")]
        public bool useHeading = true;
        public float headingAmplitude = 1, headingOffSet = 0;
        [Range(0, 1)] public float headingFilterFactor = 0.1f;


        [Space]
        public bool useTurnRate = true;
        public float turnRateAmplitude = 1, turnRateOffSet = 0;
        [Range(0, 1)] public float turnRateFilterFactor = 0.1f;


        [Space(5)]
        [Header("Altitude")]
        public bool useAltitude = true;
        public float altitudeAmplitude = 1, altitudeOffSet = 0;
        [Range(0, 1)] public float altitudeFilterFactor = 0.05f;

        [Space]
        public bool useRadarAltitude = true;
        public LayerMask radarLayer; // LayerMask.NameToLayer("Default");
        public bool absoluteMode = true;
        public float maxRadarAltitude = 1400;
        public int radarFPS = 15;
        public bool useSeparetedRadarAltitude = true;


        [Space(5)]
        [Header("AirSpeed")]
        public bool useSpeed = true;
        public float speedAmplitude = 1, speedOffSet = 0;
        [Range(0, 1)] public float speedFilterFactor = 0.25f;


        [Space(5)]
        [Header("Vertical Velocity")]
        public bool useVV = true;
        public float vvAmplitude = 1, vvOffSet = 0;
        [Range(0, 1)] public float vvFilterFactor = 0.1f;

        [Space(5)]
        [Header("Horizontal Velocity")]
        public bool useHV = true;
        public float hvAmplitude = 1, hvOffSet = 0;
        [Range(0, 1)] public float hvFilterFactor = 0.1f;


        [Space(5)]
        [Header("G-Force")]
        public bool useGForce = true;
        public float gForceAmplitude = 1, gForceOffSet = 0;
        [Range(0, 1)] public float gForceFilterFactor = 0.25f;


        [Space(5)]
        [Header("AOA, AOS and GlidePath")]
        public bool useAlphaBeta = true;
        public float alphaAmplitude = 1, alphaOffSet = 0;
        [Range(0, 1)] public float alphaFilterFactor = 0.25f;

        [Space]
        public float betaAmplitude = 1;
        public float betaOffSet = 0;
        [Range(0, 1)] public float betaFilterFactor = 0.25f;

        [Space]
        public float ballSensitivity = 0.01f;
        [Range(0, 1)] public float ballFilterFactor = 0.25f;

        [Space]
        public bool useGlidePath = false;
        [Range(0, 1)] public float glidePathFilterFactor = 0.1f;
        public float glideXDeltaClamp = 600f, glideYDeltaClamp = 700f;


        [Space(5)]
        [Header("Engine and Fuel")]
        public bool useEngine = true;
        public float engineAmplitude = 1;
        [Range(-1, 1)] public float engineOffSet = 0;
        [Range(0, 1)] public float engineFilterFactor = 0.05f;

        [Space]
        public bool useFuel = true;
        public float fuelAmplitude = 1;
        [Range(0, 1)] public float fuelFilterFactor = 0.0125f;

        [Space]
        public float fuelFlowAmplitude = 1;


        [Space(5)]
        [Header("Temperature")]
        public bool useTemperature = false;
        public float temperatureAmplitude = 1.2f, temperatureOffSet = 0;
        [Range(0, 1)] public float temperatureFilterFactor = 0.25f;

        [Space(5)]
        [Header("Flaps & Gear")]
        public bool useFlaps = false;
        [Range(0, 1)] public float flapsFilterFactor = 0.05f;

        [Space]
        public bool useGear = false;
        public bool useBrake = false;


        [Space(5)]
        [Header("Clock")]
        public bool useClock = true;
        public bool systemTime = true;

        //


        //
        [Space]
        [Header("--- Manual Controlers ---")]

        [Space]
        public bool gearDown = false;
        public bool gearBrake = false;

        [Space]
        public int flapsIndex = 0;
        public string[] flapsAngles = new string[4] { "0", "10", "15", "25" };

        [Space]
        [Tooltip("If True, Engine RPM will be calculated automaticaly.")] public bool autoRPM = true;
        public float maxEngine = 1, maxSpeed = 250;
        [Range(0, 1)] public float engineTarget = 0.75f;
        public float idleEngine = 0.25f, criticalEngine = 0.90f;
        public AudioSource EngineAS;
        public float minPitch = 0.25f, maxPitch = 2.0f;

        [Space]
        [Tooltip("Set it to False if you wish to manually control Temperature value")] public bool autoTemperature = false;
        public float maxTemperature = 1.2f;
        [Range(0, 1)] public float temperatureTarget = 0.5f;
        public float idleTemperature = 0.35f;
        [Tooltip("Multiplier to how fast the Temperature increases and decreases (Default 1)")] public float tempFlow = 1f;

        [Space]
        [Tooltip("If False, you can set manually the value for Fuel, otherwise it will be automatically controlled by the script.")]
        public bool autoFuel = true;
        public float maxFuel = 1;
        [Range(0, 3)] public float fuelTarget = 0.8f;

        [Tooltip("The Time (in Minutes) to consume 100% Fuel when Engine running at Iddle RPM - Set to 0 for no fuel consumption")]
        public float fuelMaxTime = 8f;
        [Tooltip("The Time (in Minutes) taken to consume 100% Fuel with Engine running at max RPM - Set to 0 for no fuel consumption")]
        public float fuelMinTime = 1.6666666f;
        //


        ////// All Flight Variables
        [Space(5)]
        [Header("Flight Variables - ReadOnly!")]
        [Space(10)]
        public float speed;
        public float absSpeed;
        public float altitude, radarAltitude;
        public float pitch, roll, heading, turnRate;
        public float gForce, maxGForce, minGForce;
        public float alpha, beta, ball, vv, hv;
        public float engine, fuel, fuelFlow, temperature;
        public float flaps, gear, brake;
        public float hour, minute;
        ////// All Flight Variables


        // Internal Calculation Variables
        [HideInInspector] public Vector3 currentPosition, lastPosition, relativeSpeed, absoluteSpeed, lastSpeed, relativeAccel;

        [HideInInspector] public Vector3 angularSpeed;
        Quaternion currentRotation, lastRotation, deltaTemp;
        float angleTemp = 0.0f;
        Vector3 axisTemp = Vector3.zero;

        float engineReNormalized, fuelReNormalized;
        int flapTarget;

        int waitInit = 6;

        int radarFrames = 15;
        float lastRadarAltitude = 0;
        //


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////// Initialization
        void Awake() 
        {
            if (!aircraft) aircraft = Camera.main.transform;
            if (!aircraft) aircraft = GetComponent<Transform>();
            if (useRadarAltitude && radarLayer.value == 0) radarLayer.value = 1;
        }
        void OnEnable() 
        {
            current = this;

            if (!aircraft) aircraft = Camera.main.transform;
            if (!aircraft) aircraft = GetComponent<Transform>(); 
            ResetData(); 
        }
        public void ResetData()
        {
            waitInit = 6;

            //Values to Reset        
            if (useGForce) { gForce = 0; maxGForce = 0f; minGForce = 0f; }
            if (useRadarAltitude) lastRadarAltitude = 0;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////// Initialization








        /////////////////////////////////////////////////////// Calculations
        void FixedUpdate()
        {
            // Return if not active
            if (!isActive) return;

            //////////////////////////////////////////// Frame Calculations
            lastPosition = currentPosition;
            lastSpeed = relativeSpeed;
            lastRotation = currentRotation;

            if (aircraft != null)
            {
                currentPosition = aircraft.transform.position;
                absoluteSpeed = (currentPosition - lastPosition) / Time.fixedDeltaTime;
                relativeSpeed = aircraft.transform.InverseTransformDirection((currentPosition - lastPosition) / Time.fixedDeltaTime);
                relativeAccel = (relativeSpeed - lastSpeed) / Time.fixedDeltaTime;
                currentRotation = aircraft.transform.rotation;

                //angular speed
                deltaTemp = currentRotation * Quaternion.Inverse(lastRotation);
                angleTemp = 0.0f;
                axisTemp = Vector3.zero;
                deltaTemp.ToAngleAxis(out angleTemp, out axisTemp);
                //
                angularSpeed = aircraft.InverseTransformDirection(angleTemp * axisTemp) * Mathf.Deg2Rad / Time.fixedDeltaTime;
                //
            }
            //////else if (aircraftRB != null)  //Mode RB
            //////{
            //////    currentPosition = aircraftRB.transform.position;
            //////    absoluteSpeed = (currentPosition - lastPosition) / Time.fixedDeltaTime;
            //////    relativeSpeed = aircraftRB.transform.InverseTransformDirection(aircraftRB.velocity);
            //////    relativeAccel = (relativeSpeed - lastSpeed) / Time.fixedDeltaTime;
            //////    currentRotation = aircraft.transform.rotation;
            //////    angularSpeed = aircraftRB.angularVelocity;
            //////}
            else //Zero all values
            {
                currentPosition = Vector3.zero;
                relativeSpeed = Vector3.zero;
                relativeAccel = Vector3.zero;
                angularSpeed = Vector3.zero;

                lastPosition = currentPosition;
                lastSpeed = relativeSpeed;
                lastRotation = currentRotation;
            }
            //

            //Wait some frames for stablization before starting calculating
            if (waitInit > 0) { waitInit--; return; }
            //
            //////////////////////////////////////////// Frame Calculations






            //////////////////////////////////////////// Pitch + Roll + Yaw (Heading) + TurnRate
            if (useRoll) roll = Mathf.LerpAngle(roll, currentRotation.eulerAngles.z + rollOffSet, rollFilterFactor) % 360;
            if (usePitch) pitch = Mathf.LerpAngle(pitch, pitchAmplitude  * - currentRotation.eulerAngles.x + pitchOffSet, pitchFilterFactor);
            if (useHeading) heading = Mathf.LerpAngle(heading, headingAmplitude * currentRotation.eulerAngles.y + headingOffSet, headingFilterFactor) % 360f;
            if (useTurnRate)
            {
                //Mode: World Coordinates
                //turnRate = Mathf.LerpAngle(turnRate, turnRateOffSet + turnRateAmplitude * Mathf.DeltaAngle(lastRotation.eulerAngles.y, currentRotation.eulerAngles.y) / Time.fixedDeltaTime, turnRateFilterFactor) % 360f;
                //turnRate = Mathf.Round(100f * turnRate + 0.5f) / 100f;

                //Mode: Relative to Aircraft (Default)
                turnRate = Mathf.LerpAngle(turnRate, turnRateOffSet + turnRateAmplitude * (angularSpeed.y - 0.05f * angularSpeed.z) * Mathf.Rad2Deg, turnRateFilterFactor) % 360f;
                turnRate = Mathf.Round(100f * turnRate + 0.5f) / 100f;
                //////

                if (float.IsNaN(turnRate)) turnRate = 0;
            }
            //////////////////////////////////////////// Pitch + Roll + Yaw (Heading) + TurnRate


            //////////////////////////////////////////// Altitude + RadarAltitude
            if (useAltitude)
            {
                if (!useRadarAltitude || (useRadarAltitude && useSeparetedRadarAltitude)) altitude = Mathf.Lerp(altitude, altitudeOffSet + altitudeAmplitude * currentPosition.y, altitudeFilterFactor);
                else
                {
                    radarFrames--;
                    if(radarFrames <= 0)
                    {
                        radarFrames = radarFPS;
                        RaycastHit hit;
                        if (Physics.Linecast(aircraft.position, (absoluteMode) ? aircraft.position + Vector3.down * maxRadarAltitude : aircraft.position - aircraft.transform.up * maxRadarAltitude, out hit, radarLayer))
                            lastRadarAltitude = hit.distance;
                        else lastRadarAltitude = maxRadarAltitude;
                    }

                    radarAltitude = Mathf.Lerp(radarAltitude, altitudeOffSet + altitudeAmplitude * lastRadarAltitude, altitudeFilterFactor);
                    altitude = radarAltitude;
                }
            }

            //RadarAltitude as a Separeted Instrument
            if (useRadarAltitude && useSeparetedRadarAltitude)
            {
                radarFrames--;
                if(radarFrames <= 0)
                {
                    radarFrames = radarFPS;
                    RaycastHit hit;
                    if (Physics.Linecast(aircraft.position, (absoluteMode) ? aircraft.position + Vector3.down * maxRadarAltitude : aircraft.position - aircraft.transform.up * maxRadarAltitude, out hit, radarLayer))
                        lastRadarAltitude = hit.distance;
                    else lastRadarAltitude = maxRadarAltitude;
                }

                radarAltitude = Mathf.Lerp(radarAltitude, altitudeOffSet + altitudeAmplitude * lastRadarAltitude, altitudeFilterFactor);
            }
            //////////////////////////////////////////// Altitude + RadarAltitude


            //////////////////////////////////////////// Speed + Horizontal-Vertical Velocity (HV + VV)
            if (useSpeed)
            {
                speed = Mathf.Lerp(speed, speedOffSet + speedAmplitude * relativeSpeed.z, speedFilterFactor);
                absSpeed = Mathf.Lerp(absSpeed, speedOffSet + speedAmplitude * absoluteSpeed.magnitude, speedFilterFactor);
            }
            if (useVV) vv = Mathf.Lerp(vv, vvOffSet + vvAmplitude * absoluteSpeed.y, vvFilterFactor);
            if (useHV) hv = Mathf.Lerp(hv, hvOffSet + hvAmplitude * relativeSpeed.x, hvFilterFactor);
            //////////////////////////////////////////// Speed + Horizontal-Vertical Velocity (HV + VV)


            //////////////////////////////////////////// Vertical G-Force 
            if (useGForce)
            {
                // G-FORCE -> Gravity + Vertical Acceleration + Centripetal Acceleration (v * w) radians
                float gTotal = 0;
                gTotal =
                    ((-aircraft.transform.InverseTransformDirection(Physics.gravity).y +
                    gForceAmplitude * (relativeAccel.y - angularSpeed.x * Mathf.Abs(relativeSpeed.z)
                    )) / Physics.gravity.magnitude);

                gForce = Mathf.Lerp(gForce, gForceOffSet + gTotal, gForceFilterFactor);
                if (float.IsNaN(gForce)) gForce = 0;
                //

                // Set Max-Min G-Force values to Gui and Instruments
                if (gForce > maxGForce) maxGForce = gForce;
                if (gForce < minGForce) minGForce = gForce;
                //
            }
            ////////////////////////////////////////////  Vertical G-Force 


            //////////////////////////////////////////////// AOA (Alpha) + AOS (Beta) + GlidePath (Velocity Vector)
            if (useAlphaBeta || useGlidePath)
            {
                alpha = Mathf.Lerp(alpha, alphaOffSet + alphaAmplitude * Vector2.SignedAngle(new Vector2(relativeSpeed.z, relativeSpeed.y), Vector2.right), alphaFilterFactor);
                beta = Mathf.Lerp(beta, betaOffSet + betaAmplitude * Vector2.SignedAngle(new Vector2(relativeSpeed.x, relativeSpeed.z), Vector2.up), betaFilterFactor);

                ball = Mathf.Lerp(ball, Mathf.Clamp01(Mathf.Abs(ballSensitivity * absSpeed / speedAmplitude)) * beta, ballFilterFactor);
            }
            //////////////////////////////////////////////// AOA (Alpha) + AOS (Beta)


            //////////////////////////////////////////// Engine & Fuel
            if (useEngine)
            {
                //Auto RPM control and Fuel Condition
                if (autoRPM) engineTarget = Mathf.Abs(idleEngine / engineAmplitude + absSpeed / maxSpeed * (1 - idleEngine / engineAmplitude));
                if (useFuel && fuelReNormalized < 0.01f) engineTarget = 0;
                //

                //Updates current Engine RPM
                engineTarget = Mathf.Clamp01(Mathf.Abs(engineTarget));
                engine = Mathf.Lerp(engine, engineAmplitude * Mathf.Clamp01(engineTarget + engineOffSet), engineFilterFactor);

                if (engineTarget == 0 && engine < 0.01f) engine = 0;
                engineReNormalized = Mathf.Clamp01((engine - engineOffSet) / engineAmplitude);

                if (useFuel && fuel == 0) { engineTarget = 0; engine = 0; /*engineReNormalized = 0;*/ }
                //

                //Engine Sound and Pitch
                if (EngineAS != null && EngineAS.isActiveAndEnabled)
                {
                    if (!EngineAS.isPlaying && engineTarget > 0) EngineAS.Play();

                    if (engineReNormalized > 0.01f) EngineAS.pitch = Mathf.Lerp(minPitch, maxPitch, engineReNormalized);
                    else { EngineAS.Stop(); EngineAS.pitch = 1; }
                }
                //
            }
            //
            if (useFuel)
            {
                //Calculates Fuel Consumption
                if (autoFuel)
                {
                    if (fuelMaxTime > 0 || fuelMinTime > 0)
                    {
                        if (engine != 0)
                        {
                            //Consumption in Minutes for 100% Fuel
                            fuelFlow = Time.fixedDeltaTime / (60f * (fuelMaxTime - engineReNormalized * engineReNormalized * (fuelMaxTime - fuelMinTime)));
                            fuelTarget -= fuelFlow;
                        }
                        else fuelFlow = 0;
                    }
                    else fuelFlow = 0;
                }
                //

                //Updates current Fuel value
                if (fuelTarget < 0) fuelTarget = 0; //fuelTarget = Mathf.Clamp01(fuelTarget);
                fuel = Mathf.Lerp(fuel, fuelAmplitude * fuelTarget, fuelFilterFactor);

                if (fuel < 0) fuel = 0;
                if (fuelTarget == 0 && fuel < 0.01f) fuel = 0;
                fuelReNormalized = fuel / fuelAmplitude;
                //
            }
            //////////////////////////////////////////// Engine & Fuel


            //////////////////////////////////////////// Temperature
            if (useTemperature)
            {
                //Automatic Temperature control
                if (autoTemperature)
                {
                    if (useFuel && fuel == 0) temperatureTarget += 3 * (0 - temperatureTarget) * tempFlow * Time.fixedDeltaTime / 60f; //temperatureTarget = 0;
                    else
                    {
                        // Backup of simpler versions
                        //////temperatureTarget += factor * (engineTarget - temperatureTarget) * tempFlow * Time.fixedDeltaTime / 60f;
                        //int factor = (engineTarget < temperatureTarget) ? 2 : 1; //Cools 2 times faster than it heats
                        //temperatureTarget += factor * (engineTarget - temperatureTarget + idleTemperature / temperatureAmplitude) * tempFlow * Time.fixedDeltaTime / 60f;

                        if (engineReNormalized >= criticalEngine / engineAmplitude)
                            temperatureTarget += 1.5f * engineReNormalized * tempFlow * Time.fixedDeltaTime / 60f;
                        else if (engineTarget == 0)
                            temperatureTarget += 3 * (0 - temperatureTarget) * tempFlow * Time.fixedDeltaTime / 60f;
                        else if (engineReNormalized < idleTemperature / temperatureAmplitude || engineReNormalized < temperatureTarget)
                            temperatureTarget += 3 * (idleTemperature / temperatureAmplitude - temperatureTarget) * tempFlow * Time.fixedDeltaTime / 60f;
                        else
                            temperatureTarget += 1 * (engineReNormalized - temperatureTarget) * tempFlow * Time.fixedDeltaTime / 60f;
                    }
                }
                //

                //Updates current Engine Temperature
                temperatureTarget = Mathf.Clamp(temperatureTarget, 0, maxTemperature);
                temperature = Mathf.Lerp(temperature, temperatureAmplitude * temperatureTarget + temperatureOffSet, temperatureFilterFactor);
            }
            //////////////////////////////////////////// Temeprature



            ////////////////////////////////////////////// Flaps
            if (useFlaps)
            {
                //Update Target Flap Position
                if (flapTarget != flapsIndex)
                {
                    flapsIndex = Mathf.Clamp(flapsIndex, 0, flapsAngles.Length - 1);
                    flapTarget = flapsIndex;
                }

                //Normalized Value of Flaps
                flaps = Mathf.Lerp(flaps, (float)(flapsIndex) / (float)Mathf.Clamp((flapsAngles.Length - 1), 1, flapsAngles.Length), flapsFilterFactor);
                if (flaps <= 0.01f) flaps = 0; else if (flaps >= 0.99f) flaps = 1;
            }
            ////////////////////////////////////////////// Flaps


            ////////////////////////////////////////////// Gear
            if (useGear)
            {
                //Gear Changes
                if (gearDown && gear == 0) gear = 1;
                else if (!gearDown && gear == 1) gear = 0;
            }

            if (useBrake)
            {
                if (gearBrake && brake == 0) brake = 1;
                else if (!gearBrake && brake == 1) brake = 0;
            }
            ////////////////////////////////////////////// Gear


            ////////////////////////////////////////////// Clock
            if (useClock)
            {
                if (systemTime)
                {
                    hour = System.DateTime.Now.Hour;
                    minute = System.DateTime.Now.Minute;
                }
                else
                {
                    hour += Time.deltaTime / 3600f;
                    minute += Time.deltaTime / 60f;
                }
            }
            ////////////////////////////////////////////// Clock

        }
        /////////////////////////////////////////////////////// Calculations





    }
}