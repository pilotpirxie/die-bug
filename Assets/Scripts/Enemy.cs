using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EnemyType {
    Crawling,
    Shooting,
    Turret
}

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private GameObject _playerTarget;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _rotateSpeed = 1f;
    [SerializeField] private Vector3 _positionNoise;
    [SerializeField] private float _maxNoise = 2f;
    
    [Header("Attack")]
    [SerializeField] private EnemyType _type;
    [SerializeField] private int _damage = 50;
    [SerializeField] private bool _destroyOnCollision = true;

    [Header("If shooting or turret type")]
    [SerializeField] private float _shootInterval = 1f;
    [SerializeField] private float _shootNoiseInterval = 0.25f;
    [SerializeField] private float _minDistanceFromPlayer = 10f;
    [SerializeField] private float _maxDistanceFromPlayer = 100f;
    [SerializeField] private GameObject _bullet;
    
    [Header("HP")] 
    [SerializeField] private int _maxHP = 100;
    [SerializeField] private int _currentHP;
    
    private void Start()
    {
        if (_type == EnemyType.Crawling)
        {
            InvokeRepeating("Noise", 1f, 1f / 2);
        }

        if (_type == EnemyType.Shooting || _type == EnemyType.Turret)
        {
            InvokeRepeating("Shoot", 1f, _shootInterval + Random.Range(-_shootNoiseInterval, _shootNoiseInterval));
        }

        RandomOnStart();

        _currentHP = _maxHP;
    }

    private void RandomOnStart()
    {
        Vector3 euler = transform.eulerAngles;
        euler.y = Random.Range(-360f, 360f);
        transform.eulerAngles = euler;
    }

    private void SelectPlayerTarget()
    {
        if (_playerTarget != null) return;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        _playerTarget = players[Random.Range(0, players.Length)];
    }

    private void FixedUpdate()
    {
        if (_currentHP < 0)
        {
            EnemyDie();
            return;
        }
        
        SelectPlayerTarget();

        Vector3 targetPosition = _playerTarget.transform.position + _positionNoise;

        if (_type == EnemyType.Crawling || _type == EnemyType.Shooting)
        {
            float distance = Vector3.Distance(transform.position, _playerTarget.transform.position);

            if ((_type == EnemyType.Shooting && distance > _minDistanceFromPlayer) || _type == EnemyType.Crawling)
            {
                transform.localPosition = Vector3.MoveTowards (transform.localPosition, targetPosition, _speed * Time.deltaTime);
            }
        }

        Vector3 targetDirection = targetPosition - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, _rotateSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    
    private void Noise()
    {
        _positionNoise = new Vector3(Random.Range(-_maxNoise, _maxNoise), 0, Random.Range(-_maxNoise, _maxNoise));
    }

    public int GetDamage()
    {
        return _damage;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();

            _currentHP -= bullet.GetDamage();
            bullet.DestroyBullet();
        }
    }

    public void CollideWithPlayer()
    {
        if (_destroyOnCollision)
        {
            Destroy(gameObject);
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(_bullet, transform.position, transform.rotation);
        bullet.GetComponent<Bullet>().SetDamage(_damage);
    }

    private void EnemyDie()
    {
        Destroy(gameObject);
    }
}
