using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float horizontalInput, verticalInput;
    public bool isBreaking,isBoost;
    // Start is called before the first frame update
    internal enum driver
    {
        keyboard,
        AI
    }
    [SerializeField] driver whoControlThis;
    private AiSystem track;
    public Transform currentWayPoint ;
    [Range(0,10)]public int distanceOffset;
    [Range(0,5)]public float steerForce;
    public List<Transform> nodes;
    void Start()
    {
        track = GameObject.FindGameObjectWithTag("tracking").GetComponent<AiSystem>();
        nodes = track.wayPoint;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        switch (whoControlThis)
        {
            case driver.AI:AIDrive();
                break;
            case driver.keyboard:KeyBoard();
                break;
        }
        
        
    }
    void AIDrive()
    {
        verticalInput = 1.2f;
        AISteer();
    }
    void AISteer()
    {
        DistanceForPoint();
        Vector3 relative = transform.InverseTransformPoint(currentWayPoint.transform.position).normalized;  
        horizontalInput = relative.x * steerForce;
    }
    void KeyBoard()
    {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        // Breaking Input
        isBreaking = (Input.GetAxis("Jump") !=0)? true: false;
        isBoost = Input.GetKeyDown(KeyCode.LeftShift);
    }
    private void DistanceForPoint()
    {
        Vector3 position = gameObject.transform.position;
        float closestDistance = Mathf.Infinity;
        for(int i =0;i<nodes.Count;i++)
        {
            float currentDistance = Vector3.Distance(nodes[i].position,position);
            if(currentDistance < closestDistance)
            {
                
                currentWayPoint = nodes[i + distanceOffset];    
                closestDistance = currentDistance;
            } 
            
        }
        
        
    }
    private void OnDrawGizmos()
    {
       if(currentWayPoint != null) Gizmos.DrawWireSphere(currentWayPoint.position,3f);
    }
}
