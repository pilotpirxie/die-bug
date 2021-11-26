using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private float _destroyAfter = 1f;
    [SerializeField] private GameObject _trailObject;
    
    public void Start()
    {
        Invoke("DestroyBullet", _destroyAfter);
    }

    public void FixedUpdate()
    {
        transform.Translate(transform.forward * _speed * Time.deltaTime, Space.World);
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    public int GetDamage()
    {
        return _damage;
    }

    public void DestroyBullet()
    {
        _trailObject.transform.SetParent(null);
        Destroy(gameObject);
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void SetDestroyAfter(float destroyAfter)
    {
        _destroyAfter = destroyAfter;
    }
}