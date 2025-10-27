using UnityEngine;
using System.Collections;

public class ViperSpawner : MonoBehaviour
{
    [Header("Viper Data")]
    public EnemyData data;

    // signature: (total, batchSize, moveSpeed, delay, manager, spawnPosition)
    public IEnumerator SpawnViperBatch(int total, int batchSize, float delay, WaveManager manager, Vector3 spawnPosition, bool isMidGame, bool isEndGame)
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
                GameObject viper = Instantiate(data.prefab, spawnPosition, transform.localRotation);
                viper.GetComponent<CatPrey>().SetData(data);
                viper.GetComponent<CatPrey>().SetGameState(isMidGame, isEndGame);

                if (isEndGame)
                {
                    viper.GetComponentInChildren<EnemyHealthBar>().SetMaxHealth(data.endHealth);
                }
                else if(isMidGame)
                {
                    // viper starts to spawn at mid game
                    viper.GetComponentInChildren<EnemyHealthBar>().SetMaxHealth(data.startHealth);
                }
                else
                {
                    viper.GetComponentInChildren<EnemyHealthBar>().SetMaxHealth(data.startHealth);
                }

                if (manager != null)
                {
                    if (isEndGame)
                    {
                        manager.SetupEnemyPath(viper, data.endSpeed);
                    }
                    else if(isMidGame)
                    {
                        manager.SetupEnemyPath(viper, data.startSpeed);
                    }
                    else
                    {
                        manager.SetupEnemyPath(viper, data.startSpeed);
                    }
                }
                spawned++;
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
