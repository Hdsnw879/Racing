using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginScene : MonoBehaviour
{
    public Color lineColor;
    public List<Transform> node ;
    private GameObject cameraBegin;
    private float journeyTime =3f;
    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;
        Transform [] path = GetComponentsInChildren<Transform>();
        node = new List<Transform>();
        for(int i = 1; i<path.Length;i++)
        {   
            node.Add(path[i]);
        }
        for(int i=0; i<node.Count;i++)
        {
            Vector3 currentPoint = node[i].position;
            Vector3 previousPoint = node[0].position ;
            if(i != 0)
            {
                previousPoint = node[i-1].position;
                Gizmos.DrawLine(previousPoint, currentPoint);
            }
            Gizmos.DrawSphere(currentPoint,5f);
        }
    }
    void Start()
    {
        cameraBegin = GameObject.Find("CameraBegin");
        StartCoroutine(MoveCamera());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator MoveCamera()
    {
        for (int i = 0; i < node.Count - 2; i++)
        {
            Vector3 lookPoint = node[i].position;
            Vector3 currentPoint = node[i + 1].position;
            Vector3 nextPoint = node[i + 2].position;
            

            if(i==0){cameraBegin.transform.position = currentPoint;}
            

            float elapsedTime = 0;

            while (elapsedTime < journeyTime)
            {
                cameraBegin.transform.position = Vector3.Lerp(currentPoint, nextPoint, elapsedTime / journeyTime);
                cameraBegin.transform.LookAt(lookPoint);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
        }
    }
}
