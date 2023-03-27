using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour {    

    public EnemyType Type = EnemyType.Standard;
    public EnemyScriptableObject _enemyStats;
    public bool Targeted = false;

    private EnemySpawner _enemySpawner;
    private int _hitsRemaining;


    void Awake() {
        _enemySpawner = GameObject.Find("GameManager").GetComponent<EnemySpawner>();
    }

    private void OnEnable() {
        _hitsRemaining = _enemyStats.Health;
    }

    private void OnDisable() {
        Targeted = false;
    }

    public void Hit() {
        _hitsRemaining--;
        if (_hitsRemaining <= 0) {                        
            _enemySpawner.Despawn(this, true);
        }
    }

    
    public void TargetWithCooldown(float delay) {
        Targeted = true;
        StartCoroutine(RemoveTarget(delay));
    }

    IEnumerator RemoveTarget(float delay) {
        yield return new WaitForSeconds(delay);
        Targeted = false;
    }
}