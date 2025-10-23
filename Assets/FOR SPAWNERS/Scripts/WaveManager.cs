using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject roachPrefab;
    public GameObject gerryPrefab;
    public GameObject viperPrefab;

    [Header("Path References")]
    public Transform path1;
    public Transform path2;
    public Transform path3;
    public Transform path4;

    [Header("Spawn Settings")]
    public float spawnDelay = 1.5f;

    [Header("Wave Testing (Check the wave(s) you want to run)")]
    public bool testRound1_Wave1;
    public bool testRound1_Wave2;
    public bool testRound1_Wave3;
    public bool testRound1_Wave4;
    public bool testRound1_Wave5;

    public bool testRound2_Wave1;
    public bool testRound2_Wave2;
    public bool testRound2_Wave3;
    public bool testRound2_Wave4;
    public bool testRound2_Wave5;

    void Start()
    {
        StartCoroutine(HandleWaves());
    }

    IEnumerator HandleWaves()
    {
        // Round 1
        if (testRound1_Wave1) yield return StartCoroutine(Wave1_1());
        else if (testRound1_Wave2) yield return StartCoroutine(Wave1_2());
        else if (testRound1_Wave3) yield return StartCoroutine(Wave1_3());
        else if (testRound1_Wave4) yield return StartCoroutine(Wave1_4());
        else if (testRound1_Wave5) yield return StartCoroutine(Wave1_5());
        // Round 2
        else if (testRound2_Wave1) yield return StartCoroutine(Wave2_1());
        else if (testRound2_Wave2) yield return StartCoroutine(Wave2_2());
        else if (testRound2_Wave3) yield return StartCoroutine(Wave2_3());
        else if (testRound2_Wave4) yield return StartCoroutine(Wave2_4());
        else if (testRound2_Wave5) yield return StartCoroutine(Wave2_5());
        else
        {
            // Default: run everything
            yield return StartCoroutine(Wave1_1());
            yield return new WaitForSeconds(5f);
            yield return StartCoroutine(Wave1_2());
            yield return new WaitForSeconds(5f);
            yield return StartCoroutine(Wave1_3());
            yield return new WaitForSeconds(5f);
            yield return StartCoroutine(Wave1_4());
            yield return new WaitForSeconds(5f);
            yield return StartCoroutine(Wave1_5());
            yield return new WaitForSeconds(10f);
            yield return StartCoroutine(Wave2_1());
            yield return new WaitForSeconds(5f);
            yield return StartCoroutine(Wave2_2());
            yield return new WaitForSeconds(5f);
            yield return StartCoroutine(Wave2_3());
            yield return new WaitForSeconds(5f);
            yield return StartCoroutine(Wave2_4());
            yield return new WaitForSeconds(5f);
            yield return StartCoroutine(Wave2_5());
        }
    }

    // -------- ROUND 1 -------- //

    IEnumerator Wave1_1()
    {
        Debug.Log("Round 1 - Wave 1");
        yield return SpawnBatch(roachPrefab, 12, 4, 0, 0, 2f, 0f, 0f);
    }

    IEnumerator Wave1_2()
    {
        Debug.Log("Round 1 - Wave 2");
        yield return SpawnBatch(roachPrefab, 12, 3, 4, 1, 2f, 3.5f, 0f);
    }

    IEnumerator Wave1_3()
    {
        Debug.Log("Round 1 - Wave 3");
        yield return SpawnBatch(roachPrefab, 20, 4, 10, 2, 2.2f, 3.5f, 0f);
    }

    IEnumerator Wave1_4()
    {
        Debug.Log("Round 1 - Wave 4");
        yield return SpawnBatch(roachPrefab, 8, 2, 16, 4, 2.3f, 3.8f, 0f);
    }

    IEnumerator Wave1_5()
    {
        Debug.Log("Round 1 - Wave 5");
        yield return SpawnBatch(roachPrefab, 8, 2, 16, 4, 2.5f, 4f, 5f, true);
    }

    // -------- ROUND 2 -------- //

    IEnumerator Wave2_1()
    {
        Debug.Log("Round 2 - Wave 1");
        yield return SpawnBatch(roachPrefab, 10, 2, 20, 4, 2.5f, 4f, 5f, true, 2);
    }

    IEnumerator Wave2_2()
    {
        Debug.Log("Round 2 - Wave 2");
        yield return SpawnBatch(roachPrefab, 20, 4, 20, 4, 2.5f, 4f, 5f, true, 3);
    }

    IEnumerator Wave2_3()
    {
        Debug.Log("Round 2 - Wave 3");
        yield return SpawnBatch(roachPrefab, 24, 4, 24, 4, 2.5f, 4f, 5f, true, 6);
    }

    IEnumerator Wave2_4()
    {
        Debug.Log("Round 2 - Wave 4");
        yield return SpawnBatch(roachPrefab, 30, 5, 30, 5, 2.5f, 4f, 5f, true, 12);
    }

    IEnumerator Wave2_5()
    {
        Debug.Log("Round 2 - Wave 5");
        yield return SpawnBatch(roachPrefab, 36, 6, 36, 6, 2.5f, 4f, 5f, true, 18);
    }

    // -------- Batch Spawn -------- //

    IEnumerator SpawnBatch(GameObject roach, int totalRoach, int batchRoach,
                           int totalGerry, int batchGerry,
                           float roachSpeed, float gerrySpeed, float viperSpeed,
                           bool spawnViper = false, int totalVipers = 1)
    {
        int spawnedRoach = 0;
        int spawnedGerry = 0;
        int spawnedViper = 0;

        while (spawnedRoach < totalRoach || spawnedGerry < totalGerry || (spawnViper && spawnedViper < totalVipers))
        {
            // Roaches
            for (int i = 0; i < batchRoach && spawnedRoach < totalRoach; i++)
            {
                SpawnEnemy(roachPrefab, roachSpeed);
                spawnedRoach++;
                yield return new WaitForSeconds(spawnDelay);
            }

            // Gerrys
            for (int j = 0; j < batchGerry && spawnedGerry < totalGerry; j++)
            {
                SpawnEnemy(gerryPrefab, gerrySpeed);
                spawnedGerry++;
                yield return new WaitForSeconds(spawnDelay);
            }

            // Vipers
            if (spawnViper && spawnedViper < totalVipers)
            {
                int vipersThisBatch = Mathf.Min(3, totalVipers - spawnedViper);
                for (int k = 0; k < vipersThisBatch; k++)
                {
                    SpawnEnemy(viperPrefab, viperSpeed);
                    spawnedViper++;
                    yield return new WaitForSeconds(3f);
                }
            }

            yield return new WaitForSeconds(12f);
        }
    }

    void SpawnEnemy(GameObject prefab, float speed)
    {
        if (prefab == null || path1 == null) return;

        GameObject enemy = Instantiate(prefab, path1.position, Quaternion.identity);
        EnemyPathMovement path = enemy.GetComponent<EnemyPathMovement>();

        if (path != null)
        {
            path.path1 = path1;
            path.path2 = path2;
            path.path3 = path3;
            path.path4 = path4;
            path.SetSpeed(speed);
        }
    }
}
