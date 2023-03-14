using CodeMonkey.Utils;
using System;
using UnityEngine;

public class LevelSystemAnimated {
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;

    private LevelSystem _levelSystem;
    private bool _isAnimating;
    private int _level;
    private int _experience;
    private float _updateTimer;
    private readonly float _updateTimerMax;

    public LevelSystemAnimated(LevelSystem levelSystem) {
        SetLevelSystem(levelSystem);
        _updateTimerMax = .016f;
        FunctionUpdater.Create(() => Update());        
    }

    public void SetLevelSystem(LevelSystem levelSystem) {
        _levelSystem = levelSystem;

        _level = _levelSystem.GetLevel();
        _experience = _levelSystem.GetExperience();        

        _levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        _levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnExperienceChanged(object sender, System.EventArgs e) {
        _isAnimating = true;
        Debug.Log("LevelSystem Experience Event Triggered");
    }
    private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e) {
        _isAnimating = true;        
    }

    private void Update() {
        if (_isAnimating) {
            _updateTimer += Time.deltaTime;
            while (_updateTimer > _updateTimerMax) {
                _updateTimer -= _updateTimerMax;
                UpdateAddExperience();
            }
        }        
    }

    private void UpdateAddExperience() {
        if (_level < _levelSystem.GetLevel()) {
            AddExperience();
        } else {
            if (_experience < _levelSystem.GetExperience()) {
                AddExperience();
            } else {
                _isAnimating = false;
            }
        }
    }

    private void AddExperience() {
        _experience++;
        while (_experience >= _levelSystem.GetExperienceToNextLevel(_level)) {
            _level++;
            _experience = 0;
            OnLevelChanged?.Invoke(this, EventArgs.Empty);
        }

        OnExperienceChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetLevel() {
        return _level;
    }
    public int GetExperience() {
        return _experience;
    }
    public float GetExperienceNormalised() {
        return (float)_experience / _levelSystem.GetExperienceToNextLevel(_level);
    }    
}
