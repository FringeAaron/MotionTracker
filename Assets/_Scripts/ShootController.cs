using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour {

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPos;
    [SerializeField] private float _fireRate;
    [SerializeField] private int _initialBulletCount = 2;
    
    private bool _canShoot;
    private int _bulletIndex;
    private List<Bullet> _bullets;
    private float _waitTime;
    public bool DoShoot = false;

    private void Start() {        
        _bullets = new List<Bullet>();
        for (int i = 0; i < _initialBulletCount; i++) {            
            _bullets.Add(Instantiate(_bulletPrefab).GetComponent<Bullet>());
            _bullets[i].gameObject.SetActive(false);
        }

        _waitTime = Time.time + _fireRate;
    }

    private void Update() {
        if (DoShoot) {
            if (Time.time > _waitTime) {
                _waitTime = Time.time + _fireRate;
                Shoot();
            }
        }

    }

    private void Shoot() {
        _canShoot = true;
        _bulletIndex = 0;

        while (_canShoot) {
            if (!_bullets[_bulletIndex].gameObject.activeInHierarchy) {                
                _bullets[_bulletIndex].transform.SetPositionAndRotation(_bulletSpawnPos.position, _bulletSpawnPos.rotation);
                _bullets[_bulletIndex].gameObject.SetActive(true);
                _bullets[_bulletIndex].ShootBullet(transform.up);

                _canShoot = false;
                break;
            } else {
                _bulletIndex++;
            }

            if (_bulletIndex == _bullets.Count) {

                _bullets.Add(Instantiate(_bulletPrefab, _bulletSpawnPos.position, _bulletSpawnPos.rotation).GetComponent<Bullet>());
                _bullets[_bullets.Count - 1].ShootBullet(transform.up);

                _canShoot = false;
                break;
            }
        }
    }
}
