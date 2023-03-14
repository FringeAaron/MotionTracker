using CodeMonkey.Utils;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Transform _levelUpParticles;

    private Transform _radarOverlay;
    private GameObject _visual;
    private InputManager _input;
    private ShootController _shootController;    
    private LevelSystemAnimated _levelSystemAnimated;
    public float Health = 2;

    void Start() {
        _visual = transform.Find("Visual").gameObject;
        _radarOverlay = transform.Find("Redo-Overlay");        
        InputInitialization();
        _shootController = gameObject.GetComponent<ShootController>();
    }

    private void InputInitialization() {
        _input = new InputManager();
        if (_input != null)
            _input.Player.Enable();
    }

    private void Update() {        
        Rotate();
        _shootController.DoShoot = _input.Player.Shoot.IsPressed();
    }
    void Rotate() {
        float rotateDirection = _input.Player.Rotate.ReadValue<float>();
        var rotationAmount = _speed * rotateDirection * Time.deltaTime * Vector3.forward;
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
    }
    void OnDestroy() {
        _levelSystemAnimated.OnLevelChanged -= LevelSystemAnimated_OnLevelChanged;
    }
}
