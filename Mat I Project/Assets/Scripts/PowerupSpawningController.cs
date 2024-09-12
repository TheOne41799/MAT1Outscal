using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawningController : MonoBehaviour
{
    [SerializeField] private GameObject shieldPowerupPrefab;
    [SerializeField] private GameObject scoreBoostPowerupPrefab;
    [SerializeField] private GameObject speedupPowerupPrefab;

    [SerializeField] private float minSpawnInterval = 5f;
    [SerializeField] private float maxSpawnInterval = 15f;
    [SerializeField] private float powerupLifeTime = 10f;

    [SerializeField] private LayerMask snakeLayer;
    [SerializeField] private LayerMask foodLayer;

    [SerializeField] private float respawnRadius = 0.5f;


    private void Start()
    {
        StartCoroutine(SpawnPowerups());
    }


    private IEnumerator SpawnPowerups()
    {
        while (true)
        {
            yield return new WaitForSeconds(minSpawnInterval);

            Vector2 spawnPosition = FindValidSpawnPosition();

            if (spawnPosition != Vector2.zero)
            {
                GameObject powerupPrefab = ChooseRandomPowerup();
                GameObject powerup = Instantiate(powerupPrefab, spawnPosition, Quaternion.identity);
                Destroy(powerup, powerupLifeTime);
            }

            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);
        }
    }


    private Vector2 FindValidSpawnPosition()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector2 randomPosition = new Vector2(Random.Range(-18, 18), Random.Range(-18, 18));

            if (!Physics2D.OverlapCircle(randomPosition, respawnRadius, snakeLayer) ||
                !Physics2D.OverlapCircle(randomPosition, respawnRadius, foodLayer))
            {
                return randomPosition;
            }
        }

        return Vector2.zero;
    }


    private GameObject ChooseRandomPowerup()
    {
        float rand = Random.value;

        if(rand < 0.33f)
        {
            return shieldPowerupPrefab;
        }
        else if(rand < 0.66f)
        {
            return scoreBoostPowerupPrefab;
        }
        else
        {
            return speedupPowerupPrefab;
        }
    }
}










