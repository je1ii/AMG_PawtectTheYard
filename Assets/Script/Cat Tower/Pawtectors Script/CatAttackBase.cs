using UnityEngine;
using System.Collections.Generic;

public abstract class CatAttackBase : MonoBehaviour
{
    public abstract bool Attack(List<Transform> enemies, Vector3 towerPos, int towerLevel);
}
