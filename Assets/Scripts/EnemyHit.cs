using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    private SpriteRenderer enemyRenderer;
    public Color hitColor = Color.red;
    public float flashDuration = 0.4f;

    private Color originalColor;
    private float lerpTime;
    private bool isHit;

    void Start()
    {
        enemyRenderer = GetComponent<SpriteRenderer>();
        originalColor = enemyRenderer.color;
    }

    void Update()
    {
        if (isHit)
        {
            lerpTime += Time.deltaTime / flashDuration;
            enemyRenderer.color = Color.Lerp(hitColor, originalColor, lerpTime);

            if (lerpTime >= 1f)
            {
                isHit = false;
                lerpTime = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
        {
            Debug.Log("Hit detected!");
            isHit = true;
            lerpTime = 0f;
        }
    }

// Yung bullets/projectile ng tower lagyan niyo "Bullet" TAG, RIGIDBODY2 gravity scale = 0,  CIRCLE COLLIDER2d Trigger on.
// Yung Enemies any COLLIDER2d lng Trigger off.

