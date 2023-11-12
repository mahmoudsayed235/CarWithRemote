using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarDataController))]
public class CarController : MonoBehaviour
{
    //Car properities 
    private Rigidbody rigidbody;
    CarDataController carDataController;

    //inputs
    public float gasValue;
    public float motorPower = 300f;
    public float brakeValue;
    public float brakePower = 30000f;
    public float steeringValue;

    //speed 
    private float speed;

    public AnimationCurve steeringCurve;
    // Action controllers
    public UIButtonController gasPedal;
    public UIButtonController brakePedal;
    public UIButtonController rightBtn;
    public UIButtonController leftBtn;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        carDataController = gameObject.GetComponent<CarDataController>();
     
    }

    // Update is called once per frame
    void Update()
    {
        speed = rigidbody.velocity.magnitude;
        Getnput();
        ForceMotor();
        ForceSteering();
        ForceBrake();
        UpdateWheelCollider();
        ApplyWheelPositions();
    }
    void Getnput()
    {
        //update steering
        steeringValue = 0;
        //if(rightBtn!=null)
        if (rightBtn.isPressed)
        {
            steeringValue += rightBtn.pressValue;
        }
        //if (leftBtn != null)
        if (leftBtn.isPressed)
        {
            steeringValue -= leftBtn.pressValue;
        }

        //update gas
       
        gasValue = 0;
        //if(gasPedal!= null)
        if (gasPedal.isPressed)
        {
            gasValue += gasPedal.pressValue;
        }
        //if (brakePedal != null)
        if (brakePedal.isPressed)
        {
            gasValue -= brakePedal.pressValue;
        }


       
       
        //update breke
        float movingDirection = Vector3.Dot(transform.forward, rigidbody.velocity);
        if (movingDirection < -0.5f && gasValue > 0)
        {
            brakeValue = Mathf.Abs(gasValue);
        }
        else if (movingDirection > 0.5f && gasValue < 0)
        {
            brakeValue = Mathf.Abs(gasValue);
        }
        else
        {
            brakeValue = 0;
        }
    }
    void ForceMotor()
    {

        carDataController.RRWheel.motorTorque = motorPower * gasValue;
        carDataController.RLWheel.motorTorque = motorPower * gasValue;

    }
    void ForceSteering()
    {

        float steeringAngle = steeringValue * steeringCurve.Evaluate(speed);
        steeringAngle = Mathf.Clamp(steeringAngle, -90f, 90f);
        carDataController.FRWheel.steerAngle = steeringAngle;
        carDataController.FLWheel.steerAngle = steeringAngle;
    }
    void ForceBrake()
    {
        carDataController.FRWheel.brakeTorque = brakeValue * brakePower * 0.7f;
        carDataController.FLWheel.brakeTorque = brakeValue * brakePower * 0.7f;
        carDataController.RRWheel.brakeTorque = brakeValue * brakePower * 0.3f;
        carDataController.RLWheel.brakeTorque = brakeValue * brakePower * 0.3f;


    }
    void UpdateWheelCollider()
    {
        WheelHit[] wheelHits = new WheelHit[4];
        carDataController.FRWheel.GetGroundHit(out wheelHits[0]);
        carDataController.FLWheel.GetGroundHit(out wheelHits[1]);
        carDataController.RRWheel.GetGroundHit(out wheelHits[2]);
        carDataController.RLWheel.GetGroundHit(out wheelHits[3]);
    }
    void ApplyWheelPositions()
    {
        UpdateWheel(carDataController.FRWheel, carDataController.FRWheelMesh);
        UpdateWheel(carDataController.FLWheel, carDataController.FLWheelMesh);
        UpdateWheel(carDataController.RRWheel, carDataController.RRWheelMesh);
        UpdateWheel(carDataController.RLWheel, carDataController.RLWheelMesh);
    }
    
    void UpdateWheel(WheelCollider coll, MeshRenderer wheelMesh)
    {
        Quaternion quat;
        Vector3 position;
        coll.GetWorldPose(out position, out quat);
        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = quat;
    }
}
