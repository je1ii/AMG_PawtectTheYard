using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("General Info")]
    public EnemyName enemyName;
    public GameObject prefab;

    [Header("Stats")]
    public float startHealth;
    public float startSpeed;
    public float midHealth;
    public float midSpeed;
    public float endHealth;
    public float endSpeed;
    public float damageToPlayer;
    
    [Header("Early (2), Mid (1), End (0)")]
    public GameObject[] catnipPrefabs;
    
    [Header("Animation Trigger Name")]
    public string[] animation;
}

public enum EnemyName
{
    Roach,
    Gerry,
    Viper
}
