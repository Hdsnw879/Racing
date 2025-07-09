using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSystem : MonoBehaviour
{
    public List<Transform> wayPoint;
    private void OnDrawGizmosSelected()
    {
        Transform [] path = GetComponentsInChildren<Transform>();
        wayPoint = new List<Transform>();
        for(int i=1;i<path.Length;i++)
        {
            wayPoint.Add(path[i]);
        }
        for(int i = 0;i<wayPoint.Count;i++)
        {
            Vector3 previousPoint = wayPoint[0].position;
            Vector3 currentPoint = wayPoint[i].position;
            if(i != 0)previousPoint = wayPoint[i-1].position;
            Gizmos.DrawLine(previousPoint,currentPoint);
            Gizmos.DrawSphere(currentPoint,3f);
            
            
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
