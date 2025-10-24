using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClawAttack : CatAttackBase
{
    public int maxTargets = 3;
    public float attackRadius = 0.8f;
    public override bool Attack(List<Transform> enemies, Vector3 towerPos, TowerData towerData)
    {
        if (enemies == null || enemies.Count == 0) return false;

        // Filter only valid melee enemies within attackRadius
        var meleeEnemies = enemies
            .Where(e => e != null && Vector2.Distance(e.position, towerPos) <= attackRadius)
            .OrderBy(e => Vector2.Distance(e.position, towerPos))
            .Take(maxTargets)
            .ToList();

        if (meleeEnemies.Count == 0)
        {
            Debug.Log("Claw attack: no enemies in melee range");
            return false;
        }

        // Damage each one
        foreach (var enemy in meleeEnemies)
        {
            var hp = enemy.GetComponentInChildren<EnemyHealthBar>();
            if (hp == null)
            {
                Debug.LogWarning($"[ClawAttack] {enemy.name} has no EnemyHealthBar component!");
                continue;
            }

            float damage = (towerData.currentLevel == 3) ? towerData.clawLevel3 : towerData.clawLevel2;
            hp.TakeDamage(damage);
            Debug.Log($"Claw hit {enemy.name} for {damage} damage!");
        }

        return true;
    }
}
