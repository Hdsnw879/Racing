using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public Rigidbody rb;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        GameObject car = GameObject.FindWithTag("Player");
        rb = car.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float speed = rb.linearVelocity.magnitude * 3.6f;
        text.text = "speed: " + (int)speed;

    }
}
