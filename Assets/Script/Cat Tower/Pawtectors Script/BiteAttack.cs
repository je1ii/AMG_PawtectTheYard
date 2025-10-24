using System.Collections.Generic;
using UnityEngine;

public class BiteAttack : CatAttackBase
{
    public float attackRadius = 1.7f;
    public override bool Attack(List<Transform> enemies, Vector3 towerPos, int towerLevel)
    {
        if (enemies == null || enemies.Count == 0) 
            return false;

        Transform highestHPEnemy = null;
        var maxHP = float.MinValue;

         foreach (Transform enemy in enemies)
         {
             if (enemy == null) continue;
        
             float distance = Vector2.Distance(enemy.position, towerPos);
             if (distance > attackRadius) continue; // skip enemies too far away
        
             EnemyHealthBar stats = enemy.GetComponentInChildren<EnemyHealthBar>();
             if (stats != null && stats._currentHealth > maxHP)
             {
                 highestHPEnemy = enemy;
                 maxHP = stats._currentHealth;
             }
         }
        
         if (highestHPEnemy == null)
         {
             Debug.Log("Bite attack: no valid targets in range");
             return false;
         }

        //Play bite animation here
        Debug.Log($"Bite confirmed");

        highestHPEnemy.GetComponentInChildren<EnemyHealthBar>().TakeDamage(60f);

        return true;
    }
}
