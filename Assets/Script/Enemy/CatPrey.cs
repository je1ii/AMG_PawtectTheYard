using UnityEngine;
using System.Collections;

public class CatPrey : MonoBehaviour
{
    [Header("Enemy Data")]
    public EnemyData enemyData;

    [Header("Components")]
    public SpriteRenderer spriteRenderer;

    [Header("Path Points (for curve path)")]
    public Transform path1, path2, path3, path4;

    [Header("Path Settings")]
    [SerializeField] private float curveDepth = 6f;
    private float totalPathLength;
    private float traveledDistance;
    private Vector3 startOffset = Vector3.zero;

    private bool isDead = false;
    private Animator animator;

    private AudioSource roachDeathSFX;
    private AudioSource gerryDeathSFX;
    private AudioSource viperDeathSFX;
    private AudioSource catnipDropSFX; 

    public void SetData(EnemyData data) => enemyData = data;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        roachDeathSFX = GameObject.Find("SFX Roach Death")?.GetComponent<AudioSource>();
        gerryDeathSFX = GameObject.Find("SFX Gerry Death")?.GetComponent<AudioSource>();
        viperDeathSFX = GameObject.Find("SFX Viper Death")?.GetComponent<AudioSource>();
        catnipDropSFX = GameObject.Find("SFX Catnip Drop")?.GetComponent<AudioSource>(); 

        if (enemyData == null)
        {
            Debug.LogError($"{name}: Missing EnemyData!");
            return;
        }

        CalculatePathLength();
        transform.position = path1.position + startOffset;
    }

    private void Update()
    {
        if (isDead || enemyData == null) return;
        MoveAlongCurvePath(enemyData.startSpeed);
    }

    private void CalculatePathLength()
    {
        float vLength = Vector3.Distance(path2.position, (path2.position + path3.position) / 2f + Vector3.down * curveDepth)
                      + Vector3.Distance((path2.position + path3.position) / 2f + Vector3.down * curveDepth, path3.position);

        totalPathLength = Vector3.Distance(path1.position, path2.position)
                        + vLength
                        + Vector3.Distance(path3.position, path4.position);
    }

    private void MoveAlongCurvePath(float speed)
    {
        traveledDistance += speed * Time.deltaTime;
        if (traveledDistance >= totalPathLength)
        {
            OnReachEnd();
            return;
        }

        float dist12 = Vector3.Distance(path1.position, path2.position);
        float dist23 = Vector3.Distance(path2.position, (path2.position + path3.position) / 2f + Vector3.down * curveDepth)
                      + Vector3.Distance((path2.position + path3.position) / 2f + Vector3.down * curveDepth, path3.position);
        float dist34 = Vector3.Distance(path3.position, path4.position);

        Vector3 newPos;

        if (traveledDistance <= dist12)
        {
            float t = traveledDistance / dist12;
            newPos = Vector3.Lerp(path1.position, path2.position, t);
        }
        else if (traveledDistance <= dist12 + dist23)
        {
            float t = (traveledDistance - dist12) / dist23;

            Vector3 start = path2.position + startOffset;
            Vector3 end = path3.position + startOffset;
            Vector3 control = (path2.position + path3.position) / 2f + Vector3.down * curveDepth + startOffset;

            newPos = Mathf.Pow(1 - t, 2) * start + 2 * (1 - t) * t * control + Mathf.Pow(t, 2) * end;
        }
        else
        {
            float t = (traveledDistance - dist12 - dist23) / dist34;
            newPos = Vector3.Lerp(path3.position + startOffset, path4.position + startOffset, t);
        }

        Vector3 dir = newPos - transform.position;
        if (dir != Vector3.zero)
            RotateTowardDirection(dir.normalized);

        transform.position = newPos;
    }

    private void RotateTowardDirection(Vector3 direction)
    {
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        float currentAngle = spriteRenderer.transform.eulerAngles.z;
        float rotSpeed = 10f;
        float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotSpeed * Time.deltaTime);
        spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, newAngle);
    }

    public void OnHit()
    {
        StartCoroutine(FlashHit());
    }

    private IEnumerator FlashHit()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        switch (enemyData.enemyName)
        {
            case EnemyName.Roach:
                animator.SetTrigger(enemyData.animation[0]);
                if (roachDeathSFX != null) roachDeathSFX.Play();
                break;
            case EnemyName.Gerry:
                animator.SetTrigger(enemyData.animation[Random.Range(0, enemyData.animation.Length)]);
                if (gerryDeathSFX != null) gerryDeathSFX.Play();
                break;
            case EnemyName.Viper:
                animator.SetTrigger(enemyData.animation[1]);
                if (viperDeathSFX != null) viperDeathSFX.Play();
                break;
        }

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        CanvasGroup cg = GetComponentInChildren<CanvasGroup>();
        Color c = spriteRenderer.color;

        float fadeDuration = 1f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

            c.a = alpha;
            spriteRenderer.color = c;

            if (cg != null)
                cg.alpha = alpha;

            yield return null;
        }

        if (cg != null) cg.alpha = 0f;
        c.a = 0f;
        spriteRenderer.color = c;

        if (catnipDropSFX != null)
            catnipDropSFX.Play();

        if (enemyData.catnipPrefab != null)
            Instantiate(enemyData.catnipPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void OnReachEnd()
    {
        Debug.Log($"{name} reached the end!");
        Destroy(gameObject);
    }
}
