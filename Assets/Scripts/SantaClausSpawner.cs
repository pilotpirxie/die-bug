using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaClausSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _santaClaus;
    
    void Start()
    {
        InvokeRepeating("SpawnSantaClaus", 5, 25);        
    }

    void SpawnSantaClaus()
    {
        Instantiate(_santaClaus, new Vector3(491, 208, Random.Range(260, 376)), Quaternion.identity);
    }
}
