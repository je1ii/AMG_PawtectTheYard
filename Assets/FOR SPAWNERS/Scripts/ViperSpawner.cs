using UnityEngine;
using System.Collections;

public class ViperSpawner : MonoBehaviour
{
    [Header("Viper Settings")]
    public GameObject viperPrefab;
    public int maxHealth = 100;
    public float speed = 5f;
    public int catnipDrop = 20;

    // signature: (total, batchSize, moveSpeed, delay, manager, spawnPosition)
    public IEnumerator SpawnViperBatch(int total, int batchSize, float moveSpeed, float delay, WaveManager manager, Vector3 spawnPosition)
    {
        if (viperPrefab == null)
        {
            Debug.LogWarning("ViperSpawner: viperPrefab not assigned");
            yield break;
        }

        int spawned = 0;
        while (spawned < total)
        {
            for (int i = 0; i < batchSize && spawned < total; i++)
            {
                GameObject viper = Instantiate(viperPrefab, spawnPosition, Quaternion.identity);
                if (manager != null) manager.SetupEnemyPath(viper, moveSpeed);
                spawned++;
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
