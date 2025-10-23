using UnityEngine;
using System.Collections;

public class GerrySpawner : MonoBehaviour
{
    [Header("Gerry Settings")]
    public GameObject gerryPrefab;
    public int maxHealth = 50;
    public float speed = 3.5f;
    public int catnipDrop = 10;

    // signature: (total, batchSize, moveSpeed, delay, manager, spawnPosition)
    public IEnumerator SpawnGerryBatch(int total, int batchSize, float moveSpeed, float delay, WaveManager manager, Vector3 spawnPosition)
    {
        if (gerryPrefab == null)
        {
            Debug.LogWarning("GerrySpawner: gerryPrefab not assigned");
            yield break;
        }

        int spawned = 0;
        while (spawned < total)
        {
            for (int i = 0; i < batchSize && spawned < total; i++)
            {
                GameObject gerry = Instantiate(gerryPrefab, spawnPosition, Quaternion.identity);
                if (manager != null) manager.SetupEnemyPath(gerry, moveSpeed);
                spawned++;
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
