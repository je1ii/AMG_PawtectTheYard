using UnityEngine;
using System.Collections;

public class RoachSpawner : MonoBehaviour
{
    [Header("Roach Data")]
    public EnemyData data;

    // signature: (total, batchSize, moveSpeed, delay, manager, spawnPosition)
    public IEnumerator SpawnRoachBatch(int total, int batchSize, float delay, WaveManager manager, Vector3 spawnPosition, bool isMidGame, bool isEndGame)
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
                GameObject roach = Instantiate(data.prefab, spawnPosition, transform.localRotation);
                roach.GetComponent<CatPrey>().SetData(data);
                roach.GetComponent<CatPrey>().SetGameState(isMidGame, isEndGame);

                if (isEndGame)
                {
                    roach.GetComponentInChildren<EnemyHealthBar>().SetMaxHealth(data.endHealth);
                }
                else if(isMidGame)
                {
                    roach.GetComponentInChildren<EnemyHealthBar>().SetMaxHealth(data.midHealth);
                }
                else
                {
                    roach.GetComponentInChildren<EnemyHealthBar>().SetMaxHealth(data.startHealth);
                }
                
                if (manager != null)
                {
                    if (isEndGame)
                    {
                        manager.SetupEnemyPath(roach, data.endSpeed);
                    }
                    else if(isMidGame)
                    {
                        manager.SetupEnemyPath(roach, data.midSpeed);     
                    }
                    else
                    {
                        manager.SetupEnemyPath(roach, data.startSpeed);     
                    }
                }
                
                spawned++;
                yield return new WaitForSeconds(delay);
            }

        }
    }
}
