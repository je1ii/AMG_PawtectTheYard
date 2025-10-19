using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Scriptable Objects/TowerData")]
public class TowerData : ScriptableObject
{
    public string towerName;
    public Sprite towerIcon;
    public GameObject prefab;
    public int cost;
}
