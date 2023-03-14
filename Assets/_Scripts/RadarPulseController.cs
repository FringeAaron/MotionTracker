using System;
using System.Collections.Generic;
using UnityEngine;

public class RadarPulseController : MonoBehaviour {
    [SerializeField] private Transform _radarPingPrefab;
    [SerializeField] private float _rangeMax = 100f;
    [SerializeField] private float _rangeSpeed = 100f;
    [SerializeField] private float _pulseOpacity = 1f;
    [SerializeField] private Color _enemyColor;    
    [SerializeField] private float _fadeRange = 100f;
    [SerializeField] private float _livePingInstances = 1f;
    [SerializeField] private AudioClip _pingSound;
    [SerializeField] private AudioClip _motionSound;


    private Transform _pulseTransform;
    private float _range;
    private List<Collider2D> _alreadyPingedColliderList;
    private SpriteRenderer _spriteRenderer;
    private Color _pulseColor;
    private bool _motionPickedUp = false;

    private void Awake() {
        _pulseTransform = transform.Find("Pulse");
        _spriteRenderer = _pulseTransform.GetComponent<SpriteRenderer>();
        _pulseColor = _spriteRenderer.color;
        _alreadyPingedColliderList = new List<Collider2D>();
    }

    private void Update() {
        _range += _rangeSpeed * Time.deltaTime;
        if (_range > _rangeMax) {
            _range = 0;
            _alreadyPingedColliderList.Clear();
            _motionPickedUp = false;
            AudioSystem.Instance.PlaySound(_pingSound, .5f, 1f);
        }
        _pulseTransform.localScale = new Vector3(_range, _range);

        RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(transform.position, _range / 2f, Vector2.zero);
        foreach (RaycastHit2D raycastHit2D in raycastHit2Ds) {            
            var col = raycastHit2D.collider;
            if (col != null && !col.CompareTag("HideFromRadar")) {
                if (!_alreadyPingedColliderList.Contains(col)) {
                    _alreadyPingedColliderList.Add(col);
                    
                    Transform radarPingTransform = Instantiate(_radarPingPrefab, raycastHit2D.point, Quaternion.identity);                    
                    RadarPing radarPing = radarPingTransform.GetComponent<RadarPing>();
                    PlayMotionSound(raycastHit2D, radarPing);
                    radarPing.SetDisappearTimer(_rangeMax / _rangeSpeed * _livePingInstances);

                    var enemy = col.gameObject.GetComponent<Enemy>();
                    if (enemy != null) {
                        radarPing.SetColor(GetColor(enemy.Type));
                        radarPingTransform.localScale = GetScale(enemy.Type);
                    }
                    
                }
            }
        }
        
        if (_range > _rangeMax - _fadeRange) {
            _pulseColor.a = Mathf.Lerp(0f, _pulseOpacity, (_rangeMax - _range) / _fadeRange);
        } else {
            _pulseColor.a = _pulseOpacity;
        }

        _spriteRenderer.color = _pulseColor;
    }

    private void PlayMotionSound(RaycastHit2D raycastHit2D, RadarPing radarPing) {
        if (_motionPickedUp) {
            return;
        }
        var distance = Vector2.Distance(transform.position, raycastHit2D.point);
        if (distance > 16f) {
            return;
        }
        
        var multiplier = Math.Clamp(8 / distance, .1f, 2.5f);        
        
        /*Debug.Log("Distance: " + distance);
        Debug.Log("Calculated: " + (8 / distance));
        Debug.Log("Multiplier: " + multiplier);*/

        radarPing.PlaySound(_motionSound, 1f, multiplier);        
        _motionPickedUp = true;
    }

    private Vector3 GetScale(EnemyType type) {
        return type switch {
            EnemyType.Heavy => new Vector3(4f, 4f, 1),
            _ => Vector3.one,
        };
    }

    private Color GetColor(EnemyType type) {
        return type switch {
            EnemyType.Heavy => Color.cyan,
            _ => _enemyColor,
        };
    }
}
