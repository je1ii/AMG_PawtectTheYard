using UnityEngine;
using System.Collections;

public class CameraDamageEffect : MonoBehaviour
{
    public Camera mainCam;
    public float shakeDuration = 0.3f;
    public float shakeStrength = 0.2f;
    public float flashDuration = 0.5f;
    public Color flashColor = Color.red;

    private Color originalColor;
    private bool isShaking = false;

    void Start()
    {
        if (mainCam == null)
            mainCam = Camera.main;

        originalColor = mainCam.backgroundColor;
    }

    public void TriggerDamageEffect()
    {
        if (!isShaking)
            StartCoroutine(DamageEffect());
    }

    IEnumerator DamageEffect()
    {
        // Start both shake and flash
        StartCoroutine(CameraShake());
        yield return StartCoroutine(CameraFlash());
    }

    IEnumerator CameraShake()
    {
        isShaking = true;
        Vector3 originalPos = transform.position;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            float x = Random.Range(-1f, 1f) * shakeStrength;
            float y = Random.Range(-1f, 1f) * shakeStrength;
            transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            yield return null;
        }

        transform.position = originalPos;
        isShaking = false;
    }

    IEnumerator CameraFlash()
    {
        float elapsed = 0f;

        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.PingPong(elapsed * 2f, 1f);
            mainCam.backgroundColor = Color.Lerp(originalColor, flashColor, t);
            yield return null;
        }

        mainCam.backgroundColor = originalColor;
    }
}
