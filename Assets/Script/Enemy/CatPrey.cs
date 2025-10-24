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

    public void SetData(EnemyData data) => enemyData = data;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

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
        // Approximate curve path for timing consistency
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
            // Path 1 → 2 (straight)
            float t = traveledDistance / dist12;
            newPos = Vector3.Lerp(path1.position, path2.position, t);
        }
        else if (traveledDistance <= dist12 + dist23)
        {
            // Path 2 → 3 (curved)
            float t = (traveledDistance - dist12) / dist23;

            Vector3 start = path2.position + startOffset;
            Vector3 end = path3.position + startOffset;
            Vector3 control = (path2.position + path3.position) / 2f + Vector3.down * curveDepth + startOffset;

            newPos = Mathf.Pow(1 - t, 2) * start + 2 * (1 - t) * t * control + Mathf.Pow(t, 2) * end;
        }
        else
        {
            // Path 3 → 4 (straight)
            float t = (traveledDistance - dist12 - dist23) / dist34;
            newPos = Vector3.Lerp(path3.position + startOffset, path4.position + startOffset, t);
        }

        // Rotate toward next position
        Vector3 dir = newPos - transform.position;
        if (dir != Vector3.zero)
        {
            RotateTowardDirection(dir.normalized);
        }

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
                break;
            case EnemyName.Gerry:
                // pick random animation
                animator.SetTrigger(enemyData.animation[Random.Range(0, enemyData.animation.Length)]);
                break;
            case EnemyName.Viper:
                // brown 0 (not set), green 1
                animator.SetTrigger(enemyData.animation[1]);
                break;
        }

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        Color c = spriteRenderer.color;
        while (c.a > 0)
        {
            c.a -= Time.deltaTime;
            spriteRenderer.color = c;
            yield return null;
        }

        // if (enemyData.catnipPrefab != null)
        //     Instantiate(enemyData.catnipPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void OnReachEnd()
    {
        Debug.Log($"{name} reached the end!");
        Destroy(gameObject);
    }
}
