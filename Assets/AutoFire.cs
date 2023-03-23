using UnityEngine;
public class AutoFire : MonoBehaviour {
    
    [SerializeField] private float _scanFrequency = .5f;
    [SerializeField] private int _coneSegments = 10;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private PlayerScriptableObject _player;

    private ShootController _shooter;
    private float _waitTime;

    private void Awake() {
        _shooter = GetComponent<ShootController>();
        _waitTime = Time.time + _scanFrequency;
    }
    void Update() {
        InvokeRepeating(nameof(RaycastSweep), _scanFrequency, _scanFrequency);

        if (Input.GetMouseButtonDown(0)) {
            RaycastSweep();
        }
    }

    void RaycastSweep() {
        if (Time.time < _waitTime) {
            return;
        }
            
        Vector3 startPos = transform.position; // umm, start position !

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
                _shooter.DoShoot = true;
                _waitTime = Time.time + _scanFrequency;
                Debug.Log("Hit " + hit.collider.gameObject.name);
                return;
            } else {
                _shooter.DoShoot = false;
            }

            // to show ray just for testing
            Debug.DrawLine(startPos, targetPos, Color.green);
        }
    }
}