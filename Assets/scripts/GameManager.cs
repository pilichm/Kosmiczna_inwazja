using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject[] laserPrefabs;
    public GameObject player;
    public GameObject enemyPrefab;
    public GameObject health;
    public GameObject healthBonusPrefab;
    public GameObject barrierPrefab;
    public GameObject mainMenuScreen;

    public int playerHealth;

    private int leftLaserIndex = 0;
    private int rightLaserIndex = 1;
    private int healthFullIndex = 0;
    private int healthMediumIndex = 1;
    private int healthLowIndex = 2;
    private int maxIntervalTime = 15;
    private int currentScore;
    private int valueToAddAfterOneHit = 1;
    private int maxBarrierPowerUpSpawnDelay = 20;
    private int numberOfRows = 4;
    private int numberOfCols = 15;
    private int enemyCount;
    private int difficulty = 3;

    private float laserOffsetForward = 3f;
    private float laserOffsetHorizontal = 0.5f;
    private float gameOverScreenYWhenVisible = 185.0f;

    private Vector3 defLaserPosition = new Vector3(2.25f, 0.014f, 2.795f);
    
    public Material[] healthColors;
    
    public bool healthBonusSpawned;
    public bool enemyDestroyed;
    public bool moreThanHalfEnemiesAreDestroyed;
    public bool isPaused;
    public bool gameStartedFromButton = false;

    public AudioSource gameAudio;

    public AudioClip laserSound;
    public AudioClip explosionSound;

    public Text scoreCountText;
    public Text lifeCount;
    public TextMeshProUGUI pause;

    public RawImage gameOverScreen;
    public RawImage gameLogo;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Make enemies move downwards toward player ships when more than half enemies are destroyed.
    void CheckMoreThanHalfEnemiesAreDestroyed()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        int maxNumberOfEnemies = numberOfCols * numberOfRows;

        if (enemyCount < (maxNumberOfEnemies / 2))
        {
            moreThanHalfEnemiesAreDestroyed = false;
        }
    }

    // Function changing health indicator color based on amount of lives.
    public void UpdateHealthColor()
    {
        if (playerHealth > 3)
        {
            health.GetComponent<Renderer>().material = healthColors[healthFullIndex];
        } else if (playerHealth < 3 && playerHealth >= 1)
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
        if (gameStartedFromButton)
        {
            // Create player left and right laser.
            if (Input.GetKeyDown("space") && !isPaused)
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

            if (playerHealth <= 0 && !isPaused)
            {
                gameOverScreen.gameObject.SetActive(true);
                isPaused = true;
            }

            CheckMoreThanHalfEnemiesAreDestroyed();

            // Pause game on P key pressed. Resume on second click.
            if (Input.GetKeyDown(KeyCode.P))
            {
                PauseGame();
            }

            // Restart game when R is pressed.
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
        }
    }

    // Create and position enemies when game starts
    void CreateEnemies()
    {
        float xOffset = 50.0f;
        float yOffset = 0.0f;
        float zOffset = 10.0f;

        for (int i = 0; i < numberOfRows; i++)
        {
            xOffset = -xOffset;

            for (int j=0; j< numberOfCols; j++)
            {
                if (j%difficulty==0)
                {
                    float enemyPositionX = xOffset - 21 + j * 3;
                    float enemyPositionY = yOffset;
                    float enemyPositionZ = zOffset + i * 3;

                    Vector3 currentEnemyPosition = new Vector3(enemyPositionX, enemyPositionY, enemyPositionZ);
                    Instantiate(enemyPrefab, currentEnemyPosition, Quaternion.Euler(90, 0, 0));
                }
            }
        }
    }

    private Vector3 getRandomPowerUpPosition()
    {
        float xPosition = UnityEngine.Random.Range(-19, 19);
        float zPosition = UnityEngine.Random.Range(-1, 4);
        return new Vector3(xPosition, healthBonusPrefab.transform.position.y, zPosition);
    }

    // Creates health bonus if one isn't already created.
    void CreateHealthBonus()
    {
        if (!healthBonusSpawned && !isPaused)
        {
            Instantiate(healthBonusPrefab, getRandomPowerUpPosition(), Quaternion.Euler(90, 0, 0));
            healthBonusSpawned = true;
        }
    }

    private void CreateBarrierPowerUp()
    {
        if (!isPaused)
        {
            Instantiate(barrierPrefab, getRandomPowerUpPosition(), Quaternion.Euler(90, 0, 0));
        }   
    }

    public IEnumerator SpawnBarrierCountDownRoutine()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0, maxBarrierPowerUpSpawnDelay));
        CreateBarrierPowerUp();
    }

    // Adds 10 to users score after an enemy is hit.
    public void updateScore()
    {
        currentScore += valueToAddAfterOneHit;
        scoreCountText.text = "Score: " + currentScore + " of " + (numberOfCols * numberOfRows) + " destroyed!";
    }

    // Function updating life count text and color.
    public void UpdateLifeTextAndColor()
    {
        if (playerHealth == 1)
        {
            lifeCount.text = playerHealth + " life!";
            lifeCount.color = Color.red;
        } else if (playerHealth > 2)
        {
            lifeCount.text = playerHealth + " lives!";
            lifeCount.color = Color.green;
        } else if (playerHealth > 0)
        {
            lifeCount.text = playerHealth + " lives!";
            lifeCount.color = Color.yellow;
        } else if (playerHealth < 0)
        {
            lifeCount.text = "0 lives!";
        } else if (playerHealth == 0)
        {
            lifeCount.color = Color.black;
        }
    }

    // Stop game, it can be resumed.
    public void PauseGame()
    {
        isPaused = !isPaused;
        pause.gameObject.SetActive(isPaused);
    }

    // Restart game by reloading whole scene.
    public void RestartGame()
    {
        PauseGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /*
     * Method for ending the game.
     */
    public void EndGame()
    {
        Application.Quit();
    }

    /*
     * Method for setting difficulty, 1 is hardest, 3 is easiest.
     * Also increases time between respawning of power ups and decreases time barrier is active.
     */
    void SetDifficulty(int difficultyLevel)
    {
        difficulty = difficultyLevel;
        maxIntervalTime *= difficultyLevel;
    }

    /*
     * Method for starting the game.
     */
    public void StartGame(int difficultyLevel)
    {
        SetDifficulty(difficultyLevel);

        mainMenuScreen.SetActive(false);
        player.SetActive(true);
        scoreCountText.gameObject.SetActive(true);
        lifeCount.gameObject.SetActive(true);
        health.SetActive(true);
        gameLogo.gameObject.SetActive(false);

        playerHealth = 3;
        currentScore = 0;
        scoreCountText.text = "Score: " + currentScore;

        gameAudio = GetComponent<AudioSource>();

        CreateEnemies();
        UpdateHealthColor();
        CreateBarrierPowerUp();

        // Create health bonus at random time interval.
        healthBonusSpawned = false;
        enemyDestroyed = false;
        moreThanHalfEnemiesAreDestroyed = true;
        isPaused = false;
        gameStartedFromButton = true;

        float healthSpawnDelay = UnityEngine.Random.Range(0, maxIntervalTime);
        float healthSpawnInterval = UnityEngine.Random.Range(0, maxIntervalTime);
        InvokeRepeating("CreateHealthBonus", healthSpawnDelay, healthSpawnInterval);
    }
}