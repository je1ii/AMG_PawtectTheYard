using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FurballAttack : CatAttackBase
{
    public GameObject furballPrefab;
    
    public override bool Attack(List<Transform> enemies, Vector3 towerPos, int towerLevel)
    {
        if (enemies == null || enemies.Count == 0) return false;
        
        Transform target = enemies[0];
        if (target == null) return false;
        
        Vector2 dir = target.position - towerPos;
        GameObject furball = Instantiate(furballPrefab, towerPos, Quaternion.identity);
        furball.GetComponent<Furball>().GetTowerLevel(towerLevel);
        furball.GetComponent<Furball>().SetDirection(dir);
        furball.GetComponent<Furball>().SetTarget(target);

        return true;
    }
}
