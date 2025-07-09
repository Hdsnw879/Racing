using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float moveSmoothness;
    public float rotSmoothness;

    public Vector3 moveOffset = new Vector3(0,2.2f,-5.56f);
    public Vector3 rotOffset;

    private Transform carTarget;
    void Awake()
    {
        
    }
    void Start()
    {
        GetObject(); 
    }
    void FixedUpdate()
    {
        if(carTarget != null)       
        FollowTarget();
    }

    void FollowTarget()
    {
        HandleMovement();
        HandleRotation();
    }
            
    void HandleMovement()
    {
        Vector3 targetPos ;
        targetPos = carTarget.TransformPoint(moveOffset);

        transform.position = Vector3.Lerp(transform.position, targetPos, moveSmoothness * Time.deltaTime);
    }

    void HandleRotation()
    {
        Vector3 direction = carTarget.position - transform.position + rotOffset;
        Quaternion rotation ;

        rotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation , rotSmoothness * Time.deltaTime);

    }
     
    
    
    void GetObject()
    {
        GameObject car = GameObject.FindWithTag("Player");
        carTarget = car.transform;
        FollowTarget();
    }

}   