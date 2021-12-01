using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SantaClaus : MonoBehaviour
{
    [SerializeField] private float _speed = 0.2f;
    [SerializeField] private List<GameObject> _presents;
    [SerializeField] private GameObject _healObject;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _santaSound;

    void Start()
    {
        Invoke("SpawnPresents", Random.Range(7.5f, 15));
        
        Invoke("DestroySantaClaus", 25f);
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(-_speed, 0, 0);
    }

    void SpawnPresents()
    {
        _audioSource.PlayOneShot(_santaSound);
        
        Instantiate(_presents[Random.Range(0, _presents.Count)], transform.position, transform.rotation);
        Instantiate(_presents[Random.Range(0, _presents.Count)], transform.position, transform.rotation);
        Instantiate(_healObject, transform.position, transform.rotation);
    }
    
    void DestroySantaClaus()
    {
        Destroy(gameObject);
    }
}
