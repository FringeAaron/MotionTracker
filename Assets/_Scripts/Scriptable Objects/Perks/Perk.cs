using UnityEngine;

[CreateAssetMenu(menuName="Player Perk")]
public class Perk : ScriptableObject {    
    public string PerkName;
    [field: TextArea]
    public string Description;
    public Sprite Icon;    
    public float Multiplier;
}

public enum PerkType {
    HealthI,
    HealthII,
    HealthIII,
    TurnSpeedI,
    TurnSpeedII,
    TurnSpeedIII,
    ScanRangeI,
    ScanRangeII,
    ScanRangeIII
}