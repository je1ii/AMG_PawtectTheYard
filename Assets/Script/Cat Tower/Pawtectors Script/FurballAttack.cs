using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Serialization;

public class FurballAttack : MonoBehaviour
{
    public float rotSpeed = 4.5f; // how fast the turret turning
    public float fireRange = 4f; // the range of the turret
    public float fireInterval = 2.5f; // fire rate of the turret

    [SerializeField] private Transform target; // holds the target
    public GameObject furballPrefab; // holds bullet prefab

    // controls the turret state
    private bool _isFiring = false; 
    private bool _stopFiring = false;

    void Update()
    {
        if (target == null || !IsTargetInRange(target))
        {
            FindTarget();
        }
        
        if (target == null) return;

        // points the turret to the target
        var direction = target.position - transform.position;
        direction.Normalize();
        
        // converts direction -> radians -> degrees and then -90 to match sprite rotation
        var targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
        
        // get current angle and smoothly matches targetAngle
        var currentAngle = transform.eulerAngles.z;
        var newAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, newAngle);

        if (IsTargetInRange(target)) // checks if in range
        {
            if (!_isFiring)
            {
                _isFiring = true;
                _stopFiring = false;
                _ = FireLoop(); // fire without blocking main thread
            }
        }
        else
        {
            _stopFiring = true;
        }
    }
    
    private void FindTarget()
    { 
        // checks active enemies and returns null if none
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            target = null;
            return;
        }
        
        // Instead of finding the *closest*, just find the *first* one in range
        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            var distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= fireRange)
            {
                target = enemy.transform;
                return; // stop after finding the first valid enemy
            }
        }

        // No enemies in range
        target = null;
    }
    
    private bool IsTargetInRange(Transform potentialTarget)
    {
        if (potentialTarget == null) return false;
        // calculates the distance between target and turret
        var distance = Mathf.Sqrt(Mathf.Pow(target.position.x - this.transform.position.x, 2) + Mathf.Pow(target.position.y - this.transform.position.y, 2));
        return distance <= fireRange;
    }

    private async Task FireLoop()
    {
        while (!_stopFiring)
        {
            // fire bullet after fireInterval
            FireBullet();
            await Task.Delay((int)(fireInterval * 1000));
        }

        // reset when loop ends
        _isFiring = false;
    }

    private void FireBullet()
    {
        if (target == null) return;
        
        // sets the direction and target of the bullet 
        Vector2 dir = target.position - transform.position;
        GameObject furball = Instantiate(furballPrefab, transform.position, transform.rotation);
        furball.GetComponent<Furball>().SetDirection(dir);
        furball.GetComponent<Furball>().SetTarget(target);
    }
}
