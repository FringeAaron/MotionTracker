using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour {
    public PlayerStats PlayerStats;
    
    [SerializeField] private Image _healthDisplay;

    private void Start() {
        _healthDisplay.fillAmount = (float)PlayerStats.Health / (float)PlayerStats.MaxHealth;
    }

    private void OnEnable() {
        PlayerStats.HealthChanged += UpdateHealthDisplay;
    }

    private void OnDisable() {
        PlayerStats.HealthChanged -= UpdateHealthDisplay;
    }

    private void UpdateHealthDisplay(object sender, EventArgs e) { 
        _healthDisplay.fillAmount = (float)PlayerStats.Health / (float)PlayerStats.MaxHealth;
    }
}
