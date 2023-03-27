using CodeMonkey.Utils;
using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public PlayerStats PlayerStats;
    [SerializeField] private Transform _levelUpParticles;    
    [SerializeField] private Transform _radarOverlay;
    [SerializeField] private float _respawnDelay;

    private GameObject _visual;
    private InputManager _input;
    private LevelSystemAnimated _levelSystemAnimated;

    void Start() {
        InitVisual();
        InitInput();
    }

    private void OnEnable() {
        PlayerStats.HealthChanged += CheckIfAlive;
    }

    private void InitInput() {
        _input = new InputManager();
        if (_input != null) {
            _input.Player.Enable();
        }
    }

    private void InitVisual() {
        GameObject visual = Instantiate(new GameObject("Player Visual"), transform);
        visual.transform.localScale = PlayerStats.Scale;
        SpriteRenderer renderer = visual.AddComponent<SpriteRenderer>();
        renderer.sprite = PlayerStats.Icon;
        renderer.color = PlayerStats.Color;
        _visual = visual;
    }

    private void Update() {        
        Rotate();
    }

    void Rotate() {
        float rotateDirection = _input.Player.Rotate.ReadValue<float>();
        var rotationAmount = PlayerStats.TurnSpeed * rotateDirection * Time.deltaTime * Vector3.forward;
        transform.Rotate(rotationAmount);
        _radarOverlay.transform.Rotate(rotationAmount * -1);
    }

    public void SetLevelSystemAnimated(LevelSystemAnimated levelSystemAnimated) {
        _levelSystemAnimated = levelSystemAnimated;
        _levelSystemAnimated.OnLevelChanged += LevelSystemAnimated_OnLevelChanged;
    }

    private void LevelSystemAnimated_OnLevelChanged(object sender, System.EventArgs e) {
        PlayLevelUpEffect();
    }

    private void PlayLevelUpEffect() {
        Transform effect = Instantiate(_levelUpParticles, transform);
        FunctionTimer.Create(() => Destroy(effect.gameObject), 3f);
    }

    public void TakeDamage(int damage) {
        PlayerStats.Health -= damage;
    }

    private void CheckIfAlive(object sender, EventArgs e) {
        if (PlayerStats.Health <= 0) {
            _visual.SetActive(false);
            StartCoroutine(RespawnAfterTime());
        }        
    }

    IEnumerator RespawnAfterTime() {
        yield return new WaitForSeconds(_respawnDelay);
        _visual.SetActive(true);
        PlayerStats.Health = PlayerStats.MaxHealth;
    }
    void OnDestroy() {
        _levelSystemAnimated.OnLevelChanged -= LevelSystemAnimated_OnLevelChanged;
        PlayerStats.HealthChanged -= CheckIfAlive;
    }
}
