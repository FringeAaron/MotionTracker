using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player", order = 1)]
public class PlayerScriptableObject : ScriptableObject {
    public Color32 Color;    
    public Vector3 Scale;
    public Sprite Icon;

    public PlayerHealthValue Health;
    //public int Health;
    public int Experience;
    public int Level;
    public int Damage;

    public float TurnSpeed;
    public float FireRate;
    public float FireRange;
    public float FireAngle;
    
    public float RadarScanFrequency;
    public float RadarScanRange;
    
}