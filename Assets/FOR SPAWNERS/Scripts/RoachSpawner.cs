using UnityEngine;
using System.Collections;

public class RoachSpawner : MonoBehaviour
{
    [Header("Roach Data")]
    public EnemyData data;

    // signature: (total, batchSize, moveSpeed, delay, manager, spawnPosition)
    public IEnumerator SpawnRoachBatch(int total, int batchSize, float delay, WaveManager manager, Vector3 spawnPosition)
    {
        if (data.prefab == null)
        {
            Debug.LogWarning("RoachSpawner: roachPrefab not assigned");
            yield break;
        }

        int spawned = 0;
        while (spawned < total)
        {
            for (int i = 0; i < batchSize && spawned < total; i++)
            {
                GameObject roach = Instantiate(data.prefab, spawnPosition, Quaternion.identity);
                roach.GetComponentInChildren<EnemyHealthBar>().SetMaxHealth(data.startHealth);
                roach.GetComponent<CatPrey>().SetData(data);
                if (manager != null) manager.SetupEnemyPath(roach, data.startSpeed);
                spawned++;
                yield return new WaitForSeconds(delay);
            }

        }
    }
}
