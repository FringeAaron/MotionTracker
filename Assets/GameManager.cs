using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private LevelUI _levelUI;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private PlayerStats _playerStats;

    private void Awake() {
        _playerStats.InitDefaults();
    }

    private void Start() {
        LevelSystem levelSystem = new();
        LevelSystemAnimated levelSystemAnimated = new(levelSystem);
        _enemySpawner = GetComponent<EnemySpawner>();

        _levelUI.SetLevelSystem(levelSystem);
        _enemySpawner.SetLevelSystem(levelSystem);

        _levelUI.SetLevelSystemAnimated(levelSystemAnimated);
        _playerController.SetLevelSystemAnimated(levelSystemAnimated);
    }



}
