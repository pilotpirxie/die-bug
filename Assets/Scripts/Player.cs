using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _speedMultiplier = 1f;

    [SerializeField] private KeyCode _up;
    [SerializeField] private KeyCode _down;
    [SerializeField] private KeyCode _left;
    [SerializeField] private KeyCode _right;
    [SerializeField] private KeyCode _shoot;

    [SerializeField] private int _maxHP = 1000;
    [SerializeField] private int _currentHP;

    [SerializeField] private GameObject _bullet;
    [SerializeField] private int _damage = 100;
    
    void Start()
    {
        _currentHP = _maxHP;
    }

    void Update()
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

        if (Input.GetKeyDown(_shoot))
        {
            GameObject bullet = Instantiate(_bullet, transform.position, transform.rotation);
            bullet.GetComponent<Bullet>().SetDamage(_damage);
        }
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
}
