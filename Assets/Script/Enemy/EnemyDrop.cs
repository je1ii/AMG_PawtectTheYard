using UnityEngine;
using System.Collections;

public class EnemyDrop : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth = 30;
    public float fadeDuration = 1f;

    [Header("Catnip Settings")]
    public GameObject catnipPrefab;

    [Header("Hit Detection")]
    public float hitRange = 0.5f; // how close the bullet must be to count as a hit

    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");

        foreach (GameObject bullet in bullets)
        {
            float distance = Vector2.Distance(transform.position, bullet.transform.position);

            if (distance <= hitRange)
            {
                TakeDamage(8);
                Destroy(bullet);
                break; 
            }
        }
    }

    void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log("Enemy HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Enemy eliminated!");
        StartCoroutine(FadeAndDestroyEnemy());
    }

    IEnumerator FadeAndDestroyEnemy()
    {
        yield return StartCoroutine(FadeOut(spriteRenderer, "Enemy"));

        Instantiate(catnipPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator FadeOut(SpriteRenderer renderer, string objectName)
    {
        Color startColor = renderer.color;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            renderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        renderer.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
    }
}
