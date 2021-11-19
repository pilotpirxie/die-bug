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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            GameObject enemy = Instantiate(_enemyToSpawn);
            Destroy(gameObject);
        }
    }
}
