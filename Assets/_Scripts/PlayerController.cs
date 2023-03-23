using CodeMonkey.Utils;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private PlayerScriptableObject _player;
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

    private void InitInput() {
        _input = new InputManager();
        if (_input != null) {
            _input.Player.Enable();
        }
    }

    private void InitVisual() {
        GameObject visual = Instantiate(new GameObject("Player Visual"), transform);
        visual.transform.localScale = _player.Scale;
        SpriteRenderer renderer = visual.AddComponent<SpriteRenderer>();
        renderer.sprite = _player.Icon;
        renderer.color = _player.Color;
        _visual = visual;
    }

    private void Update() {        
        Rotate();
    }

    void Rotate() {
        float rotateDirection = _input.Player.Rotate.ReadValue<float>();
        var rotationAmount = _player.TurnSpeed * rotateDirection * Time.deltaTime * Vector3.forward;
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

    public void Die() {
        _visual.SetActive(false);
        StartCoroutine(RespawnAfterTime());
    }
    IEnumerator RespawnAfterTime() {
        yield return new WaitForSeconds(_respawnDelay);
        _visual.SetActive(true);
    }
    void OnDestroy() {
        _levelSystemAnimated.OnLevelChanged -= LevelSystemAnimated_OnLevelChanged;
    }
}
