using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player", order = 1)]
public class PlayerStats : ScriptableObject {
    [SerializeField] private Color32 _color;
    [SerializeField] private Vector3 _scale;
    [SerializeField] private Sprite _icon;

    [Header("Starting Values")]
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _experience;
    [SerializeField] private int _level;
    [SerializeField] private int _damage;

    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _fireRange;
    [SerializeField] private float _fireAngle;

    [SerializeField] private float _radarScanFrequency;
    [SerializeField] private float _radarScanRange;


    public Color32 Color;    
    public Vector3 Scale;
    public Sprite Icon;

    private int _activeHealth;
    private int _activeMaxHealth;
    public event EventHandler HealthChanged;
    public event EventHandler MaxHealthChanged;

    public int Health {
        get { return _activeHealth; }
        set {
            if (value != _activeHealth) {
                _activeHealth = Mathf.Max(value, 0);
                HealthChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    public int MaxHealth {
        get { return _activeMaxHealth; }
        set {
            if (value != _activeMaxHealth) {
                _activeMaxHealth = Mathf.Max(value, 1);
                MaxHealthChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    public int Experience;
    public int Level;
    public int Damage;

    public float TurnSpeed;
    public float FireRate;
    public float FireRange;
    public float FireAngle;
    
    public float RadarScanFrequency;
    public float RadarScanRange;
    
    public void InitDefaults() {
        Health = _health;
        MaxHealth = _maxHealth;
        Experience = _experience;
        Level = _level;
        Damage = _damage;
    }
}