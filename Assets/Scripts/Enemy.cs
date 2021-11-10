using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _playerTarget;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _rotateSpeed = 1f;
    [SerializeField] private Vector3 _positionNoise;
    [SerializeField] private float _maxNoise = 2f;
    
    private void Start()
    {
        InvokeRepeating("Noise", 1f, 1f / 2);
        RandomOnStart();
    }

    private void RandomOnStart()
    {
        Vector3 euler = transform.eulerAngles;
        euler.y = Random.Range(-360f, 360f);
        transform.eulerAngles = euler;
    }

    private void SelectPlayerTarget()
    {
        if (_playerTarget == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            int x = Random.Range(0, players.Length);
            _playerTarget = players[x];
        }
    }

    private void Update()
    {
        SelectPlayerTarget();

        Vector3 targetPosition = _playerTarget.transform.position + _positionNoise;
        
        transform.localPosition = Vector3.MoveTowards (transform.localPosition, targetPosition, _speed * Time.deltaTime);
        Vector3 targetDirection = targetPosition - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, _rotateSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void Noise()
    {
        _positionNoise = new Vector3(Random.Range(-_maxNoise, _maxNoise), 0, Random.Range(-_maxNoise, _maxNoise));
    }
}
