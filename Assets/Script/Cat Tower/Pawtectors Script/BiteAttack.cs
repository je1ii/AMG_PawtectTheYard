using System.Collections.Generic;
using UnityEngine;

public class BiteAttack : CatAttackBase
{
    public float attackRadius = 1.7f;

    private AudioSource feralBiteSFX;

    private void Start()
    {
        feralBiteSFX = GameObject.Find("Feral Bite")?.GetComponent<AudioSource>();
        if (feralBiteSFX == null)
            Debug.LogWarning("SFX Feral Bite not found in scene!");
    }

    public override bool Attack(List<Transform> enemies, Vector3 towerPos, TowerData towerData)
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

        Debug.Log($"Bite confirmed");

        if (feralBiteSFX != null)
            feralBiteSFX.Play();

        highestHPEnemy.GetComponentInChildren<EnemyHealthBar>().TakeDamage(towerData.biteLevel3);

        return true;
    }
}
