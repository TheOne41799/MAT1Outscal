using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawningController : MonoBehaviour
{
    [SerializeField] private GameObject massGainerPrefab;
    [SerializeField] private GameObject massBurnerPrefab;

    [SerializeField] private float spawnIntervalMinimum = 7f;
    [SerializeField] private float spawningIntervalMaximum = 11f;

    private GameObject currentFood;
    private Coroutine respawnCoroutine;


    private void Start()
    {
        SpawnRandomFood();
    }


    private void SpawnRandomFood()
    {
        if (currentFood != null)
        {
            Destroy(currentFood);
        }

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

            return randomPosition;
        }

        return Vector2.zero;
    }


    private void StartSpawnTimer()
    {
        if(respawnCoroutine != null)
        {
            StopCoroutine(respawnCoroutine);
        }

        respawnCoroutine = StartCoroutine(RespawnFoodAfterTimerout());
    }


    private IEnumerator RespawnFoodAfterTimerout()
    {
        int interval = (int) Random.Range(spawnIntervalMinimum, spawningIntervalMaximum);
        Debug.Log(interval);

        yield return new WaitForSeconds(interval);
        SpawnRandomFood();
    }
}
















