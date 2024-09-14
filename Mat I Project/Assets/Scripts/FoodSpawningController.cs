using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FoodSpawningController : MonoBehaviour
{
    [SerializeField] private GameObject massGainerPrefab;
    [SerializeField] private GameObject massBurnerPrefab;

    [SerializeField] private float spawnIntervalMinimum = 7f;
    [SerializeField] private float spawningIntervalMaximum = 11f;

    [SerializeField] private LayerMask snakeLayer;

    [SerializeField] private float respawnRadius = 0.5f;

    private GameObject currentFood;
    private Coroutine respawnCoroutine;


    public void SpawnRandomFood()
    {
        DestroyCurrentFood();

        GameObject food = Random.Range(0, 2) == 0 ? massGainerPrefab : massBurnerPrefab;

        Vector2 spawnPosition = FindValidSpawnPosition();
        if (spawnPosition == Vector2.zero) return;

        currentFood = Instantiate(food, spawnPosition, Quaternion.identity);

        StartSpawnTimer();
    }


    private Vector2 FindValidSpawnPosition()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector2 randomPosition = new Vector2(Random.Range(-18, 18), Random.Range(-18, 18));

            if (!Physics2D.OverlapCircle(randomPosition, respawnRadius, snakeLayer))
            {
                return randomPosition;
            }
        }

        return Vector2.zero;
    }


    private void StartSpawnTimer()
    {
        if (respawnCoroutine != null)
        {
            StopCoroutine(respawnCoroutine);
        }

        respawnCoroutine = StartCoroutine(RespawnFoodAfterTimerout());
    }


    public void StopSpawningFoods()
    {
        StopCoroutine(respawnCoroutine);
    }


    private IEnumerator RespawnFoodAfterTimerout()
    {
        int interval = (int)Random.Range(spawnIntervalMinimum, spawningIntervalMaximum);
        yield return new WaitForSeconds(interval);

        SpawnRandomFood();
    }


    public void OnFoodCollected()
    {
        SpawnRandomFood();
    }


    public void DestroyCurrentFood()
    {
        //This is already happening in spawnrandomfood() but a safety check
        if (currentFood != null)
        {
            Destroy(currentFood);
        }       
    }
}













