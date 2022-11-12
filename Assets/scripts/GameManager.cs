using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] laserPrefabs;
    public GameObject player;
    public GameObject enemyPrefab;
    private Vector3 defLaserPosition = new Vector3(2.25f, 0.014f, 2.795f);
    private int leftLaserIndex = 0;
    private int rightLaserIndex = 1;
    private float laserOffsetForward = 3.0f;
    private float laserOffsetHorizontal = 2.28f;
    public Material[] healthColors;

    public int playerHealth = 3;
    private int healthFullIndex = 0;
    private int healthMediumIndex = 1;
    private int healthLowIndex = 2;
    public GameObject health;
    public bool healthBonusSpawned;
    private int maxIntervalTime = 15;
    public GameObject healthBonusPrefab;

    public AudioSource gameAudio;
    public AudioClip laserSound;
    public AudioClip explosionSound;
    public bool enemyDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        gameAudio = GetComponent<AudioSource>();

        CreateEnemies();
        UpdateHealthColor();

        // Create health bonus at random time interval.
        healthBonusSpawned = false;
        enemyDestroyed = false;
        float healthSpawnDelay = UnityEngine.Random.Range(0, maxIntervalTime);
        float healthSpawnInterval = UnityEngine.Random.Range(0, maxIntervalTime);
        InvokeRepeating("CreateHealthBonus", healthSpawnDelay, healthSpawnInterval);
    }

    // Function changing health indicator color based on amount of lives.
    public void UpdateHealthColor()
    {
        if (playerHealth >= 3)
        {
            health.GetComponent<Renderer>().material = healthColors[healthFullIndex];
        } else if (playerHealth == 2)
        {
            health.GetComponent<Renderer>().material = healthColors[healthMediumIndex];
        } else
        {
            health.GetComponent<Renderer>().material = healthColors[healthLowIndex];
        }

        if (enemyDestroyed)
        {
            if (gameAudio.isPlaying)
            {
                gameAudio.Stop();
            }
             
            gameAudio.PlayOneShot(explosionSound, 3.0f);
            enemyDestroyed = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Create player left and right laser.
        if (Input.GetKeyDown("space"))
        {
            gameAudio.PlayOneShot(laserSound, 1.0f);

            Vector3 laserLeftPosition = new Vector3(
                player.transform.position.x + laserOffsetHorizontal, 
                defLaserPosition.y, 
                player.transform.position.z + laserOffsetForward);

            Vector3 laserRightPosition = new Vector3(
                player.transform.position.x - laserOffsetHorizontal, 
                defLaserPosition.y, 
                player.transform.position.z + laserOffsetForward);

            Instantiate(laserPrefabs[leftLaserIndex], laserLeftPosition, Quaternion.Euler(90, 0, 0));
            Instantiate(laserPrefabs[rightLaserIndex], laserRightPosition, Quaternion.Euler(90, 0, 0));
        }
    }

    // Create and position enemies when game starts
    void CreateEnemies()
    {
        float xOffset = 50.0f;
        float yOffset = 0.0f;
        float zOffset = 10.0f;
        int numberOfRows = 4;
        int numberOfCols = 15;

        for (int i = 0; i < numberOfRows; i++)
        {
            xOffset = -xOffset;

            for (int j=0; j< numberOfCols; j++)
            {
                float enemyPositionX = xOffset - 21 + j * 3;
                float enemyPositionY = yOffset;
                float enemyPositionZ = zOffset + i * 3;

                Vector3 currentEnemyPosition = new Vector3(enemyPositionX, enemyPositionY, enemyPositionZ);
                Instantiate(enemyPrefab, currentEnemyPosition, Quaternion.Euler(90, 0, 0));
            }
        }
    }

    // Creates health bonus if one isn't already created.
    void CreateHealthBonus()
    {
        if (!healthBonusSpawned)
        {
            float xPosition = UnityEngine.Random.Range(-19, 19);
            float zPosition = UnityEngine.Random.Range(-1, 4);
            Vector3 healthBonusPosition = new Vector3(xPosition, healthBonusPrefab.transform.position.y, zPosition);

            Instantiate(healthBonusPrefab, healthBonusPosition, Quaternion.Euler(90, 0, 0));
            healthBonusSpawned = true;
        }
    }
}
