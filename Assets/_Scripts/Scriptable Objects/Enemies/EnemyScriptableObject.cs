using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemyScriptableObject : ScriptableObject {
    public Color Color = new(0.8f, 0.3f, 0.3f);
    public int Health = 1;
    public int Damage = 1;
    public float Speed = 1f;
    public float Scale = 1f;
    public float SpawnRadius = 18f;
    public int ExperienceForKill = 10;
    public EnemyType Type = EnemyType.Standard;
}

public enum EnemyType {
    Standard,
    Heavy
}
