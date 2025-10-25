using UnityEngine;
using System.Collections;

public class GerrySpawner : MonoBehaviour
{
    [Header("Gerry Data")]
    public EnemyData data;

    // signature: (total, batchSize, moveSpeed, delay, manager, spawnPosition)
    public IEnumerator SpawnGerryBatch(int total, int batchSize, float delay, WaveManager manager, Vector3 spawnPosition, bool isRound2)
    {
        if (data.prefab == null)
        {
            Debug.LogWarning("GerrySpawner: gerry data = prefab not assigned");
            yield break;
        }

        int spawned = 0; 
        while (spawned < total)
        {
            for (int i = 0; i < batchSize && spawned < total; i++)
            {
                GameObject gerry = Instantiate(data.prefab, spawnPosition, Quaternion.identity);
                
                if (isRound2)
                {
                    gerry.GetComponentInChildren<EnemyHealthBar>().SetMaxHealth(data.endHealth);
                }
                else
                {
                    gerry.GetComponentInChildren<EnemyHealthBar>().SetMaxHealth(data.startHealth);
                }
                
                gerry.GetComponent<CatPrey>().SetData(data);

                if (manager != null)
                {
                    if (isRound2)
                    {
                        manager.SetupEnemyPath(gerry, data.endSpeed);
                    }
                    else
                    {
                        manager.SetupEnemyPath(gerry, data.startSpeed);
                    }
                }
                
                spawned++;
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
