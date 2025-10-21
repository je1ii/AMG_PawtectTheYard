using UnityEngine;
using System.Collections;

public class EnemyDrop : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth = 30;
    public float speed = 2f;
    public float fadeDuration = 1f;

    [Header("Catnip Settings")]
    public GameObject catnipPrefab; // Drag your Catnip prefab here

    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(8); // Bullet damage
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
        Debug.Log("Enemy fade start");

        yield return StartCoroutine(FadeOut(spriteRenderer, "Enemy"));

        Debug.Log("Enemy fade complete!");

        // Now spawn Catnip after the enemy has faded
        Instantiate(catnipPrefab, transform.position, Quaternion.identity);
        Debug.Log("Catnip dropped after enemy destroyed.");

        Destroy(gameObject);
        Debug.Log("Enemy object destroyed.");
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
            Debug.Log($"{objectName} fading... alpha: {alpha:F2}");
            yield return null;
        }

        renderer.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
    }
}
