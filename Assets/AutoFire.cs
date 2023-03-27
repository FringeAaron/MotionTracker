using UnityEngine;
public class AutoFire : MonoBehaviour {
    
    [SerializeField] private float _scanFrequency = .5f;
    [SerializeField] private int _coneSegments = 10;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private PlayerStats _player;
    [SerializeField] private GameObject _muzzleFlash;

    private float _waitTime;
    private bool _doShoot = false;


    private void Awake() {
        _waitTime = Time.time + _scanFrequency;
        _muzzleFlash.SetActive(false);

    }
    void Update() {
        InvokeRepeating(nameof(RaycastSweep), _scanFrequency, _scanFrequency);
    }

    void RaycastSweep() {
        if (Time.time < _waitTime) {
            return;
        }
        
        _muzzleFlash.SetActive(_doShoot);

        Vector3 startPos = new Vector3(transform.position.x, _muzzleFlash.transform.position.y, transform.position.z); // umm, start position !

        int startAngle = Mathf.RoundToInt(-_player.FireAngle * 0.5f); // half the angle to the Left of the forward
        int finishAngle = Mathf.RoundToInt(_player.FireAngle * 0.5f); // half the angle to the Right of the forward

        // the gap between each ray (increment)
        int inc = Mathf.RoundToInt(_player.FireAngle / _coneSegments);

        RaycastHit2D hit;

        // step through and find each target point
        for (var i = startAngle; i < finishAngle; i += inc) {
            Vector3 targetPos = (Quaternion.Euler(0, 0, i) * transform.up).normalized * _player.FireRange;

            // linecast between points
            hit = Physics2D.Linecast(startPos, targetPos, _layerMask);
            if (hit.collider != null) {
                if (hit.collider.TryGetComponent<Enemy>(out var enemy)) {
                    enemy.TargetWithCooldown(1.5f);
                    enemy.Hit();
                    _doShoot = true;
                    _muzzleFlash.SetActive(_doShoot);
                }
                
                _waitTime = Time.time + _scanFrequency;
                
                Debug.Log("Hit " + hit.collider.gameObject.name);
                return;
            } else {
                _doShoot = false;
            }

            // to show ray just for testing
            Debug.DrawLine(startPos, targetPos, Color.green);
        }

        _doShoot = false;
    }
}