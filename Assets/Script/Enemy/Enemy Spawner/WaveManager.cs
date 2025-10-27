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

    [Header("Other Reference")]
    public DayManager dayManager;
    public GameObject loadingAnim;

    [Header("Spawn Settings")]
    public float spawnDelay = 1.5f;
    public float batchDelay = 18f;

    [Header("Spawn Sound Effects")]
    public AudioSource roachSpawnSound;
    public AudioSource gerrySpawnSound;
    public AudioSource viperSpawnSound;

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
        if (loadingAnim.activeInHierarchy == false)loadingAnim.SetActive(true);
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
        // loading anim
        yield return new WaitForSeconds(3f);
        
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
    IEnumerator Round1_Wave1() { yield return SpawnBatchSequential(12, 4, 0, 0, false, 0, false, false); }
    IEnumerator Round1_Wave2() { yield return SpawnBatchSequential(12, 3, 4, 1, false, 0, false, false); }
    IEnumerator Round1_Wave3() { yield return SpawnBatchSequential(20, 4, 10, 2, false, 0, false, false); }
    IEnumerator Round1_Wave4() { yield return SpawnBatchSequential(8, 2, 16, 4, false, 0, true, false); }
    IEnumerator Round1_Wave5() { yield return SpawnBatchSequential(8, 2, 16, 4, true, 1, true, false); }

    // -------- ROUND 2 -------- // 
    IEnumerator Round2_Wave1() { yield return SpawnBatchSequential(10, 2, 20, 4, true, 2, true, false); }
    IEnumerator Round2_Wave2() { yield return SpawnBatchSequential(20, 4, 20, 4, true, 3, true, false); }
    IEnumerator Round2_Wave3() { yield return SpawnBatchSequential(24, 4, 24, 4, true, 6, false, true); }
    IEnumerator Round2_Wave4() { yield return SpawnBatchSequential(30, 5, 30, 5, true, 12, false, true); }
    IEnumerator Round2_Wave5() { yield return SpawnBatchSequential(36, 6, 36, 6, true, 18, false, true); }

    // -------- BATCH SPAWN -------- //
    IEnumerator SpawnBatchSequential(
        int totalRoach, int batchRoach,
        int totalGerry, int batchGerry,
        bool spawnViper, int totalVipers, bool isMidGame,bool isEndGame)
    {
        int roachSpawned = 0;
        int gerrySpawned = 0;
        int viperSpawned = 0;

        // Repeat until all totals are met
        while (roachSpawned < totalRoach || gerrySpawned < totalGerry || (spawnViper && viperSpawned < totalVipers))
        {
            int roachThisBatch = Mathf.Min(batchRoach, totalRoach - roachSpawned);
            int gerryThisBatch = Mathf.Min(batchGerry, totalGerry - gerrySpawned);
            int viperThisBatch = spawnViper ? Mathf.Min(3, totalVipers - viperSpawned) : 0;

            // --- Spawn Roaches ---
            if (roachThisBatch > 0 && roachSpawner != null)
            {
                if (roachSpawnSound != null) roachSpawnSound.Play();
                yield return StartCoroutine(roachSpawner.SpawnRoachBatch(roachThisBatch, roachThisBatch, spawnDelay, this, path1.position, isMidGame, isEndGame));
            }
            roachSpawned += roachThisBatch;

            // --- Spawn Gerrys ---
            if (gerryThisBatch > 0 && gerrySpawner != null)
            {
                if (gerrySpawnSound != null) gerrySpawnSound.Play();
                yield return StartCoroutine(gerrySpawner.SpawnGerryBatch(gerryThisBatch, gerryThisBatch, spawnDelay, this, path1.position, isMidGame, isEndGame));
            }
            gerrySpawned += gerryThisBatch;

            // --- Spawn Vipers ---
            if (viperThisBatch > 0 && viperSpawner != null)
            {
                if (viperSpawnSound != null) viperSpawnSound.Play();
                yield return StartCoroutine(viperSpawner.SpawnViperBatch(viperThisBatch, viperThisBatch, spawnDelay, this, path1.position, isMidGame, isEndGame));
            }
            viperSpawned += viperThisBatch;

            // Wait between batches
            float elapsed = 0f;

            while (elapsed < batchDelay)
            {
                if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
                    break;

                elapsed += Time.deltaTime;
                yield return null;
            }

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
        
        if(loadingAnim!=null) 
            loadingAnim.GetComponent<Animator>().SetTrigger("LoadingOut");
        yield return new WaitForSeconds(1f);
        
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("Victory");
    }
}
