using System;
using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private VoidEvent onWaveStart;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private Timer timer;

    public int[,] waveArray = new int[3, 11];

    private int numberOfEnemiesToSpawn;
    private int monsterSpawnTime;

    private float[] probabilities = { 0.15f, 0.15f, 0.35f, 0.35f };

    private void Start()
    {
        // Enemy spawn intervals for each wave
        waveArray[1, 1] = 60;
        waveArray[1, 2] = 30;
        waveArray[1, 3] = 30;
        waveArray[1, 4] = 25;
        waveArray[1, 5] = 25;
        waveArray[1, 6] = 20;
        waveArray[1, 7] = 20;
        waveArray[1, 8] = 15;
        waveArray[1, 9] = 15;
        waveArray[1, 10] = 10;

        // Number of enemies spawned at each interval
        waveArray[2, 1] = 1;
        waveArray[2, 2] = 2;
        waveArray[2, 3] = 3;
        waveArray[2, 4] = 3;
        waveArray[2, 5] = 4;
        waveArray[2, 6] = 4;
        waveArray[2, 7] = 5;
        waveArray[2, 8] = 5;
        waveArray[2, 9] = 6;
        waveArray[2, 10] = 6;
    }

    private void OnEnable()
    {
        onWaveStart.AddListerner(MonsterSpawn);
    }

    private void OnDisable()
    {
        onWaveStart.RemoveListerner(MonsterSpawn);
    }

    private void MonsterSpawn()
    {
        StartCoroutine(MonsterSpawnTime());
    }

    private void InstantiateEnemy()
    {
        float randomValue = UnityEngine.Random.value;
        float cumulativeProbability = 0f;

        for (int i = 0; i < probabilities.Length; i++)
        {
            cumulativeProbability += probabilities[i];

            if (randomValue < cumulativeProbability)
            {
                Instantiate(enemies[i], transform.position, Quaternion.identity);
                break;
            }
        }
    }

    private IEnumerator MonsterSpawnTime()
    {
        monsterSpawnTime = waveArray[1, timer.turSayisi];
        numberOfEnemiesToSpawn = waveArray[2, timer.turSayisi];

        int numberOfSpawnIntervals = Convert.ToInt32(timer.remainingTime) / monsterSpawnTime;

        for (int i = 0; i < numberOfSpawnIntervals; i++)
        {
            for (int x = 0; x < numberOfEnemiesToSpawn; x++)
            {
                InstantiateEnemy();
            }

            yield return new WaitForSeconds(monsterSpawnTime);
        }
    }
}