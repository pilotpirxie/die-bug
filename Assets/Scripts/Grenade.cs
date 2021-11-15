using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float _timeToExplode = 3f;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private int _numberOfBullets = 72;
    [SerializeField] private int _damage;
    [SerializeField] private AudioSource _grenadeAudioSource;
    [SerializeField] private AudioClip _explosionSound;
    
    private void Start()
    {
        Invoke("Explode", _timeToExplode);
    }

    private void Explode()
    {
        for (int i = 0; i < _numberOfBullets; i++) Invoke("Shoot", i * 0.01f);

        _grenadeAudioSource.PlayOneShot(_explosionSound);
        Invoke("DestroyGrenade", (_numberOfBullets + 1) * 0.01f + 1f);
    }

    private void DestroyGrenade()
    {
        Destroy(gameObject);
    }

    private void Shoot()
    {
        Quaternion weaponRotation = Quaternion.identity;
        Vector3 weaponPosition = transform.position;
        weaponRotation.eulerAngles += new Vector3(0, Random.Range(-360, 360), 0);

        GameObject bullet = Instantiate(_bullet, weaponPosition, weaponRotation);
        bullet.GetComponent<Bullet>().SetDamage(_damage);
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }
}