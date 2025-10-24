using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "TowerData", menuName = "Scriptable Objects/TowerData")]
public class TowerData : ScriptableObject
{
    [Header("General Info")]
    public TowerName towerName;
    public Sprite towerIcon;
    public GameObject prefab;
    public int currentLevel;
    public int levelCost;
    
    [Header("Abilities Damage")]
    public float furballLevel1;
    public float furballLevel2;
    public float furballLevel3;
    public float clawLevel2;
    public float clawLevel3;
    public float biteLevel3;

    [Header("Others")] public int attacksBeforeBite;

}

public enum TowerName
{
    Empty,
    Maja,
    Tustado,
    Keso
}
