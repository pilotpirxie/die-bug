using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] private GameObject _enemyToSpawn;
    [SerializeField] private GameObject _hitParticles;
    [SerializeField] private CameraController _cameraController;

    public void Start()
    {
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
            _cameraController.Shake(-3f, 3f);
            Instantiate(_enemyToSpawn, gameObject.transform.position + new Vector3(0, 3f, 0), gameObject.transform.rotation);
            Instantiate(_hitParticles, gameObject.transform.position + new Vector3(0, 3f, 0), gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }
}
