using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xe1 : MonoBehaviour
{
    // Start is called before the first frame update
    private float currentSteerAngle, currentbreakForce,currentAcceleration;
    private bool playDone =true;
    private float maxSpeed=120  ;
    public float currentSpeed;
    //Car Audio
    public AudioSource carAudio,carSkid;
    
    private float minPitch=0.1f;
    // Settings
    private float acceleration=400, brakeForce=400, maxSteerAngle=20;
    private GameObject wCollider,effect;
    
    // Wheel Colliders 0=FL,1=FR,2=BL,3=BR
    private WheelCollider[] wheelCollider =new WheelCollider[4];
    // Wheels
    private Transform [] wheelMesh = new Transform [4];
    private ParticleSystem []smoke = new ParticleSystem [4];
    private ParticleSystem []boost = new ParticleSystem [2];
    private Rigidbody rb;
    private InputManager inputManager;
    private void Start()
    {
        GetObject();

    }
    private void GetObject()
    {
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        //Get Wheel Collider from Game Object 
        wCollider = transform.Find("Wheel_Collider").gameObject;
        wheelCollider[0] = wCollider.transform.Find("Wheel_FL").gameObject.GetComponent<WheelCollider>();
        wheelCollider[1] = wCollider.transform.Find("Wheel_FR").gameObject.GetComponent<WheelCollider>();
        wheelCollider[2] = wCollider.transform.Find("Wheel_BL").gameObject.GetComponent<WheelCollider>();
        wheelCollider[3] = wCollider.transform.Find("Wheel_BR").gameObject.GetComponent<WheelCollider>();
        //Get Wheel Mesh 
        wheelMesh[0] = transform.Find("WHEEL_FL").transform;
        wheelMesh[1] = transform.Find("WHEEL_FR").transform;
        wheelMesh[2] = transform.Find("WHEEL_RL").transform;
        wheelMesh[3] = transform.Find("WHEEL_RR").transform;
        //Get Particle
        effect = transform.Find("Effect").gameObject;
        smoke[0] = effect.transform.Find("SmokeFL").gameObject.GetComponent<ParticleSystem>();
        smoke[1] = effect.transform.Find("SmokeFR").gameObject.GetComponent<ParticleSystem>();
        smoke[2] = effect.transform.Find("SmokeBL").gameObject.GetComponent<ParticleSystem>();
        smoke[3] = effect.transform.Find("SmokeBR").gameObject.GetComponent<ParticleSystem>();
        

        //Get Audio Source
        carAudio = GetComponents<AudioSource>()[0];
        carSkid = GetComponents<AudioSource>()[1];
        carAudio.Play();
        carAudio.loop = true;
    }

    

    private void FixedUpdate() 
    {
        currentSpeed = rb.linearVelocity.magnitude*3.6f;
        Acceleration();
        TurnSystem();
        DiftEffect();
        UpdateWheels();
        GetFriction();
        EngineSound();
        BrakeSound();
        //Drift();
         
        
        //BoostUp();
    }
    public void Drift()
    {
        
        if(inputManager.isBreaking)
        {
            foreach(var wheel in wheelCollider)
            {
                WheelFrictionCurve frictionSideWay = wheel.sidewaysFriction;
                frictionSideWay.extremumSlip = .5f;
                wheel.sidewaysFriction = frictionSideWay;
            }
        }else
        {
            foreach(var wheel in wheelCollider)
            {
                WheelFrictionCurve frictionSideWay = wheel.sidewaysFriction;
                frictionSideWay.extremumSlip = 0.05f; 
                wheel.sidewaysFriction = frictionSideWay;
            }
        }
    }

    //Tăng tốc xe
    private void Acceleration() 
    {

        if(wheelCollider[0].isGrounded && inputManager.verticalInput != 0){
            currentAcceleration = inputManager.verticalInput * acceleration;
            if(currentSpeed > maxSpeed)
            {
                currentAcceleration = inputManager.verticalInput * acceleration/3;
            }
            wheelCollider[0].motorTorque = currentAcceleration + 50;
            wheelCollider[1].motorTorque = currentAcceleration + 50;
            wheelCollider[2].motorTorque = currentAcceleration - 50;
            wheelCollider[3].motorTorque = currentAcceleration - 50;
        }else
        {
            wheelCollider[0].motorTorque = 0;
            wheelCollider[1].motorTorque = 0;
            wheelCollider[2].motorTorque = 0;
            wheelCollider[3].motorTorque = 0;
        }
            currentbreakForce = inputManager.isBreaking ? brakeForce : 0f;
        ApplyBreaking();
    }
    
    //Tạo phanh cho xe
    private void ApplyBreaking() 
    {
        foreach(var brake in wheelCollider)
        {
            brake.brakeTorque = currentbreakForce;
        }
    }

    //Cho xe có thể rẽ trái, phải
    private void TurnSystem() 
    {
        currentSteerAngle = maxSteerAngle * inputManager.horizontalInput; 
        if(inputManager.horizontalInput > 0){
            wheelCollider[0].steerAngle = currentSteerAngle;
            wheelCollider[1].steerAngle = currentSteerAngle + 5;
        }else if(inputManager.horizontalInput < 0)
        {
            wheelCollider[0].steerAngle = currentSteerAngle + 5;
            wheelCollider[1].steerAngle = currentSteerAngle;
        }else
        {
            wheelCollider[0].steerAngle=0;
            wheelCollider[1].steerAngle=0;
        }
        
    }

    //Tạo chuyển động cho xe trở nên chân thật
    private void UpdateWheels() 
    {
        int i = 0;
        foreach(var collider in wheelCollider){
            UpdateSingleWheel(collider,wheelMesh[i]);
            i++;    
        }
    }
    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot; 
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    //Tạo hiệu ứng
    private void DiftEffect()
    {
        float currentSpeed = rb.linearVelocity.magnitude*3.6f;       
        if(inputManager.isBreaking && currentSpeed > 30 && wheelCollider[0].isGrounded)
        {
            foreach(var smoke in smoke){
                smoke.Emit(1);
            }
        }
    }
    private void GetFriction()
    {
        foreach(var collider in wheelCollider)
        {
            WheelHit wheelHit;
            collider.GetGroundHit(out wheelHit);

        }
    }
    void EngineSound()
    {
        float currentSpeed = rb.linearVelocity.magnitude * 3.6f;
        float pitchFromCar = rb.linearVelocity.magnitude / 60f;

        if(currentSpeed < 2)
        {
            carAudio.pitch = minPitch;
        }
        if(currentSpeed > 2)
        {
            carAudio.pitch = minPitch + pitchFromCar;
        }
    }
    private void BrakeSound()
    {
        float currentSpeed = rb.linearVelocity.magnitude*3.6f;
        if(inputManager.isBreaking && playDone && currentSpeed > 30 && wheelCollider[0].isGrounded)
        {
            carSkid.Play();
            playDone = false;
        }else if(inputManager.isBreaking == false)
        {
            carSkid.Stop();
            playDone = true;
        }else if(currentSpeed<20){
            carSkid.Stop();
        }
    }
    
}
