using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {
    private PlayerController _player;
    void Start() {
        _player = GetComponent<PlayerController>();
    }

    public void Damage(float damage) {
        _player.Health -= damage;
        if (_player.Health <= 0) {
            _player.Die();
        }
    }
}
