using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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

    [Header("Day Manager Reference")]
    public DayManager dayManager;

    [Header("Spawn Settings")]
    public float spawnDelay = 1.5f;
    public float batchDelay = 18f;

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

    IEnumerator HandleWaves(System.Func<IEnumerator> waveRoutine)
    {
        yield return StartCoroutine(waveRoutine());
        if (dayManager != null)
            dayManager.CompleteWave();
    }

    IEnumerator HandleWaves()
    {
        // --- Round 1 ---
        if (testRound1_Wave1) yield return StartCoroutine(HandleWaves(Round1_Wave1));
        if (testRound1_Wave2) yield return StartCoroutine(HandleWaves(Round1_Wave2));
        if (testRound1_Wave3) yield return StartCoroutine(HandleWaves(Round1_Wave3));
        if (testRound1_Wave4) yield return StartCoroutine(HandleWaves(Round1_Wave4));
        if (testRound1_Wave5) yield return StartCoroutine(HandleWaves(Round1_Wave5));

        // --- Round 2 ---
        if (testRound2_Wave1) yield return StartCoroutine(HandleWaves(Round2_Wave1));
        if (testRound2_Wave2) yield return StartCoroutine(HandleWaves(Round2_Wave2));
        if (testRound2_Wave3) yield return StartCoroutine(HandleWaves(Round2_Wave3));
        if (testRound2_Wave4) yield return StartCoroutine(HandleWaves(Round2_Wave4));
        if (testRound2_Wave5) yield return StartCoroutine(HandleWaves(Round2_Wave5));

        // Default play if no test flags are active (Round 1 W1-5 - Round 2 W1-5)
        if (!testRound1_Wave1 && !testRound1_Wave2 && !testRound1_Wave3 &&
            !testRound1_Wave4 && !testRound1_Wave5 &&
            !testRound2_Wave1 && !testRound2_Wave2 && !testRound2_Wave3 &&
            !testRound2_Wave4 && !testRound2_Wave5)
        {
            // Round 1
            yield return StartCoroutine(HandleWaves(Round1_Wave1)); yield return new WaitForSeconds(5f);
            yield return StartCoroutine(HandleWaves(Round1_Wave2)); yield return new WaitForSeconds(5f);
            yield return StartCoroutine(HandleWaves(Round1_Wave3)); yield return new WaitForSeconds(5f);
            yield return StartCoroutine(HandleWaves(Round1_Wave4)); yield return new WaitForSeconds(5f);
            yield return StartCoroutine(HandleWaves(Round1_Wave5)); yield return new WaitForSeconds(10f);

            // Round 2
            yield return StartCoroutine(HandleWaves(Round2_Wave1)); yield return new WaitForSeconds(5f);
            yield return StartCoroutine(HandleWaves(Round2_Wave2)); yield return new WaitForSeconds(5f);
            yield return StartCoroutine(HandleWaves(Round2_Wave3)); yield return new WaitForSeconds(5f);
            yield return StartCoroutine(HandleWaves(Round2_Wave4)); yield return new WaitForSeconds(5f);
            yield return StartCoroutine(HandleWaves(Round2_Wave5));
        }

        CheckForVictory();
    }

    // -------- ROUND 1 -------- //
    IEnumerator Round1_Wave1() { yield return SpawnBatchSequential(12, 4, 0, 0, false, 0, false); }
    IEnumerator Round1_Wave2() { yield return SpawnBatchSequential(12, 3, 4, 1, false, 0, false); }
    IEnumerator Round1_Wave3() { yield return SpawnBatchSequential(20, 4, 10, 2, false, 0, false); }
    IEnumerator Round1_Wave4() { yield return SpawnBatchSequential(8, 2, 16, 4, false, 0, false); }
    IEnumerator Round1_Wave5() { yield return SpawnBatchSequential(8, 2, 16, 4, true, 1, false); }

    // -------- ROUND 2 -------- // 
    IEnumerator Round2_Wave1() { yield return SpawnBatchSequential(10, 2, 20, 4, true, 2, true); }
    IEnumerator Round2_Wave2() { yield return SpawnBatchSequential(20, 4, 20, 4, true, 3, true); }
    IEnumerator Round2_Wave3() { yield return SpawnBatchSequential(24, 4, 24, 4, true, 6, true); }
    IEnumerator Round2_Wave4() { yield return SpawnBatchSequential(30, 5, 30, 5, true, 12, true); }
    IEnumerator Round2_Wave5() { yield return SpawnBatchSequential(36, 6, 36, 6, true, 18, true); }

    // -------- BATCH SPAWN -------- //
    IEnumerator SpawnBatchSequential(
        int totalRoach, int batchRoach,
        int totalGerry, int batchGerry,
        bool spawnViper, int totalVipers, bool isRound2)
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
                yield return StartCoroutine(roachSpawner.SpawnRoachBatch(roachThisBatch, roachThisBatch, spawnDelay, this, path1.position, isRound2));
            roachSpawned += roachThisBatch;

            // Spawn Gerrys
            if (gerryThisBatch > 0 && gerrySpawner != null)
                yield return StartCoroutine(gerrySpawner.SpawnGerryBatch(gerryThisBatch, gerryThisBatch, spawnDelay, this, path1.position, isRound2));
            gerrySpawned += gerryThisBatch;

            // Spawn Vipers
            if (viperThisBatch > 0 && viperSpawner != null)
                yield return StartCoroutine(viperSpawner.SpawnViperBatch(viperThisBatch, viperThisBatch, spawnDelay, this, path1.position, isRound2));
            viperSpawned += viperThisBatch;

           // Wait between batches
            float elapsed = 0f;

            // Keep checking both time and enemies
            while (elapsed < batchDelay)
            {
                // If all enemies are already gone, skip the rest of the delay
                if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
                    break;

                elapsed += Time.deltaTime;
                yield return null;
            }

            // After the delay (if enemies still exist), wait for them to be gone
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
        }
    }

    // -------- PATH SETUP -------- //
    public void SetupEnemyPath(GameObject enemy, float speed)
    {
        if (enemy == null) return;
        
        CatPrey p = enemy.GetComponent<CatPrey>();
        if (p != null)
        {
            p.path1 = path1;
            p.path2 = path2;
            p.path3 = path3;
            p.path4 = path4;
        }
    }

    void CheckForVictory()
    {
        StartCoroutine(WaitAndLoadVictoryScene());
    }

    IEnumerator WaitAndLoadVictoryScene()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);

        SceneManager.LoadScene("Victory");
    }
}