using UnityEngine;

using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    float startTime = 0;
    float spawnInterval = 10;
    int maxEnemies = 3;

    GameObject[] cows;
    GameObject enemy;
    (Vector3, int) posAndIndex;

    [SerializeField] Camera camera2;
    // public Text camera2Text;
    
    void Update()
    {
        if (EnemiesCount() > 0)
            return;

        startTime += Time.deltaTime;
        if (startTime > spawnInterval)
        {
            SpawnEnemy();
            startTime = 0;
        }
    }

    void SpawnEnemy()
    {
        posAndIndex = GetSpawnPosition();
        Vector3 spawnPos = posAndIndex.Item1;
        enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        LookTo(cows[posAndIndex.Item2].transform.position);

        camera2.gameObject.SetActive(true);
        // camera2Text.text = "FOX ATTACK!";
    }

    (Vector3, int) GetSpawnPosition()
    {
        cows = GameObject.FindGameObjectsWithTag("WhiteCow");

        int randomIdx = Random.Range(0, cows.Length);
        Vector3 spawnPos = cows[randomIdx].transform.position;
        Vector2 spawnPosRaw = Random.insideUnitCircle * 3;
        spawnPos.x += spawnPosRaw.x;
        spawnPos.z += spawnPosRaw.y;

        return (spawnPos, randomIdx);
    }

    int EnemiesCount()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.Length;
    }

    void LookTo(Vector3 targetPosition)
    {
        Vector3 directionToPosition = Vector3.Normalize(targetPosition - enemy.transform.position);
        directionToPosition.y = 0;
        enemy.transform.forward = directionToPosition;
    }
}
