using System.Collections.Generic;
using UnityEngine;

public class BiteAttack : CatAttackBase
{
    public float damage = 100f;
    public float attackRadius = 2.5f;
    public override bool Attack(List<Transform> enemies, Vector3 towerPos)
    {
        if (enemies == null || enemies.Count == 0) 
            return false;

        Transform highestHPEnemy = null;
        float maxHP = float.MinValue;

        // foreach (Transform enemy in enemies)
        // {
        //     if (enemy == null) continue;
        //
        //     float distance = Vector2.Distance(enemy.position, towerPos);
        //     if (distance > attackRadius) continue; // skip enemies too far away
        //
        //     EnemyStats stats = enemy.GetComponent<EnemyStats>();
        //     if (stats != null && stats.currentHP > maxHP)
        //     {
        //         highestHPEnemy = enemy;
        //         maxHP = stats.currentHP;
        //     }
        // }
        //
        // if (highestHPEnemy == null)
        // {
        //     Debug.Log("Bite attack: no valid targets in range");
        //     return false;
        // }

        // Play bite animation here
        //Debug.Log($"Bite attack! Bit {highestHPEnemy.name} for {damage} damage!");
        Debug.Log($"Bite confirmed");

        // Example damage call (uncomment when EnemyStats exists)
        // highestHPEnemy.GetComponent<EnemyStats>().TakeDamage(damage);

        return true;
    }
}
