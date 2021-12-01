using System.Collections;
using TMPro;
//using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private KeyCode _map;
    [SerializeField] private KeyCode _dash;

    [Header("Player Stats")] 
    [SerializeField] private int _maxHp = 1000;
    public int _currentHp;

    [Header("Weapons")] 
    [SerializeField] private Weapon _selectedWeapon = Weapon.Basic;
    
    [SerializeField] private GameObject _basicWeapon;
    [SerializeField] private GameObject _basicIcon;
    [SerializeField] private float _basicAmmo;
    [SerializeField] private GameObject _shotgunWeapon;
    [SerializeField] private GameObject _shotgunIcon;
    [SerializeField] private float _shotgunAmmo;
    [SerializeField] private GameObject _grenadeWeapon;
    [SerializeField] private GameObject _grenadeIcon;
    [SerializeField] private float _grenadeAmmo;
    //[SerializeField] private GameObject _baseballWeapon;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private GameObject _grenade;
    [SerializeField] private GameObject _baseballArea;
    [SerializeField] private GameObject _shootFrom;
    [SerializeField] private float _currentAmmo;
    [SerializeField] private TextMeshProUGUI _ammoUI;
    
    [SerializeField] private int _basicDamage = 50;
    [SerializeField] private int _shotgunDamage = 150;
    [SerializeField] private int _grenadeDamage = 1000;
    //[SerializeField] private int _baseballDamage = 200;

    [SerializeField] private AudioClip _basicSound;
    [SerializeField] private AudioClip _shotgunSound;
    [SerializeField] private AudioClip _grenadeSound;
    [SerializeField] private AudioClip _emptyMagSound;
    [SerializeField] private AudioClip _deathSound;
    //[SerializeField] private AudioClip _baseballSound;
    [SerializeField] private AudioClip[] _footstepsSounds;

    [Header("Controllers")] 
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private AudioSource _playerAudioSource;
    [SerializeField] private AudioSource _footstepsAudioSource;
    [SerializeField] private AudioSource _deathAudioSource;
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private GameObject _minimap;
    [SerializeField] private Rigidbody _rigidbody;
    
    [Header("Materials")] 
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _damagedMaterial;
    [SerializeField] private Material _healingMaterial;
    [SerializeField] private float _duration;
    [SerializeField] private GameplayController _gameplayController;
    
    
    [Header("DashSettings")]
    [SerializeField] private float _dashForce;
    [SerializeField] private GameObject _dashParticle;
    [SerializeField] private float _dashCooldown = 10f;
    [SerializeField] private Image _dashImage;
    [SerializeField] private AudioClip _dashSound;
    [SerializeField] private AudioSource _dashAudioSource;

    private bool _isDead = false;
        
    private bool _dashReady = true;
    private float _currentCooldown;

    
    
    private bool damaged = false;
    private bool healed = false;
    private string _currentWeapon;
    private ParticleSystem _muzzleFlash;

    private void Start()
    {
        _currentHp = _maxHp;
        SetHealth(_currentHp);
        DisplayCorrectWeapon();
        
        _cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }

    private void Update()
    {
        PlayerDamaged();
        PlayerHealed();
        
        Vector3 inputs = Vector3.zero;

        if(!_isDead)
        {
            if (Input.GetKey(_up)) inputs.z = 1;
            if (Input.GetKey(_down)) inputs.z = -1;
            if (Input.GetKey(_right)) inputs.x = 1;
            if (Input.GetKey(_left)) inputs.x = -1;
        }
        inputs = Vector3.ClampMagnitude(inputs, 1f);
        _playerAnimator.SetBool("isMoving", false);
        if (inputs.magnitude != 0)
        {
            transform.rotation = Quaternion.LookRotation(inputs);
            transform.Translate(inputs * _speed * _speedMultiplier * Time.deltaTime, Space.World);
            _playerAnimator.SetBool("isMoving", true);
        }

        if (Input.GetKeyDown(_attack) && !_isDead)
            switch (_selectedWeapon)
            {
                case Weapon.Basic:
                    BasicAttack();
                    break;
                case Weapon.Shotgun:
                    SingleShotgunShoot();
                    break;
                case Weapon.Grenade:
                    GrenadeAttack();
                    break;
                // case Weapon.Baseball:
                //     BaseballAttack();
                //     break;
            }
        if (Input.GetKeyDown(_map))
            _minimap.SetActive(true);

        if (Input.GetKeyUp(_map))
            _minimap.SetActive(false);
        
        if (Input.GetKeyUp(_dash) && _dashReady)
            PlayerDash();

        if (!_dashReady)
            StartCoroutine(DashOnCooldown());
        
        if (_currentCooldown >= _dashCooldown)
        {
            StopCoroutine(DashOnCooldown());
            _currentCooldown = 0f;
        }
    }

    private void PlayerDash()
    {
        GameObject particles = Instantiate(_dashParticle, gameObject.transform.position, gameObject.transform.rotation);

        //StartCoroutine("DashOnCooldown");
        _dashAudioSource.PlayOneShot(_dashSound);
        _rigidbody.AddRelativeForce(0f, 0f, _dashForce, ForceMode.Impulse);
        _dashReady = false;
    }

    private IEnumerator DashOnCooldown()
    {
        _currentCooldown += .01f;
        _dashImage.fillAmount = ((_currentCooldown / _dashCooldown) * 100) / 100;
        if (_currentCooldown >= _dashCooldown)
        {
            _dashReady = true;
        }
        yield return new WaitForSeconds(.2f);
    }
    private void PlayerDamaged()
    {
        if (damaged)
        {
            float lerp = Mathf.PingPong(Time.time, _duration) / _duration;
            _renderer.material.Lerp(_defaultMaterial, _damagedMaterial, lerp);
            if (lerp > .9f)
            {
                lerp = 0;
                _renderer.material.Lerp(_defaultMaterial, _damagedMaterial, lerp);

                damaged = false;
            }
        }
    }
    private void PlayerHealed()
    {
        if (healed)
        {
            float lerp = Mathf.PingPong(Time.time, _duration) / _duration;
            _renderer.material.Lerp(_defaultMaterial, _healingMaterial, lerp);
            if (lerp > .9f)
            {
                lerp = 0;
                _renderer.material.Lerp(_defaultMaterial, _healingMaterial, lerp);

                healed = false;
                healed = false;
            }
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy") && !_isDead)
        {
            _cameraController.Shake(-1f, 1f);
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            _currentHp -= enemy.GetDamage();
            SetHealth(_currentHp);
            enemy.CollideWithPlayer();
            
            _deathAudioSource.pitch = Random.Range(.7f, 1f);
            _deathAudioSource.PlayOneShot(_deathSound);

            _gameplayController.playerScore -= enemy.GetDamage();
            
            damaged = true;
        }

        if (other.gameObject.CompareTag("Present") && !_isDead)
        {
            Present present = other.gameObject.GetComponent<Present>();

            _selectedWeapon = present.PresentWeapon;

            DisplayCorrectWeapon();
            
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyBullet") && !_isDead)
        {
            _cameraController.Shake(-1f, 1f);
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            _currentHp -= bullet.GetDamage();
            SetHealth(_currentHp);
            bullet.DestroyBullet();

            _gameplayController.playerScore -= bullet.GetDamage();

            _deathAudioSource.pitch = Random.Range(.7f, 1f);
            _deathAudioSource.PlayOneShot(_deathSound);
            
            damaged = true;
        }

        if (other.gameObject.CompareTag("Healing") && !_isDead)
        {
            _currentHp = _maxHp;
            Destroy(other.gameObject);
            SetHealth(_currentHp);
            
            healed = true;
        }
    }

    public void SetHealth(int health)
    {
        _hpSlider.value = health;
        
        if(_currentHp <= 0)
        {
            _playerAudioSource.PlayOneShot(_deathSound);
            _isDead = true;
            _gameplayController.DeathPause(_isDead);
        }
    }
    private void DisplayCorrectWeapon()
    {
            _basicWeapon.SetActive(false);
            _shotgunWeapon.SetActive(false);
            _grenadeWeapon.SetActive(false);
            //_baseballWeapon.SetActive(false);
            
            
            _basicIcon.SetActive(false);
            _shotgunIcon.SetActive(false);
            _grenadeIcon.SetActive(false);

            _currentWeapon = _selectedWeapon.ToString();

            switch (_selectedWeapon)
            {
                case Weapon.Basic:
                    _basicWeapon.SetActive(true);
                    _currentAmmo = _basicAmmo;
                    _ammoUI.SetText(_currentAmmo.ToString());
                    _muzzleFlash = _basicWeapon.GetComponentsInChildren<ParticleSystem>()[0];
                    _basicIcon.SetActive(true);
                    break;
                case Weapon.Shotgun:
                    _shotgunWeapon.SetActive(true);
                    _currentAmmo = _shotgunAmmo;
                    _ammoUI.SetText(_currentAmmo.ToString());
                    _muzzleFlash = _shotgunWeapon.GetComponentsInChildren<ParticleSystem>()[0];
                    _shotgunIcon.SetActive(true);
                    break;
                case Weapon.Grenade:
                    _grenadeWeapon.SetActive(true);
                    _currentAmmo = _grenadeAmmo;
                    _ammoUI.SetText(_currentAmmo.ToString());
                    _muzzleFlash = _grenadeWeapon.GetComponentsInChildren<ParticleSystem>()[0];
                    _grenadeIcon.SetActive(true);
                    break;
                // case Weapon.Baseball:
                //     //_baseballWeapon.SetActive(true);
                //     break;
            }
    }

    private void BasicAttack()
    {
        if(_currentAmmo > 0)
        {
            _currentAmmo--;
            _ammoUI.SetText(_currentAmmo.ToString());

            _playerAudioSource.PlayOneShot(_basicSound);
            _playerAnimator.SetTrigger("Shoot");
            _muzzleFlash.Play();

            GameObject bullet = Instantiate(_bullet, _shootFrom.transform.position, _shootFrom.transform.rotation);
            bullet.GetComponent<Bullet>().SetDamage(_basicDamage);
        }
        else
            _playerAudioSource.PlayOneShot(_emptyMagSound);
    }

    private void SingleShotgunShoot()
    {
        if(_currentAmmo > 0)
        {
            _currentAmmo--;
            _ammoUI.SetText(_currentAmmo.ToString());
            
            Quaternion weaponRotation = _shootFrom.transform.rotation;
            Vector3 weaponPosition = _shootFrom.transform.position;
            _playerAnimator.SetTrigger("Shoot");

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
            _muzzleFlash.Play();
        }
        else
            _playerAudioSource.PlayOneShot(_emptyMagSound);
    }

    private void GrenadeAttack()
    {
        if(_currentAmmo > 0)
        {
            _currentAmmo--;
            _ammoUI.SetText(_currentAmmo.ToString());
            
            _playerAudioSource.PlayOneShot(_grenadeSound);
            _playerAnimator.SetTrigger("Shoot");
            _muzzleFlash.Play();

            GameObject grenade =
                Instantiate(_grenade, _shootFrom.transform.position, _shootFrom.transform.rotation);
            grenade.GetComponent<Rigidbody>().AddForce(grenade.transform.forward * 7f, ForceMode.Impulse);
            grenade.GetComponent<Rigidbody>().AddForce(grenade.transform.up * 10f, ForceMode.Impulse);
            grenade.GetComponent<Grenade>().SetDamage(_grenadeDamage);
        }
        else
            _playerAudioSource.PlayOneShot(_emptyMagSound);
    }

    // private void BaseballAttack()
    // {
    //     _playerAudioSource.PlayOneShot(_baseballSound);
    //
    //     _playerAnimator.ResetTrigger("onSwing");
    //     _playerAnimator.SetTrigger("onSwing");
    //     
    //     GameObject bullet = Instantiate(_baseballArea, _shootFrom.transform.position, _shootFrom.transform.rotation);
    //     bullet.GetComponent<Bullet>().SetDamage(_baseballDamage);
    //     bullet.GetComponent<Bullet>().SetDestroyAfter(0.2f);
    //
    // }

    // used in the walk animation as event
    public void FootstepsAudio()
    {
        _footstepsAudioSource.PlayOneShot(_footstepsSounds[Random.Range(0, 3)]);
    }
}