using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GleyTrafficSystem
{
    public class hazardcontrol : MonoBehaviour
    {
        private enum StateOfHazard
        {
            idle,
            pedestrian_triggered,
            vehicle_acce,
            vehicle_turn,
            vehicle_to_vanish
        }

        public GameObject Mycar;
        private Transform waypoint;
        private float cur;
        public float accel;
        private float max;
       // time it takes to collisde given the current speed
        public float InitialSpeed = 2f;
        public int Speed_rot = 60;
        public float steer_angle;
        public float turning;


        public Animator animator;
        public GameObject pedestrian;
        public GameObject carHazard;

        [SerializeField] private float TimeToArrival;
        private StateOfHazard curstage;
        
        private float TTAofleftturn=5f;
        private Rigidbody rb;//get the speed by rb.velocity

        //reaction time
        private static bool isTiming = false;
        private float timeTillKeyisPressed = 0;
        private static bool isManeuvered = false;
        public int numOfHazard = 0;
        private int numofPedestrainHazard = 0;
        private StateOfHazard[] states;
        VehicleLightsComponent lightsComponent;


        void Awake()
        {
            lightsComponent =carHazard.GetComponent<VehicleLightsComponent>();
            lightsComponent.Initialize();
            animator.GetComponent<Animator>();

            
            states = new StateOfHazard[] {StateOfHazard.pedestrian_triggered,StateOfHazard.vehicle_acce,StateOfHazard.pedestrian_triggered,StateOfHazard.pedestrian_triggered,StateOfHazard.pedestrian_triggered,StateOfHazard.idle};
            curstage =StateOfHazard.idle;
            carHazard.SetActive(false);
            rb = Mycar.GetComponent<Rigidbody>();
        }

        void Update()
        {
           
            float localVelocity = rb.velocity.magnitude;
            float tta;
          
             switch (curstage)
            {
                case (StateOfHazard.idle):
                    {
                        StateOfHazard curState = states[numOfHazard];
                        if (curState == StateOfHazard.pedestrian_triggered)
                        {
                            
                           tta = pedestrian.GetComponent<pedestriancontroller>().CalculateTTA(Mycar.transform, numofPedestrainHazard, localVelocity);

                            if (tta < TimeToArrival)
                            {
                                pedestrian.SetActive(true);
                                curstage = StateOfHazard.pedestrian_triggered;
                            }
                        }
                        else if (curState == StateOfHazard.vehicle_acce)
                        {
                            carHazard.SetActive(true);
                           tta=carHazard.GetComponent<CarHazard>().WithinTTA(Mycar.transform,localVelocity);
                            
                            if (tta<TTAofleftturn)
                                curstage = StateOfHazard.vehicle_acce;
                        }
                        break;
                    }
                case (StateOfHazard.pedestrian_triggered):
                    {
                        isTiming = true;
                        timeTillKeyisPressed += Time.deltaTime;
                        pedestrian.GetComponent<pedestriancontroller>().Wakling();
                        if (isManeuvered)
                        {
                            OutputReactionTime();
                            timeTillKeyisPressed = 0;
                            isTiming = false;
                            isManeuvered = false;
                            pedestrian.GetComponent<pedestriancontroller>().moveToNextPoint(++numofPedestrainHazard);
                            pedestrian.SetActive(false);
                            numOfHazard++;
                            curstage = StateOfHazard.idle;
                        }
                        break;
                    }
                case (StateOfHazard.vehicle_acce):
                    {
                       
                        lightsComponent.SetBlinker(BlinkType.BlinkRight);
                        lightsComponent.UpdateLights();
                        InitialSpeed += Time.deltaTime * accel;
                        carHazard.transform.Translate(transform.forward * Time.deltaTime * InitialSpeed);
                        if (carHazard.transform.position.x <turning)
                            curstage = StateOfHazard.vehicle_turn;
                         break;
                    }
                case (StateOfHazard.vehicle_turn):
                    {
                        lightsComponent.SetBlinker(BlinkType.BlinkRight);
                        lightsComponent.UpdateLights();
                        carHazard.transform.Translate(Vector3.forward * Time.deltaTime * InitialSpeed);
                        carHazard.transform.Rotate(Vector3.up * Time.deltaTime * Speed_rot);
                        isTiming = true;
                        Debug.Log("hazard activated" + numOfHazard);
                        timeTillKeyisPressed += Time.deltaTime;
                        if (isManeuvered)
                        {
                            OutputReactionTime();
                            timeTillKeyisPressed = 0;
                            isTiming = false;
                            isManeuvered = false;
                            
                            curstage = StateOfHazard.vehicle_to_vanish;
                        }
                        break;
                    }
                case (StateOfHazard.vehicle_to_vanish):
                    {
                        Destroy(carHazard);
                        curstage = StateOfHazard.idle;
                        numOfHazard++;
                        break;
                    }
             }
        }
      public static void ManuverByBrake()
        {
           if (isTiming)
                isManeuvered = true;
       }
      private void OutputReactionTime()
        {

            Debug.Log("reaction time:" + timeTillKeyisPressed + " hazard" + numOfHazard);
            Debug.Log("start" + (System.DateTime.Now.AddSeconds(-timeTillKeyisPressed)).ToString("ss.fff"));

        }
    }
}