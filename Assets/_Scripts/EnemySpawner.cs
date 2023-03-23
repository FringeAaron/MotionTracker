using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public event EventHandler OnEnemyKilled;

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _initialEnemyCount = 2;

    private readonly float _spawnRadius = 15f;
    private int _enemyIndex;
    private List<Enemy> _enemies;
    private bool _canSpawn;
    private LevelSystem _levelSystem;



    private void Start() {
        _enemies = new List<Enemy>();
        for (int i = 0; i < _initialEnemyCount; i++) {
            _enemies.Add(Instantiate(_enemyPrefab).GetComponent<Enemy>());
            _enemies[i].gameObject.SetActive(false);
        }
    }

    public void SpawnEnemies(int amt) {
        for (int i = 0; i < amt; i++) {
            _enemyIndex = 0;
            _canSpawn = true;
            while (_canSpawn) {
                var position = new Vector3(UnityEngine.Random.Range(-_spawnRadius, _spawnRadius), UnityEngine.Random.Range(-_spawnRadius, _spawnRadius), 0);
                if (!_enemies[_enemyIndex].gameObject.activeInHierarchy) {
                    _enemies[_enemyIndex].transform.position = position;
                    _enemies[_enemyIndex].gameObject.SetActive(true);

                    _canSpawn = false;
                    break;
                } else {
                    _enemyIndex++;
                }

                if (_enemyIndex == _enemies.Count) {
                    _enemies.Add(Instantiate(_enemyPrefab, position, Quaternion.identity).GetComponent<Enemy>());

                    _canSpawn = false;
                    break;
                }
            }
        }
    }

    public void Respawn(Enemy enemy) {
        enemy.transform.position = new Vector3(UnityEngine.Random.Range(-_spawnRadius, _spawnRadius), UnityEngine.Random.Range(-_spawnRadius, _spawnRadius), 0);
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

}