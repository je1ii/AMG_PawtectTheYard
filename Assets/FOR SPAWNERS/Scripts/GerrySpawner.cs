using UnityEngine;

public class GerrySpawner : MonoBehaviour
{
    [Header("Gerry Settings")]
    public float speed = 8f;
    public GameObject gerryPrefab;

    [Header("Spawn Settings")]
    public float spawnSpacing = 1f;

    public GameObject SpawnGerry(Vector3 spawnPosition)
    {
        if (gerryPrefab == null) return null;

        Vector3 offset = new Vector3(
            Random.Range(-spawnSpacing, spawnSpacing),
            0f,
            Random.Range(-spawnSpacing, spawnSpacing)
        );

        return Instantiate(gerryPrefab, spawnPosition + offset, Quaternion.identity);
    }
}
