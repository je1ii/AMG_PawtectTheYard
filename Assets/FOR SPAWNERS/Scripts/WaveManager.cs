using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("Spawner References")]
    public RoachSpawner roachSpawner;
    public GerrySpawner gerrySpawner;
    public ViperSpawner viperSpawner;

    [Header("Path References")]
    public Transform path1;
    public Transform path2;
    public Transform path3;
    public Transform path4;

    [Header("Spawn Settings")]
    public float spawnDelay = 1.5f;
    public float batchDelay = 12f;

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
        if (roachSpawner == null) Debug.LogWarning("WaveManager: roachSpawner not assigned!");
        if (gerrySpawner == null) Debug.LogWarning("WaveManager: gerrySpawner not assigned!");
        if (viperSpawner == null) Debug.LogWarning("WaveManager: viperSpawner not assigned!");
        if (path1 == null) Debug.LogWarning("WaveManager: path1 not assigned!");

        StartCoroutine(HandleWaves());
    }

    IEnumerator HandleWaves()
    {
        // --- Round 1 ---
        if (testRound1_Wave1) yield return StartCoroutine(Round1_Wave1());
        if (testRound1_Wave2) yield return StartCoroutine(Round1_Wave2());
        if (testRound1_Wave3) yield return StartCoroutine(Round1_Wave3());
        if (testRound1_Wave4) yield return StartCoroutine(Round1_Wave4());
        if (testRound1_Wave5) yield return StartCoroutine(Round1_Wave5());

        // --- Round 2 ---
        if (testRound2_Wave1) yield return StartCoroutine(Round2_Wave1());
        if (testRound2_Wave2) yield return StartCoroutine(Round2_Wave2());
        if (testRound2_Wave3) yield return StartCoroutine(Round2_Wave3());
        if (testRound2_Wave4) yield return StartCoroutine(Round2_Wave4());
        if (testRound2_Wave5) yield return StartCoroutine(Round2_Wave5());

        // Default play if no test flags are active (Round 1 W1-5 - Round 2 W1-5)
        if (!testRound1_Wave1 && !testRound1_Wave2 && !testRound1_Wave3 &&
            !testRound1_Wave4 && !testRound1_Wave5 &&
            !testRound2_Wave1 && !testRound2_Wave2 && !testRound2_Wave3 &&
            !testRound2_Wave4 && !testRound2_Wave5)
        {
            // Round 1
            yield return StartCoroutine(Round1_Wave1()); yield return new WaitForSeconds(5f);
            yield return StartCoroutine(Round1_Wave2()); yield return new WaitForSeconds(5f);
            yield return StartCoroutine(Round1_Wave3()); yield return new WaitForSeconds(5f);
            yield return StartCoroutine(Round1_Wave4()); yield return new WaitForSeconds(5f);
            yield return StartCoroutine(Round1_Wave5()); yield return new WaitForSeconds(10f);

            // Round 2
            yield return StartCoroutine(Round2_Wave1()); yield return new WaitForSeconds(5f);
            yield return StartCoroutine(Round2_Wave2()); yield return new WaitForSeconds(5f);
            yield return StartCoroutine(Round2_Wave3()); yield return new WaitForSeconds(5f);
            yield return StartCoroutine(Round2_Wave4()); yield return new WaitForSeconds(5f);
            yield return StartCoroutine(Round2_Wave5());
        }
    }

    // -------- ROUND 1 -------- //
    IEnumerator Round1_Wave1() { yield return SpawnBatchSequential(12, 4, 0, 0, 2f, 0f, 0f); }
    IEnumerator Round1_Wave2() { yield return SpawnBatchSequential(12, 3, 4, 1, 2f, 3.5f, 0f); }
    IEnumerator Round1_Wave3() { yield return SpawnBatchSequential(20, 4, 10, 2, 2.2f, 3.5f, 0f); }
    IEnumerator Round1_Wave4() { yield return SpawnBatchSequential(8, 2, 16, 4, 2.3f, 3.8f, 0f); }
    IEnumerator Round1_Wave5() { yield return SpawnBatchSequential(8, 2, 16, 4, 2.5f, 4f, 5f, true); }

    // -------- ROUND 2 -------- //
    IEnumerator Round2_Wave1() { yield return SpawnBatchSequential(10, 2, 20, 4, 2.5f, 4f, 5f, true, 2); }
    IEnumerator Round2_Wave2() { yield return SpawnBatchSequential(20, 4, 20, 4, 2.5f, 4f, 5f, true, 3); }
    IEnumerator Round2_Wave3() { yield return SpawnBatchSequential(24, 4, 24, 4, 2.5f, 4f, 5f, true, 6); }
    IEnumerator Round2_Wave4() { yield return SpawnBatchSequential(30, 5, 30, 5, 2.5f, 4f, 5f, true, 12); }
    IEnumerator Round2_Wave5() { yield return SpawnBatchSequential(36, 6, 36, 6, 2.5f, 4f, 5f, true, 18); }

    // -------- BATCH SPAWN -------- //
    IEnumerator SpawnBatchSequential(
        int totalRoach, int batchRoach,
        int totalGerry, int batchGerry,
        float roachSpeed, float gerrySpeed, float viperSpeed,
        bool spawnViper = false, int totalVipers = 1)
    {
        int roachSpawned = 0;
        int gerrySpawned = 0;
        int viperSpawned = 0;

        // Repeat until all totals are met
        while (roachSpawned < totalRoach || gerrySpawned < totalGerry || (spawnViper && viperSpawned < totalVipers))
        {
            // Spawn batch sequentially: Roach, Gerry, Viper 
            int roachThisBatch = Mathf.Min(batchRoach, totalRoach - roachSpawned);
            int gerryThisBatch = Mathf.Min(batchGerry, totalGerry - gerrySpawned);
            int viperThisBatch = spawnViper ? Mathf.Min(3, totalVipers - viperSpawned) : 0;

            // Spawn Roaches
            if (roachThisBatch > 0 && roachSpawner != null)
                yield return StartCoroutine(roachSpawner.SpawnRoachBatch(roachThisBatch, roachThisBatch, roachSpeed, spawnDelay, this, path1.position));
            roachSpawned += roachThisBatch;

            // Spawn Gerrys
            if (gerryThisBatch > 0 && gerrySpawner != null)
                yield return StartCoroutine(gerrySpawner.SpawnGerryBatch(gerryThisBatch, gerryThisBatch, gerrySpeed, spawnDelay, this, path1.position));
            gerrySpawned += gerryThisBatch;

            // Spawn Vipers
            if (viperThisBatch > 0 && viperSpawner != null)
                yield return StartCoroutine(viperSpawner.SpawnViperBatch(viperThisBatch, viperThisBatch, viperSpeed, spawnDelay, this, path1.position));
            viperSpawned += viperThisBatch;

            // Wait between batches
            yield return new WaitForSeconds(batchDelay);
        }
    }

    // -------- PATH SETUP -------- //
    public void SetupEnemyPath(GameObject enemy, float speed)
    {
        if (enemy == null) return;

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
