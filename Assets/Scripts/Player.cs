using UnityEngine;

public enum Weapon
{
    Basic,
    Shotgun,
    Grenade,
    Baseball
}

public class Player : MonoBehaviour
{
    [Header("Movement")] 
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _speedMultiplier = 1f;

    [Header("Keyboard Configuration")] 
    [SerializeField] private KeyCode _up;
    [SerializeField] private KeyCode _down;
    [SerializeField] private KeyCode _left;
    [SerializeField] private KeyCode _right;
    [SerializeField] private KeyCode _attack;

    [Header("Player Stats")] 
    [SerializeField] private int _maxHp = 1000;
    [SerializeField] private int _currentHp;

    [Header("Weapons")] 
    [SerializeField] private Weapon _selectedWeapon = Weapon.Basic;
    
    [SerializeField] private GameObject _basicWeapon;
    [SerializeField] private GameObject _shotgunWeapon;
    [SerializeField] private GameObject _grenadeWeapon;
    [SerializeField] private GameObject _baseballWeapon;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private GameObject _grenade;
    [SerializeField] private GameObject _baseballArea;
    
    [SerializeField] private int _basicDamage = 50;
    [SerializeField] private int _shotgunDamage = 150;
    [SerializeField] private int _grenadeDamage = 1000;
    [SerializeField] private int _baseballDamage = 200;

    [SerializeField] private AudioClip _basicSound;
    // [SerializeField] private AudioClip _basicReloadSound;
    [SerializeField] private AudioClip _shotgunSound;
    // [SerializeField] private AudioClip _shotgunReloadSound;
    [SerializeField] private AudioClip _grenadeSound;
    // [SerializeField] private AudioClip _grenadeReloadSound;
    [SerializeField] private AudioClip _baseballSound;
    // [SerializeField] private AudioClip _baseballReloadSound;
    
    [Header("Controllers")] 
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private AudioSource _playerAudioSource;

    private void Start()
    {
        _currentHp = _maxHp;
    }

    private void Update()
    {
        Vector3 inputs = Vector3.zero;

        if (Input.GetKey(_up)) inputs.z = 1;
        if (Input.GetKey(_down)) inputs.z = -1;
        if (Input.GetKey(_right)) inputs.x = 1;
        if (Input.GetKey(_left)) inputs.x = -1;

        inputs = Vector3.ClampMagnitude(inputs, 1f);
        _playerAnimator.SetBool("isMoving", false);
        if (inputs.magnitude != 0)
        {
            transform.rotation = Quaternion.LookRotation(inputs);
            transform.Translate(inputs * _speed * _speedMultiplier * Time.deltaTime, Space.World);
            _playerAnimator.SetBool("isMoving", true);
        }

        if (Input.GetKeyDown(_attack))
            switch (_selectedWeapon)
            {
                case Weapon.Basic:
                    BasicAttack();
                    break;
                case Weapon.Shotgun:
                    ShotgunAttack();
                    break;
                case Weapon.Grenade:
                    GrenadeAttack();
                    break;
                case Weapon.Baseball:
                    BaseballAttack();
                    break;
            }

        DisplayCorrectWeapon();
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            _currentHp -= enemy.GetDamage();
            enemy.CollideWithPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            _currentHp -= bullet.GetDamage();
            bullet.DestroyBullet();
        }
    }

    private void DisplayCorrectWeapon()
    {
        _basicWeapon.SetActive(false);
        _shotgunWeapon.SetActive(false);
        _grenadeWeapon.SetActive(false);
        _baseballWeapon.SetActive(false);

        switch (_selectedWeapon)
        {
            case Weapon.Basic:
                _basicWeapon.SetActive(true);
                break;
            case Weapon.Shotgun:
                _shotgunWeapon.SetActive(true);
                break;
            case Weapon.Grenade:
                _grenadeWeapon.SetActive(true);
                break;
            case Weapon.Baseball:
                _baseballWeapon.SetActive(true);
                break;
        }
    }

    private void BasicAttack()
    {
        _playerAudioSource.PlayOneShot(_basicSound);

        GameObject bullet = Instantiate(_bullet, _basicWeapon.transform.position, _basicWeapon.transform.rotation);
        bullet.GetComponent<Bullet>().SetDamage(_basicDamage);
    }

    private void ShotgunAttack()
    {
        for (int i = 0; i < 3; i++) Invoke("SingleShotgunShoot", i * 0.1f);
    }

    private void SingleShotgunShoot()
    {
        Quaternion weaponRotation = _shotgunWeapon.transform.rotation;
        Vector3 weaponPosition = _shotgunWeapon.transform.position;

        weaponRotation.eulerAngles += new Vector3(0, -20f + Random.Range(-5, 5), 0);
        GameObject bullet1 = Instantiate(_bullet, weaponPosition, weaponRotation);
        bullet1.GetComponent<Bullet>().SetDamage(_shotgunDamage);

        weaponRotation.eulerAngles += new Vector3(0, 10 + Random.Range(-5, 5), 0);
        GameObject bullet2 = Instantiate(_bullet, weaponPosition, weaponRotation);
        bullet2.GetComponent<Bullet>().SetDamage(_shotgunDamage);

        weaponRotation.eulerAngles += new Vector3(0, 10 + Random.Range(-5, 5), 0);
        GameObject bullet3 = Instantiate(_bullet, weaponPosition, weaponRotation);
        bullet3.GetComponent<Bullet>().SetDamage(_shotgunDamage);

        weaponRotation.eulerAngles += new Vector3(0, 10 + Random.Range(-5, 5), 0);
        GameObject bullet4 = Instantiate(_bullet, weaponPosition, weaponRotation);
        bullet4.GetComponent<Bullet>().SetDamage(_shotgunDamage);

        weaponRotation.eulerAngles += new Vector3(0, 10 + Random.Range(-5, 5), 0);
        GameObject bullet5 = Instantiate(_bullet, weaponPosition, weaponRotation);
        bullet5.GetComponent<Bullet>().SetDamage(_shotgunDamage);
        
        _playerAudioSource.PlayOneShot(_shotgunSound);
    }

    private void GrenadeAttack()
    {
        _playerAudioSource.PlayOneShot(_grenadeSound);

        GameObject grenade =
            Instantiate(_grenade, _grenadeWeapon.transform.position, _grenadeWeapon.transform.rotation);
        grenade.GetComponent<Rigidbody>().AddForce(grenade.transform.forward * 7f, ForceMode.Impulse);
        grenade.GetComponent<Rigidbody>().AddForce(grenade.transform.up * 10f, ForceMode.Impulse);
        grenade.GetComponent<Grenade>().SetDamage(_grenadeDamage);
    }

    private void BaseballAttack()
    {
        _playerAudioSource.PlayOneShot(_baseballSound);

        _playerAnimator.ResetTrigger("onSwing");
        _playerAnimator.SetTrigger("onSwing");
        
        GameObject bullet = Instantiate(_baseballArea, _baseballWeapon.transform.position, _baseballWeapon.transform.rotation);
        bullet.GetComponent<Bullet>().SetDamage(_baseballDamage);
        bullet.GetComponent<Bullet>().SetDestroyAfter(0.2f);

    }
}