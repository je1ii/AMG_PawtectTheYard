using System.Collections.Generic;
using UnityEngine;

public class ClawAttack : CatAttackBase
{
    public float damage = 10f;
    public int maxTargets = 3;
    public float attackRadius = 1.5f;
    public override bool Attack(List<Transform> enemies, Vector3 towerPos)
    {
        if (enemies == null || enemies.Count == 0) return false;

        int hits = 0;
        foreach (Transform enemy in enemies)
        {
            if (enemy == null) continue;

            float distance = Vector2.Distance(enemy.position, towerPos);

            if (distance <= attackRadius)
            {
                Debug.Log($"Claw hit {enemy.name} for {damage} damage!");
                // enemy.GetComponent<Enemy>().TakeDamage(damage);
                hits++;

                if (hits >= maxTargets)
                    break;
            }
        }

        if (hits == 0)
            Debug.Log("Claw attack: no enemies in melee range");

        return hits > 0;
    }
}
