using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class Testing : MonoBehaviour {

    private EnemySpawner _enemySpawner;

    private List<string> keypadKeys = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
    void Start() {
        _enemySpawner = GameObject.Find("GameManager").GetComponent<EnemySpawner>();
        
        var buttonListener = InputSystem.onAnyButtonPress.Call(ctrl => {
            if (keypadKeys.Contains(ctrl.displayName)) {
                _enemySpawner.SpawnEnemies(Int32.Parse(ctrl.displayName.Substring(ctrl.displayName.Length - 1)));
            }
        });
    }

    // Update is called once per frame
    void Update() {
        
        
    }
}
