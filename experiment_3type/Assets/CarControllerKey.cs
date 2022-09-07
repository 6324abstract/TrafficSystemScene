using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AxleInfoKey
{
    public WheelCollider LeftWheel;
    public WheelCollider RightWheel;
    public bool motor;
    public bool sterring;//2 or 4 driven 
}
public class CarControllerKey : MonoBehaviour
{
   public List<AxleInfoKey> axleInfos;
   private float maxMotorTorque;
   private float maxSteeringAngle;


    void Start()
    {
        maxMotorTorque = 300;
        maxSteeringAngle = 30;
    }
    private void FixedUpdate()
    {
        runTheCar();
    }
    public void runTheCar()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.A))
            GleyTrafficSystem.hazardcontrol.ManuverByBrake();
        foreach (AxleInfoKey axleinfo in axleInfos)
        {
            if (axleinfo.motor)
            {
                
                axleinfo.LeftWheel.motorTorque = motor;
                axleinfo.RightWheel.motorTorque = motor;
            }
            if (axleinfo.sterring)
            {
                axleinfo.LeftWheel.steerAngle = steering;
                axleinfo.RightWheel.steerAngle = steering;
            }
        }

    }

}
