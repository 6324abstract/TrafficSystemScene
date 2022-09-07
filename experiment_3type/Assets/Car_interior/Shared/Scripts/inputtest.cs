/*public class AxleInfo
{
    public WheelCollider LeftWheel;
    public WheelCollider RightWheel;
    public bool motor;
    public bool sterring;//2 or 4 driven 
}
public class CarControl : MonoBehaviour 
{
    public List<AxleInfo> axleInfos;
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
        Debug.Log(steering);
        foreach(AxleInfo axleinfo in axleInfos)
        {
            if (axleinfo.motor)
            {
                axleinfo.LeftWheel.motorTorque = motor;
                axleinfo.RightWheel.motorTorque = motor;
            }
            if (axleinfo.sterring)
            {
                axleinfo.LeftWheel.steerAngle = -steering;
                axleinfo.RightWheel.steerAngle = -steering;
            }
        }
        
    }

}*/
