using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

public class CatTower : MonoBehaviour
{
    public float meleeRange = 1f;
    public float longRange = 2.5f;
    public float rotSpeed = 4f;
    public float attackInterval = 2f;

    private int attackCounter = 0;
    private Transform currentTarget;
    private Coroutine attackCoroutine;
    
    public CatAttackBase furballAttack;
    public CatAttackBase clawAttack;
    public CatAttackBase biteAttack;
    
    [SerializeField] private List<Transform> enemiesInRange = new List<Transform>();
    
    public int slotParent;
    public int towerLevel;

    [SerializeField] private string[] clawAnim = { "ClawL", "ClawR" };
    private Animator catAnimator;

    void Awake()
    {
        catAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        UpdateEnemiesInRange();
        UpdateCurrentTarget();
        RotateTowardTarget();

        // If there's a valid target and no attack loop, start one
        if (currentTarget != null && attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(AttackLoop());
        }
        // If no target and a loop is running, stop it
        else if (currentTarget == null && attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    private IEnumerator AttackLoop()
    {
        while (currentTarget != null)
        {
            float distance = FindDistance(currentTarget);
            Attack(distance);

            // Wait before next attack
            yield return new WaitForSeconds(attackInterval);

            // Stop loop if target goes out of range
            if (currentTarget == null || FindDistance(currentTarget) > longRange)
                break;
        }

        // Cleanup
        attackCoroutine = null;
    }

    private async void Attack(float distance)
    {
        try
        {
            if (this.gameObject == null) return;
            switch (towerLevel)
            {
                case 1:
                    await PlayFurballAnim();
                    if (this.gameObject != null)furballAttack.Attack(enemiesInRange, transform.position, towerLevel);
                    break;

                case 2:
                    if (distance <= meleeRange)
                    {
                        Debug.Log("Claw Lvl 3");
                        await PlayClawAnim();
                        clawAttack.Attack(enemiesInRange, transform.position, towerLevel);
                    }
                    else
                    {
                        await PlayFurballAnim();
                        furballAttack.Attack(enemiesInRange, transform.position, towerLevel);
                    }
                    break;

                case 3:
                    if (attackCounter >= 5)
                    {
                        Debug.Log("Bite Lvl 3");
                        await PlayBiteAnim();
                        biteAttack.Attack(enemiesInRange, transform.position, towerLevel);
                        attackCounter = 0;
                    }
                    else if (distance <= meleeRange)
                    {
                        Debug.Log("Claw Lvl 3");
                        await PlayClawAnim();
                        clawAttack.Attack(enemiesInRange, transform.position, towerLevel);
                        attackCounter++;
                    }
                    else
                    {
                        await PlayFurballAnim();
                        furballAttack.Attack(enemiesInRange, transform.position, towerLevel);
                        attackCounter++;
                    }
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Cat tower replaced with null exception error, we catched it!");
        }
    }
    
    private void UpdateEnemiesInRange()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Clean existing list
        enemiesInRange.RemoveAll(e => e == null || FindDistance(e) > longRange);

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            float distance = FindDistance(enemy.transform);
            if (distance <= longRange && !enemiesInRange.Contains(enemy.transform))
            {
                enemiesInRange.Add(enemy.transform);
            }
        }
    }
    
    private void UpdateCurrentTarget()
    {
        // Keep current melee target locked if still valid
        if (currentTarget != null && FindDistance(currentTarget) <= meleeRange)
            return;

        // Find enemies within melee range
        Transform closestMelee = enemiesInRange
            .Where(e => e != null && FindDistance(e) <= meleeRange)
            .OrderBy(e => FindDistance(e))
            .FirstOrDefault();

        if (closestMelee != null)
        {
            // Prioritize melee enemy
            if (currentTarget != closestMelee)
            {
                currentTarget = closestMelee;
                Debug.Log($"[Tower] Switched to melee");
            }
            return;
        }

        // No melee enemies -> find closest long-range target
        if (currentTarget == null || FindDistance(currentTarget) > longRange)
        {
            currentTarget = enemiesInRange
                .Where(e => e != null)
                .OrderBy(e => FindDistance(e))
                .FirstOrDefault();

            if (currentTarget != null)
                Debug.Log($"[Tower] Switched to long-range");
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

    private async Task PlayFurballAnim()
    {
        catAnimator.SetTrigger("Furball");
        await Task.Delay(1125);
    }

    private async Task PlayClawAnim()
    {
        var clawSide = clawAnim[UnityEngine.Random.Range(0, clawAnim.Length)];
        catAnimator.SetTrigger(clawSide);
        await Task.Delay(750);
    }

    private async Task PlayBiteAnim()
    {
        catAnimator.SetTrigger("Bite");
        await Task.Delay(1100);
    }

    private float FindDistance(Transform target)
    {
        var distance = Mathf.Sqrt(Mathf.Pow(target.transform.position.x - this.transform.position.x, 2) + Mathf.Pow(target.transform.position.y - this.transform.position.y, 2));
        return distance;
    }
    
    public void AssignSlot(int slot) => slotParent = slot;
    public void AssignLevel(int level) => towerLevel = level;
    public void AssignAttackInterval(float interval) => attackInterval = interval;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, longRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
