using UnityEngine;
using System.Collections;

public class CatnipFade : MonoBehaviour
{
    public float lifetime = 3f;
    public float fadeDuration = 1f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeAndDestroy());
    }

    IEnumerator FadeAndDestroy()
    {
        yield return new WaitForSeconds(lifetime);
        Debug.Log("Catnip fade start");

        float t = 0f;
        Color startColor = spriteRenderer.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        Debug.Log("Catnip destroyed");
        Destroy(gameObject);
    }
}
