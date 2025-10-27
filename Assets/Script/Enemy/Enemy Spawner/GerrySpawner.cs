using UnityEngine;
using System.Collections;

public class GerrySpawner : MonoBehaviour
{
    [Header("Gerry Data")]
    public EnemyData data;

    // signature: (total, batchSize, moveSpeed, delay, manager, spawnPosition)
    public IEnumerator SpawnGerryBatch(int total, int batchSize, float delay, WaveManager manager, Vector3 spawnPosition, bool isMidGame, bool isEndGame)
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
                GameObject gerry = Instantiate(data.prefab, spawnPosition, transform.localRotation);
                gerry.GetComponent<CatPrey>().SetData(data);
                gerry.GetComponent<CatPrey>().SetGameState(isMidGame, isEndGame);
                
                if (isEndGame)
                {
                    gerry.GetComponentInChildren<EnemyHealthBar>().SetMaxHealth(data.endHealth);
                }
                else if (isMidGame)
                {
                    gerry.GetComponentInChildren<EnemyHealthBar>().SetMaxHealth(data.midHealth);
                }
                else
                {
                    gerry.GetComponentInChildren<EnemyHealthBar>().SetMaxHealth(data.startHealth);
                }

                if (manager != null)
                {
                    if (isEndGame)
                    {
                        manager.SetupEnemyPath(gerry, data.endSpeed);
                    }
                    else if (isMidGame)
                    {
                        manager.SetupEnemyPath(gerry, data.midSpeed);
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
