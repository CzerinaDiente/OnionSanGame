using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
[System.Serializable]

public class Wave{
    public string waveName;
    public int noOfEnemies;
    public GameObject[] typeOfEnemies;
    public float spawnInterval;
}

public class WaveSpawner : MonoBehaviour
{
    [Header("Current Wave")]
    public Wave currentWave;
    private int currentWaveNumber = 0;
    private float nextSpawnTime;

    [Space(10)]
    [Header("Player in scene")]

    public GameObject Player;

    //commented out since i randomize the property of the current wave and increment it by 1 each cleared wave
    //public Wave[] waves;

    [Space(10)]
    [Header("Spawn Points")]
    public Transform[] spawnPoints;
    public Animator animator;
    public TextMeshProUGUI waveName; 



    private bool canSpawn = true;
    private bool canAnimate = false;

    //Added
    bool isWaveComplete = false;
    bool isCalled = false;

    public PlayerMovement playerMovement;

    private void Start()
    {
        //increment by 1 at the start of wave
        currentWaveNumber++;
        ChangeWavesProperty.RandomizeProperty(currentWave, currentWaveNumber);
    }
    private void Update()
    {
        //currentWave = waves[currentWaveNumber];
        SpawnWave();
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Only check wave completion AFTER spawning finished //and if player is still alive
        if (!canSpawn && totalEnemies.Length == 0 && Player != null){
            Debug.Log("canSpawn is " + canSpawn);
            //Value returns true from "SpawnWave()"
            if (isWaveComplete)
            {
                StartCoroutine(ReloadWave());
            }
        else { playerMovement.GameOverMenu(); } //Gameover
        }
    }

    void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time)
        {
            StartCoroutine(Randomize_CurrentWaveProperty(currentWave));
            currentWave.noOfEnemies--;

            nextSpawnTime = Time.time + currentWave.spawnInterval;

            if (currentWave.noOfEnemies == 0)
            {
                canSpawn = false;
                //Trigger wave cleared
                isWaveComplete = true;
                canAnimate = true;
            }
        }
    }

    //Originally from the SpawnWave, separated to call it by intervals based on spawnInterval value.
    IEnumerator Randomize_CurrentWaveProperty(Wave _currentWave)
    {
        if (!isCalled)
        {
            GameObject randomEnemy;
            //First 2 waves spawns slow enemies
            if(currentWaveNumber <= 2)
                randomEnemy = currentWave.typeOfEnemies[Random.Range(0, currentWave.typeOfEnemies.Length -1)];
            else
                randomEnemy = currentWave.typeOfEnemies[Random.Range(0, currentWave.typeOfEnemies.Length)];

            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);
            isCalled = true;
            Debug.Log("Spawned!");
        }
        nextSpawnTime = _currentWave.spawnInterval;
        yield return new WaitForSeconds(nextSpawnTime);
        isCalled = false;
    }

    IEnumerator ReloadWave()
    {
        //triggers animation when wave is done
        if (canAnimate)
        { 
            //Update wave count
            currentWaveNumber++;
            waveName.text = "Wave " + ChangeWavesProperty.NumToWords(currentWaveNumber);

            Debug.Log(waveName.text);
            animator.SetTrigger("WaveComplete");
            canAnimate = false;
        }
        //I manually set the values for timing, if you want more precise, use animation event function in animation tab (Add Event button).
        yield return new WaitForSeconds(6.5f);
        Invoke("SpawnNextWave", 0f);
    }

    void SpawnNextWave()
    {
        if (!canSpawn)
        {
            canSpawn = true;
            ChangeWavesProperty.RandomizeProperty(currentWave, currentWaveNumber);
            isWaveComplete = false;
            return;
        }
    }
}