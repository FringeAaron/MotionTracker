using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public event EventHandler OnEnemyKilled;

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _initialEnemyCount = 2;
    [SerializeField] private float _minSpawnRadius = 10f;
    [SerializeField] private float _maxSpawnRadius = 35f;
    [SerializeField] private TargetDamageStats _targetDamage;

    private int _enemyIndex;
    private List<Enemy> _enemies;
    private bool _canSpawn;
    private LevelSystem _levelSystem;
    private PlayerController _target;



    private void Start() {
        _target = GameObject.FindGameObjectsWithTag("Player").First().GetComponent<PlayerController>();
        _targetDamage.TargetStats = _target.PlayerStats;

        _enemies = new List<Enemy>();
        for (int i = 0; i < _initialEnemyCount; i++) {
            var enemy = Instantiate(_enemyPrefab);
            enemy.name = "Enemy " + i;
            enemy.SetActive(false);
            _enemies.Add(enemy.GetComponent<Enemy>());
        }
    }

    public void SpawnEnemies(int amt) {
        for (int i = 0; i < amt; i++) {
            _enemyIndex = 0;
            _canSpawn = true;
            
            // Find a spot in an annulus shape around the player
            Vector2 pos = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(_minSpawnRadius, _maxSpawnRadius);

            while (_canSpawn) {
                if (!_enemies[_enemyIndex].gameObject.activeInHierarchy) {
                    // Respawn here so the array reference is updated too
                    _enemies[_enemyIndex].transform.position = pos;
                    _enemies[_enemyIndex].gameObject.SetActive(true);                    
                    _canSpawn = false;
                    break;
                } else {
                    _enemyIndex++;
                }

                if (_enemyIndex == _enemies.Count) {
                    var newEnemy = Instantiate(_enemyPrefab, pos, Quaternion.identity);
                    newEnemy.name = "Enemy " + _enemyIndex;
                    _enemies.Add(newEnemy.GetComponent<Enemy>());

                    _canSpawn = false;
                    break;
                }
            }
        }
    }


    public void Despawn(Enemy enemy, bool earnExperience) {
        enemy.gameObject.SetActive(false);
        if (earnExperience) {
            OnEnemyKilled?.Invoke(this, EventArgs.Empty);
            _levelSystem.AddExperience(enemy._enemyStats.ExperienceForKill);
        }
    }

    public void SetLevelSystem(LevelSystem levelSystem) {
        _levelSystem = levelSystem;
    }

    private void Update() {
        for (int i = 0; i < _enemies.Count; i++) {
            Enemy enemy = _enemies[i];
            if (!enemy.gameObject.activeInHierarchy) {
                continue;
            }
            var step = enemy._enemyStats.Speed * Time.deltaTime;
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, _target.transform.position, step);

            if (Vector3.Distance(enemy.transform.position, _target.transform.position) < 0.001f) {                
                _targetDamage.Damage = enemy._enemyStats.Damage;
                _targetDamage.CommitDamage();
                Despawn(enemy, false);
            }
        }
    }

}