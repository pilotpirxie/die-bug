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
            GameObject enemy = Instantiate(_enemyToSpawn, gameObject.transform.position + new Vector3(0, 3f, 0), gameObject.transform.rotation);
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("DestroyZone"))
        {
            Destroy(gameObject);
        }
    }
}
