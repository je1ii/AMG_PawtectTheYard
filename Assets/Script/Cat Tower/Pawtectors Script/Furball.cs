using UnityEngine;

public class Furball : MonoBehaviour
{
    public float speed = 15f; // speed of the bullet
    public float deathRange = 0.5f; // how close the bullet to the target to be considered collided

    private int towerLevel;

    private Transform _enemy; // holds the target
    private Vector2 _direction; // holds the direction of the bullet after spawn

    void Update()
    {
        // move bullet in the right direction and speed
        this.transform.position += (Vector3)_direction * (speed * Time.deltaTime);
        
        if (_enemy != null) 
        {
            // checks the enemy position
            var distance = Mathf.Sqrt(Mathf.Pow(_enemy.position.x - this.transform.position.x, 2) + Mathf.Pow(_enemy.position.y - this.transform.position.y, 2));
            if (distance < deathRange) // checks if the bullet collided with the enemy
            {
                EnemyHealthBar health = _enemy.GetComponentInChildren<EnemyHealthBar>();
                if (health != null)
                {
                    switch (towerLevel)
                    {
                        case 1:
                            health.TakeDamage(8f);
                            break;
                        case 2:
                            health.TakeDamage(15f);
                            break;
                        case 3:
                            health.TakeDamage(25f);
                            break;
                    }
                }
                Destroy(gameObject);
            }
        }
    }

    public void GetTowerLevel(int level)
    {
        towerLevel = level;
    }
    
    public void SetDirection(Vector2 dir)
    {
        _direction = dir.normalized;
    }
    
    public void SetTarget(Transform e)
    {
        _enemy = e;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject); 
    }
}
