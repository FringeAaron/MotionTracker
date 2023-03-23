using UnityEngine;

public class HealthController : MonoBehaviour {
    [SerializeField] private PlayerScriptableObject _player;
    private PlayerController _playerController;
    void Start() {
        _playerController = GetComponent<PlayerController>();
    }

    public void Damage(int damage) {
        _player.Health.Value -= damage;
        if (_player.Health.Value <= 0) {
            _playerController.Die();
        }
    }
}
