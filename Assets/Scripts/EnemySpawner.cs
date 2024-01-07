using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private float spawnRatePerMinute = 30;
    [SerializeField] private float spawnRateIncrement = 1;
    private float spawnNext = 0;

    void Update()
    {
        if (Time.time > spawnNext)
        {
            spawnNext = Time.time + 60 / spawnRatePerMinute;
            spawnRatePerMinute += spawnRateIncrement;
            Vector3 spawnPosition = new Vector3(Random.Range(-GetScreenWidthWorldUnits(), GetScreenWidthWorldUnits()), GetScreenTopWorldUnits(), -10);
            Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
        }
    }
    
    void OnDrawGizmos()
    {
        float width = GetScreenWidthWorldUnits();
        float height = GetScreenTopWorldUnits();
        Vector3 topLeft = new Vector3(-width, height, -10);
        Vector3 topRight = new Vector3(width, height, -10);
        Vector3 bottomLeft = new Vector3(-width, -height, -10);
        Vector3 bottomRight = new Vector3(width, -height, -10);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }


    private float GetScreenWidthWorldUnits()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            return mainCamera.orthographicSize * mainCamera.aspect;
        }
        return 0;
    }

    private float GetScreenTopWorldUnits()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            return mainCamera.orthographicSize;
        }
        return 0;
    }
}