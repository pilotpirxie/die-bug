using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Meteor : MonoBehaviour
{
    [SerializeField] private GameObject _enemyToSpawn;
    [SerializeField] private GameObject _hitParticles;
    [SerializeField] private CameraController _cameraController;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _explosionAudioSource;
    //[SerializeField] private AudioClip _whooshSound;
    [SerializeField] private AudioClip _explosionSound;

    public void Start()
    {
        _audioSource.PlayDelayed(Random.Range(.5f, 2.5f));
        _cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }

    public void SetEnemy(GameObject enemyPrefab)
    {
        _enemyToSpawn = enemyPrefab;
    }

    private void FixedUpdate() 
    {
        if (transform.position.y < -150)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _explosionAudioSource.pitch = .5f;
            _explosionAudioSource.PlayOneShot(_explosionSound);
            
            _cameraController.Shake(-3f, 3f);
            Instantiate(_enemyToSpawn, gameObject.transform.position + new Vector3(0, 3f, 0), gameObject.transform.rotation);
            Instantiate(_hitParticles, gameObject.transform.position + new Vector3(0, 3f, 0), gameObject.transform.rotation);
            Destroy(gameObject, 2f);
        }
    }
}
