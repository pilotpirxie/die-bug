using UnityEngine;

public enum EnemyType
{
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
    [SerializeField] private bool _isSpawner;
    [SerializeField] private GameObject _enemySpawn;
    
    [Header("If shooting or turret type")] 
    [SerializeField] private float _shootInterval = 1f;
    [SerializeField] private float _shootNoiseInterval = 0.25f;
    [SerializeField] private float _minWalkDistanceFromPlayer = 10f;
    [SerializeField] private float _maxShootingDistanceFromPlayer = 100f;
    [SerializeField] private GameObject _bullet;

    [Header("HP")] 
    [SerializeField] private int _maxHp = 100;
    [SerializeField] private int _currentHp;
    [SerializeField] private GameObject _deathParticle;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private AudioClip _laserSound;

    [SerializeField] private float _scoreWorth;

    [Header("Controllers")]
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Animator _animatorController;
    [SerializeField] private GameplayController _gameplayController;
    
    private bool _isDead = false;
    private void Start()
    {
        _cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        _gameplayController = GameObject.FindGameObjectWithTag("Gameplay").GetComponent<GameplayController>();

        if (_type == EnemyType.Crawling) InvokeRepeating("Noise", 1f, 1f / 2);

        if (_type == EnemyType.Shooting || _type == EnemyType.Turret)
            InvokeRepeating("Shoot", 1f, _shootInterval + Random.Range(-_shootNoiseInterval, _shootNoiseInterval));

        RandomOnStart();

        _currentHp = _maxHp;
        
        if (_isSpawner) InvokeRepeating("SpawnEnemy", 1f, 5f);
    }

    private void SpawnEnemy()
    {
        Instantiate(_enemySpawn, transform.position, transform.rotation);
    }

    private void FixedUpdate()
    {
        

        if (_isDead)
            gameObject.transform.up = new Vector3(0,-1f * Time.deltaTime, 0) ;
        
        
        SelectPlayerTarget();
        if (_playerTarget == null) return;
        Vector3 targetPosition = _playerTarget.transform.position + _positionNoise;

        if (_type == EnemyType.Crawling || _type == EnemyType.Shooting)
        {
            float distance = Vector3.Distance(transform.position, _playerTarget.transform.position);

            if (_type == EnemyType.Shooting && distance > _minWalkDistanceFromPlayer || _type == EnemyType.Crawling)
                transform.localPosition =
                    Vector3.MoveTowards(transform.localPosition, targetPosition, _speed * Time.deltaTime);
        }

        Vector3 targetDirection = targetPosition - transform.position;
        Vector3 newDirection =
            Vector3.RotateTowards(transform.forward, targetDirection, _rotateSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
        
        if (transform.position.y < -150)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {  
            _audioSource.pitch = Random.Range(.8f, 1.2f);
            _audioSource.PlayOneShot(_hitSound);
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            _cameraController.Shake(-1f, 1f);

            _currentHp -= bullet.GetDamage();
            if (_currentHp < 0)
            {
                EnemyDie();
                return;
            }
            bullet.DestroyBullet();
        }
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
        if (players.Length > 0)
        {
            _playerTarget = players[Random.Range(0, players.Length)];
        }
    }


    private void Noise()
    {
        _positionNoise = new Vector3(Random.Range(-_maxNoise, _maxNoise), 0, Random.Range(-_maxNoise, _maxNoise));
    }

    public int GetDamage()
    {
        return _damage;
    }

    public void CollideWithPlayer()
    {
        if (_destroyOnCollision) EnemyDie();
    }

    private void Shoot()
    {
        if (!_isDead)
        {
            if (_playerTarget == null) return;
            float distance = Vector3.Distance(transform.position, _playerTarget.transform.position);
            if (!(distance < _maxShootingDistanceFromPlayer)) return;

            GameObject bullet = Instantiate(_bullet, transform.position, transform.rotation);
            bullet.GetComponent<Bullet>().SetDamage(_damage);
            _audioSource.pitch = Random.Range(.9f, 1.33f);
            _audioSource.PlayOneShot(_laserSound);
        }
    }


    private void EnemyDie()
    {
        _gameplayController.playerScore +=
            _scoreWorth;
        
        Destroy(gameObject, 1.5f);
        
        Instantiate(_deathParticle, transform.position, transform.rotation);
        
        _audioSource.pitch = Random.Range(.8f, 1.2f);
        _audioSource.PlayOneShot(_deathSound);
        
        _isDead = true;
        _speed = 0;
        _animatorController.SetBool("isDead", true);

        _boxCollider.enabled = false;
        _rigidbody.isKinematic = true;
    }
}