using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraDamageEffect : MonoBehaviour
{
    public Camera mainCam;
    public float detectRange = 1f;
    public float shakeStrength = 0.02f;
    public float shakeTime = 0.01f; 
    public Image redFlash; 

    bool isEffectActive = false;
    Transform path4;
    Vector3 originalPos;

    void Start()
    {
        if (!mainCam) mainCam = Camera.main;
        path4 = GameObject.FindGameObjectWithTag("EndPath").transform;
        originalPos = mainCam.transform.position;

        if (redFlash != null)
            redFlash.color = new Color(1, 0, 0, 0); 
    }

    public void OnHit()
    {
        if (isEffectActive) return;

        StartCoroutine(DoEffect());
    }

    private IEnumerator DoEffect()
    {
        isEffectActive = true;

        // Shake
        float t = 0f;
        while (t < shakeTime)
        {
            t += Time.deltaTime;
            mainCam.transform.position = originalPos + Random.insideUnitSphere * shakeStrength;
            yield return null;
        }
        mainCam.transform.position = originalPos;

        // Quick red flash
        if (redFlash != null)
        {
            redFlash.color = new Color(1, 0, 0, 0.4f);
            yield return new WaitForSeconds(0.05f);
            redFlash.color = new Color(1, 0, 0, 0);
        }

        isEffectActive = false;
    }
}