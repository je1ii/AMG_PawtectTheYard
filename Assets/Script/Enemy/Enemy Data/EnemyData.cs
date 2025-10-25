using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("General Info")]
    public EnemyName enemyName;
    public GameObject prefab;

    [Header("Stats")]
    public float startHealth;
    public float endHealth;
    public float startSpeed;
    public float endSpeed;
    public float damageToPlayer;
    public GameObject catnipPrefab;
    
    [Header("Animation Trigger Name")]
    public string[] animation;
}

public enum EnemyName
{
    Roach,
    Gerry,
    Viper
}
