using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CameraMovementController : MonoBehaviour
{
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private float moveAmount = 100f;
    [SerializeField] private float edgeSize = 30f;
    
    [SerializeField] private List<Transform> enemiesInTheScene = new List<Transform>();
    [SerializeField] private Transform endPathPoint;
    [SerializeField] private Transform enemyClosestToEnd;

    private Vector3 cameraPosition;
    [SerializeField] private bool lockedOnEnemy;
    
    void Start()
    {
        cameraPosition = transform.position;
        cameraMovement.SetUp(() => cameraPosition);
    }
    
    void Update()
    {
        UpdateEnemiesInScene();
        UpdateClosestEnemy();
        
        if (Input.GetKey(KeyCode.D))
        {
            if(cameraPosition.x < 10f)
                cameraPosition.x += moveAmount * Time.unscaledDeltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            if(cameraPosition.x > -10f)
                cameraPosition.x -= moveAmount * Time.unscaledDeltaTime;
        }
        if (Input.mousePosition.x > Screen.width - edgeSize)
        {
            if(cameraPosition.x < 10f)
                cameraPosition.x += moveAmount * Time.unscaledDeltaTime;
        }
        if (Input.mousePosition.x < edgeSize)
        {
            if(cameraPosition.x > -10f)
                cameraPosition.x -= moveAmount * Time.unscaledDeltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (lockedOnEnemy)
            {
                cameraPosition = cameraMovement.transform.position;
                cameraMovement.GetCameraPositionFunc(() => cameraPosition);
                lockedOnEnemy = false;
            }
            else
            {
                // make a condition that checks if the enemy is equals to null and is locked on
                // switch to free camera if is equal to null temporarily if still locked on
                cameraMovement.GetCameraPositionFunc(() => enemyClosestToEnd.position);
                lockedOnEnemy = true;
            }   
        }
    }
    
    private void UpdateEnemiesInScene()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Clean existing list
        enemiesInTheScene.RemoveAll(e => e == null);

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            var distancetoEndPoint = FindDistanceToAPoint(enemy.transform, endPathPoint);
            if (distancetoEndPoint > 0 && !enemiesInTheScene.Contains(enemy.transform))
            {
                enemiesInTheScene.Add(enemy.transform);
            }
        }
    }
    
    private void UpdateClosestEnemy()
    {
        if (enemyClosestToEnd != null && FindDistanceToAPoint(enemyClosestToEnd, endPathPoint) > 0)
            return;

        Transform closestMelee = enemiesInTheScene
            .Where(e => e != null && FindDistanceToAPoint(e, endPathPoint) > 0)
            .OrderBy(e => FindDistanceToAPoint(e, endPathPoint))
            .FirstOrDefault();

        if (closestMelee != null)
        {
            enemyClosestToEnd = closestMelee;
        }
    }
    
    private float FindDistanceToAPoint(Transform target, Transform point)
    {
        var distance = Mathf.Sqrt(Mathf.Pow(target.position.x - point.position.x, 2) + Mathf.Pow(target.position.y - point.position.y, 2));
        return distance;
    }
}
