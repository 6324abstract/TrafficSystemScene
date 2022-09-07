using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class CarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float maxSpeed;
    public float BrakeTorque;
    public GameObject Handle;
    public GameObject SpeedMeter;
    public GameObject RPMMeter;
   
    private float Brake_th = 0.15f;//in case of unexpected input 

  

    private Rigidbody m_Rigidbody;
    private  float speed;
    private bool isActivated;
    StreamWriter sw;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        LogitechGSDK.LogiSteeringInitialize(false);
        isActivated = false;
        //sw = new StreamWriter(@"" + "Steering" + ".csv", false);
    }
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }


    public void FixedUpdate()
    {
        speed = m_Rigidbody.velocity.magnitude;
        float speedx = m_Rigidbody.velocity.x;
        float brake=0;
       
        float accle = 0f;

        //改修した部分
        //InputManagerを経由せず、直接ペタルから数値を取得する方式
      
        //初期化
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);
             brake = -rec.lRz / 65536f + 0.5f;
           
            //rec.LYはアクセル、なぜか加速はマイナス、減速はプラスになる
            //rec.LRxはハンドル
            accle = -rec.lY / 32768f;
        }

        float motor = maxMotorTorque * accle;
        if (motor > 0)//press brake for the first time
            isActivated = true;
        //速度制限とバック防止
        if (speed > maxSpeed / 3.6f)
           m_Rigidbody.velocity = maxSpeed / 3.6f * m_Rigidbody.velocity.normalized;
       if(!isActivated)//avoid going backward at the begining
            m_Rigidbody.velocity = 0 * m_Rigidbody.velocity.normalized;

        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        

        Handle.transform.localEulerAngles = new Vector3(23, 0, Input.GetAxis("Horizontal") * 90);
        SpeedMeter.transform.localEulerAngles = new Vector3(12.214f, 0, speed * 4.5f);
        RPMMeter.transform.localEulerAngles = new Vector3(12.214f, 0, System.Math.Abs(accle) * 250);
        


        string[] str = { "" + Input.GetAxis("Horizontal") * 90, "" + UnityEngine.Time.time };
       

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
                if (brake > Brake_th)
                {
                    GleyTrafficSystem.hazardcontrol.ManuverByBrake();
                    axleInfo.leftWheel.brakeTorque = brake*BrakeTorque;
                    axleInfo.rightWheel.brakeTorque =brake*BrakeTorque;
                }
                else
                {
                    axleInfo.leftWheel.brakeTorque =0;
                    axleInfo.rightWheel.brakeTorque = 0;
                }
            
            }
            
            
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }

    }
}