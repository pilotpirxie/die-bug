using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _speedMultiplier = 1f;


    [SerializeField] private KeyCode _up;
    [SerializeField] private KeyCode _down;
    [SerializeField] private KeyCode _left;
    [SerializeField] private KeyCode _right;
    
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 inputs = Vector3.zero;

        if (Input.GetKey(_up)) inputs.z = 1;
        if (Input.GetKey(_down)) inputs.z = -1;
        if (Input.GetKey(_right)) inputs.x = 1;
        if (Input.GetKey(_left)) inputs.x = -1;

        inputs = Vector3.ClampMagnitude(inputs, 1f);

        if (inputs.magnitude != 0)
        {
            transform.rotation = Quaternion.LookRotation (inputs);
  
            transform.Translate(inputs * _speed * _speedMultiplier, Space.World);
        }
    }
}
