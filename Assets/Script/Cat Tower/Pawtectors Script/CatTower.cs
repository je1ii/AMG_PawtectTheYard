using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class CatTower : MonoBehaviour
{
    public float meleeRange = 0.8f;
    public float longRange = 1.7f;
    public float attackInterval = 2f;
    public float rotSpeed = 4f;

    private int attackCounter = 0;
    private bool isFiring = false;
    private bool stopFiring = false;
    private Transform currentTarget;
    
    public CatAttackBase furballAttack;
    public CatAttackBase clawAttack;
    public CatAttackBase biteAttack;
    
    [SerializeField] private List<Transform> enemiesInRange = new List<Transform>();
    
    public int slotParent;
    public int towerLevel;
    
    void Update()
    {
        UpdateEnemiesInRange();
        UpdateCurrentTarget();
        
        if (currentTarget == null)
        {
            stopFiring = true;
            isFiring = false;
            return;
        }

        RotateTowardTarget();
        
        if (!isFiring)
        {
            isFiring = true;
            stopFiring = false;
            _ = AttackLoop(); // fire without blocking main thread
        }
    }

    private async Task AttackLoop()
    {
        while (!stopFiring)
        {
            var distance = FindDistance(currentTarget);
            Attack(distance);
            await Task.Delay((int)(attackInterval * 1000));
            
            if (currentTarget == null || FindDistance(currentTarget) > longRange)
            {
                stopFiring = true;
                isFiring = false;
                break;
            }
        }
        isFiring = false;
    }

    private void Attack(float distance)
    {
        switch (towerLevel)
        {
            case 1:
                Debug.Log("Furball attack!");
                furballAttack.Attack(enemiesInRange, transform.position);
                break;

            case 2:
                if (distance <= meleeRange)
                {
                    Debug.Log("Claw attack!");
                    clawAttack.Attack(enemiesInRange, transform.position);
                }
                else
                {
                    Debug.Log("Furball attack!");
                    furballAttack.Attack(enemiesInRange, transform.position);
                }
                break;

            case 3:
                if (attackCounter >= 5)
                {
                    Debug.Log("Bite attack!");
                    biteAttack.Attack(enemiesInRange, transform.position);
                    attackCounter = 0;
                }
                else if (distance <= meleeRange)
                {
                    Debug.Log("Claw attack!");
                    clawAttack.Attack(enemiesInRange, transform.position);
                    attackCounter++;
                }
                else
                {
                    Debug.Log("Furball attack!");
                    furballAttack.Attack(enemiesInRange, transform.position);
                    attackCounter++;
                }
                break;
        }
    }
    
    private void UpdateEnemiesInRange()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = enemiesInRange.Count - 1; i >= 0; i--)
        {
            Transform enemy = enemiesInRange[i];
            if (enemy == null || FindDistance(enemy) > longRange)
                enemiesInRange.RemoveAt(i);
        }

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            var distance = FindDistance(enemy.transform);
            if (distance <= longRange && !enemiesInRange.Contains(enemy.transform))
            {
                enemiesInRange.Add(enemy.transform);
            }
        }
    }
    
    private void UpdateCurrentTarget()
    {
        if (currentTarget == null || !enemiesInRange.Contains(currentTarget))
        {
            if (enemiesInRange.Count > 0)
                currentTarget = enemiesInRange[0]; 
            else
                currentTarget = null;
        }
    }
    
    private void RotateTowardTarget()
    {
        if (currentTarget == null) return;

        var direction = currentTarget.position - transform.position;
        direction.Normalize();

        var targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        var currentAngle = transform.eulerAngles.z;
        var newAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, newAngle);
    }

    private float FindDistance(Transform target)
    {
        var distance = Mathf.Sqrt(Mathf.Pow(target.transform.position.x - this.transform.position.x, 2) + Mathf.Pow(target.transform.position.y - this.transform.position.y, 2));
        return distance;
    }
    
    public void AssignSlot(int slot)
    {
        slotParent = slot;
    }
    
    public void AssignLevel(int level)
    {
        towerLevel = level;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, longRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
