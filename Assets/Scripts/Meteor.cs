using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] private GameObject _enemyToSpawn;

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
            Instantiate(_enemyToSpawn, gameObject.transform.position + new Vector3(0, 3f, 0), gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }
}
