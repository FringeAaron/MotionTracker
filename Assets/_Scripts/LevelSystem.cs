using System;
using UnityEngine;

public class LevelSystem {
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;

    private int _level;
    private int _experience;    


    public LevelSystem() {
        _level = 1;
        _experience = 0;        
    }

    public void AddExperience(int amt) {
        _experience += amt;
        var experienceToNextLevel = GetExperienceToNextLevel(_level);
        while (_experience >= experienceToNextLevel) {
            _level++;
            _experience -= experienceToNextLevel;
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
    public int GetExperienceToNextLevel(int level){
        return (int)Mathf.Floor(100 * level * Mathf.Pow(level, 0.1f));
    }

    public float GetExperienceNormalised() {
        return (float)_experience / GetExperienceToNextLevel(_level);
    }
}