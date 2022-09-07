using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GleyTrafficSystem
{
    public class CarHazard : MonoBehaviour
    {
        public GameObject Car;
       // public Transform turning_point;
        public float speed;
        public float acce;
        
        private float steering;
      
        VehicleLightsComponent lightsComponent;

        public void Start()
        {
            lightsComponent = gameObject.GetComponent<VehicleLightsComponent>();
            lightsComponent.Initialize();
            speed = 5;
           

        }
        public void go_straight()// stage 1 acclerating to the intersection
        {
            Transform hazardcar = Car.transform;
            speed += Time.deltaTime * acce;
            hazardcar.Translate(transform.forward * Time.deltaTime * speed);
            lightsComponent.SetBlinker(BlinkType.BlinkRight);

        }
        public void left_turn()
        {
            lightsComponent.SetBlinker(BlinkType.BlinkRight);
            Transform hazardcar = Car.transform;
            hazardcar.Translate(hazardcar.forward * Time.deltaTime * speed);
            hazardcar.Rotate(hazardcar.up * Time.deltaTime * speed);
        }

        public float WithinTTA(Transform mycar,float Curspeed)
        {

            return CalculateTTA(mycar, Curspeed) ;
}
       private float CalculateTTA(Transform mycar,float Curspeed)
        {
            float distance = Mathf.Abs(mycar.position.x - transform.position.x);//x,y,z needs to be determined 
            return distance/(Curspeed+speed);
        }
    }
}