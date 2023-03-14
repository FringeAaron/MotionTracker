using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class LevelUI : MonoBehaviour {
    private Text _levelText;
    private Image _experienceBar;
    private LevelSystem _levelSystem;
    private LevelSystemAnimated _levelSystemAnimated;

    private void Awake() {
        _levelText = transform.Find("Level Text").GetComponent<Text>();
        _experienceBar = transform.Find("Experience Bar").Find("Bar").GetComponent<Image>();

        transform.Find("Experience 5 Btn").GetComponent<Button_UI>().ClickFunc = () => _levelSystem.AddExperience(5);
        transform.Find("Experience 50 Btn").GetComponent<Button_UI>().ClickFunc = () => _levelSystem.AddExperience(50);
        transform.Find("Experience 500 Btn").GetComponent<Button_UI>().ClickFunc = () => _levelSystem.AddExperience(500);
    }

    private void SetExperienceBarSize(float experienceNormalised) {
        _experienceBar.fillAmount = experienceNormalised;
    }

    private void SetLevel(int level) {
        _levelText.text = "LEVEL " + (level);
    }

    public void SetLevelSystem(LevelSystem levelSystem) {
        _levelSystem = levelSystem;
    }
    public void SetLevelSystemAnimated(LevelSystemAnimated levelSystemAnimated) {
        _levelSystemAnimated = levelSystemAnimated;

        SetLevel(_levelSystemAnimated.GetLevel());
        SetExperienceBarSize(_levelSystemAnimated.GetExperienceNormalised());

        _levelSystemAnimated.OnExperienceChanged += LevelSystemAnimated_OnExperienceChanged;
        _levelSystemAnimated.OnLevelChanged += LevelSystemAnimated_OnLevelChanged;
    }

    private void LevelSystemAnimated_OnLevelChanged(object sender, System.EventArgs e) {
        SetLevel(_levelSystemAnimated.GetLevel());
    }

    private void LevelSystemAnimated_OnExperienceChanged(object sender, System.EventArgs e) {
        Debug.Log("LevelSystemAnimated Experience Event Triggered");
        SetExperienceBarSize(_levelSystemAnimated.GetExperienceNormalised());
    }
    

    void OnDestroy() {
        _levelSystemAnimated.OnExperienceChanged -= LevelSystemAnimated_OnExperienceChanged;
        _levelSystemAnimated.OnLevelChanged -= LevelSystemAnimated_OnLevelChanged;
    }
    
}