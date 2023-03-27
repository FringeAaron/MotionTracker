using UnityEngine;


public class TargetDamageStats : MonoBehaviour {
    public PlayerStats TargetStats;
    public int Damage;

    public void CommitDamage() {
        TargetStats.Health -= Damage;
    }
}