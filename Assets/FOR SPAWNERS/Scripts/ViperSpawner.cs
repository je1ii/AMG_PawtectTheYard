using UnityEngine;
using System.Collections;

public class ViperSpawner : MonoBehaviour
{
    [Header("Viper Data")]
    public EnemyData data;

    // signature: (total, batchSize, moveSpeed, delay, manager, spawnPosition)
    public IEnumerator SpawnViperBatch(int total, int batchSize, float delay, WaveManager manager, Vector3 spawnPosition)
    {
        if (data.prefab == null)
        {
            Debug.LogWarning("ViperSpawner: viperPrefab not assigned");
            yield break;
        }

        int spawned = 0;
        while (spawned < total)
        {
            for (int i = 0; i < batchSize && spawned < total; i++)
            {
                GameObject viper = Instantiate(data.prefab, spawnPosition, Quaternion.identity);
                viper.GetComponentInChildren<EnemyHealthBar>().SetMaxHealth(data.startHealth);
                viper.GetComponent<CatPrey>().SetData(data);
                if (manager != null) manager.SetupEnemyPath(viper, data.startSpeed);
                spawned++;
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
