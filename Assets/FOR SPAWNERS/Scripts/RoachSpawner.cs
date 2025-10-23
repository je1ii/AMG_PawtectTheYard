using UnityEngine;
using System.Collections;

public class RoachSpawner : MonoBehaviour
{
    [Header("Roach Settings")]
    public GameObject roachPrefab;
    public int maxHealth = 30;
    public float speed = 2f;
    public int catnipDrop = 5;

    // signature: (total, batchSize, moveSpeed, delay, manager, spawnPosition)
    public IEnumerator SpawnRoachBatch(int total, int batchSize, float moveSpeed, float delay, WaveManager manager, Vector3 spawnPosition)
    {
        if (roachPrefab == null)
        {
            Debug.LogWarning("RoachSpawner: roachPrefab not assigned");
            yield break;
        }

        int spawned = 0;
        while (spawned < total)
        {
            for (int i = 0; i < batchSize && spawned < total; i++)
            {
                GameObject roach = Instantiate(roachPrefab, spawnPosition, Quaternion.identity);
                if (manager != null) manager.SetupEnemyPath(roach, moveSpeed);
                spawned++;
                yield return new WaitForSeconds(delay);
            }

        }
    }
}
