using System;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour {    

    public EnemyType Type = EnemyType.Standard;
    public EnemyScriptableObject _enemyStats;

    private EnemySpawner _enemySpawner;
    private float _speed = 1f;
    private int _hits = 1;
    private Transform _target;
    private int _hitsRemaining;

    private void OnEnable() {
        SetupEnemyFromConfiguration();
    }

    void Start() {
        _hitsRemaining = _hits;
        _target = GameObject.FindGameObjectsWithTag("Player").First().transform;
        _enemySpawner = GameObject.Find("GameManager").GetComponent<EnemySpawner>();
    }

    void Update() {
        if (!gameObject.activeInHierarchy) {
            return;
        }
        var step = _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _target.position, step);

        if (Vector3.Distance(transform.position, _target.position) < 0.001f) {
            if (_target.TryGetComponent<HealthController>(out var health)) {
                health.Damage(_enemyStats.Damage);
            }
            Despawn(false);
        }
    }

    public void Hit() {
        _hitsRemaining--;
        if (_hitsRemaining <= 0) {            
            Despawn();
        }
    }

    public void Despawn(bool earnExperience = true) {
        _hitsRemaining = _hits;
        _enemySpawner.Despawn(this, earnExperience);        
    }

    private void SetupEnemyFromConfiguration() {
        _hits = _enemyStats.Health;
        _speed = _enemyStats.Speed;        
        
        Type = _enemyStats.Type;
    }
}