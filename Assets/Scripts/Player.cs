using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    [SerializeField] private int _maxHP = 1000;
    [SerializeField] private int _currentHP;

    [Header("Weapons")] 
    [SerializeField] private Weapon _selectedWeapon = Weapon.Basic;
    [SerializeField] private GameObject _basicWeapon;
    [SerializeField] private GameObject _shotgunWeapon;
    [SerializeField] private GameObject _grenadeWeapon;
    [SerializeField] private GameObject _baseballWeapon;
    [SerializeField] private GameObject _basicBullet;
    [SerializeField] private GameObject _shotgunBullet;
    [SerializeField] private GameObject _grenade;
    
    [SerializeField] private int _basicDamage = 50;
    [SerializeField] private int _shotgunDamage = 150;
    [SerializeField] private int _grenadeDamage = 1000;
    [SerializeField] private int _baseballDamage = 200;
    
    private void Start()
    {
        _currentHP = _maxHP;
    }

    private void Update()
    {
        Vector3 inputs = Vector3.zero;

        if (Input.GetKey(_up)) inputs.z = 1;
        if (Input.GetKey(_down)) inputs.z = -1;
        if (Input.GetKey(_right)) inputs.x = 1;
        if (Input.GetKey(_left)) inputs.x = -1;

        inputs = Vector3.ClampMagnitude(inputs, 1f);
        if (inputs.magnitude != 0)
        {
            transform.rotation = Quaternion.LookRotation (inputs);
            transform.Translate(inputs * _speed * _speedMultiplier * Time.deltaTime, Space.World);
        }

        if (Input.GetKeyDown(_attack))
        {
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
        }

        DisplayCorrectWeapon();
    }
    
    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            _currentHP -= enemy.GetDamage();
            enemy.CollideWithPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            _currentHP -= bullet.GetDamage();
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
        GameObject bullet = Instantiate(_basicBullet, _shotgunWeapon.transform.position, _shotgunWeapon.transform.rotation);
        bullet.GetComponent<Bullet>().SetDamage(_basicDamage);
    }

    private void ShotgunAttack()
    {
        for (int i = 0; i < 3; i++)
        {
            Invoke("SingleShotgunShoot", i * 0.1f);
        }
    }

    private void SingleShotgunShoot()
    {
        Quaternion weaponRotation = _shotgunWeapon.transform.rotation;
        Vector3 weaponPosition = _shotgunWeapon.transform.position;
        
        weaponRotation.eulerAngles += new Vector3(0, -20f + Random.Range(-5, 5), 0);
        GameObject bullet1 = Instantiate(_basicBullet, weaponPosition, weaponRotation);
        bullet1.GetComponent<Bullet>().SetDamage(_basicDamage);
        
        weaponRotation.eulerAngles += new Vector3(0, 10 + Random.Range(-5, 5), 0);
        GameObject bullet2 = Instantiate(_basicBullet, weaponPosition, weaponRotation);
        bullet2.GetComponent<Bullet>().SetDamage(_basicDamage);

        weaponRotation.eulerAngles += new Vector3(0, 10 + Random.Range(-5, 5), 0);
        GameObject bullet3 = Instantiate(_basicBullet, weaponPosition, weaponRotation);
        bullet3.GetComponent<Bullet>().SetDamage(_basicDamage);

        weaponRotation.eulerAngles += new Vector3(0, 10 + Random.Range(-5, 5), 0);
        GameObject bullet4 = Instantiate(_basicBullet, weaponPosition, weaponRotation);
        bullet4.GetComponent<Bullet>().SetDamage(_basicDamage);

        weaponRotation.eulerAngles += new Vector3(0, 10 + Random.Range(-5, 5), 0);
        GameObject bullet5 = Instantiate(_basicBullet, weaponPosition, weaponRotation);
        bullet5.GetComponent<Bullet>().SetDamage(_basicDamage);
    }

    private void GrenadeAttack()
    {
        
    }

    private void BaseballAttack()
    {
        
    }
}
