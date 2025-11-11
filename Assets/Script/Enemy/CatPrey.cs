using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class CatPrey : MonoBehaviour
{
    [Header("Enemy Data")]
    public EnemyData enemyData;

    [Header("Components")]
    public SpriteRenderer spriteRenderer;

    [Header("Path Points")]
    public Transform[] paths;

    [Header("Path Settings")]
    private float totalPathLength;
    private float traveledDistance;
    private Vector3 startOffset = Vector3.zero;

    private bool isDead = false;
    private Animator animator;
    private bool isMidGame = false;
    private bool isEndGame = false;

    private AudioSource roachDeathSFX;
    private AudioSource gerryDeathSFX;
    private AudioSource viperDeathSFX;
    private AudioSource catnipDropSFX; 

    public void SetData(EnemyData data) => enemyData = data;

    public void SetGameState(bool mid, bool end)
    {
        isEndGame = end;
        isMidGame = mid;
    }

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

        if (paths == null || paths.Length == 0)
        {
            Debug.LogError($"{name}: Path points not assigned!");
            return;
        }

        CalculatePathLength();
        transform.position = paths[0].position + startOffset;
    }

    private void Update()
    {
        if (isDead || enemyData == null) return;
        MoveAlongPath(enemyData.startSpeed);
    }

    private void CalculatePathLength()
    {
        float length = 0f;

        // Go through each segment between points
        for (int i = 0; i < paths.Length - 1; i++)
        {
            // Add the distance between consecutive points
            length += Vector3.Distance(paths[i].position, paths[i + 1].position);
        }

        totalPathLength = length;
    }

    private void MoveAlongPath(float speed)
    {
        if (paths == null || paths.Length < 2)
            return;

        traveledDistance += speed * Time.deltaTime;

        // If total path length isn't computed yet, calculate it
        if (totalPathLength <= 0f)
            CalculatePathLength();

        if (traveledDistance >= totalPathLength)
        {
            OnReachEnd();
            return;
        }

        // Find which segment we're currently in
        float distanceCovered = 0f;
        Vector3 newPos = paths[0].position;

        for (int i = 0; i < paths.Length - 1; i++)
        {
            float segmentLength = Vector3.Distance(paths[i].position, paths[i + 1].position);

            if (traveledDistance <= distanceCovered + segmentLength)
            {
                float t = (traveledDistance - distanceCovered) / segmentLength;
                newPos = Vector3.Lerp(paths[i].position, paths[i + 1].position, t);
                break;
            }

            distanceCovered += segmentLength;
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

        if (isEndGame)
        {
            if(enemyData.catnipPrefabs[0] != null) Instantiate(enemyData.catnipPrefabs[0], transform.position, Quaternion.identity);
            Debug.Log("Dropped end game catnip");
        }
        else if (isMidGame)
        {
            if(enemyData.catnipPrefabs[1] != null) Instantiate(enemyData.catnipPrefabs[1], transform.position, Quaternion.identity);
            Debug.Log("Dropped mid game catnip");
        }
        else
        {
            if(enemyData.catnipPrefabs[2] != null) Instantiate(enemyData.catnipPrefabs[2], transform.position, Quaternion.identity);
            Debug.Log("Dropped early game catnip");
        }

        Destroy(gameObject);
    }

    private void OnReachEnd()
    {
        Debug.Log($"{name} reached the end!");
        PlayerHealth.Instance.DamagePlayer(enemyData.damageToPlayer);
        Destroy(gameObject);
    }
}
