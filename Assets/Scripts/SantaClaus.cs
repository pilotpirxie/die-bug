using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SantaClaus : MonoBehaviour
{
    [SerializeField] private float _speed = 0.2f;
    
    void Start()
    {
        Invoke("SpawnPresents", Random.Range(5, 15));
        Invoke("DestroySantaClaus", 25f);
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(-_speed, 0, 0);
    }

    void SpawnPresents()
    {
        Debug.Log("Present!");
    }
    
    void DestroySantaClaus()
    {
        Destroy(gameObject);
    }
}
