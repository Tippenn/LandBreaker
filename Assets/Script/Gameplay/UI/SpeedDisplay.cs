using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedDisplay : MonoBehaviour
{
    public TMP_Text text;
    public Rigidbody rb;
    public float speed;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        rb = GameObject.Find("Character").GetComponent<Rigidbody>();

    }
    void Start()
    {
       
    }
    
    void Update()
    {
        speed = rb.velocity.magnitude;
        text.text = "Speed: " + speed;
    }
}
