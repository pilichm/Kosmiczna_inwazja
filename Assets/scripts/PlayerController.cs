using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 15.0f;
    public float horizontalInput;
    public float verticalInput;

    private float horizontalBoundary = 18.0f;
    private float verticalBoundaryTop = 6.0f;
    private float verticalBoundaryBottom = -1.0f;
    private float rotationSpeed = 50.0f;
    private float maxWingRotation = 0.1f;

    private GameManager gameManager;

    public GameObject playerWing;
    public GameObject playerBarrier;

    private const string TAG_ENEMY_LASER = "EnemyLaser";
    private const string TAG_HEALTH_BONUS = "HealthBonus";
    private const string TAG_BARRIER = "Barrier";

    public AudioClip bunusPickedSound;

    private AudioSource playerAudio;

    private List<int> enemyLasersAlreadyCollided;

    private int barrierActiveTime = 5;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerAudio = GetComponent<AudioSource>();
        enemyLasersAlreadyCollided = new List<int>();
        playerBarrier.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameManager.isPaused)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            if (transform.position.x < -horizontalBoundary)
            {
                transform.position = new Vector3(-horizontalBoundary, transform.position.y, transform.position.z);
            }

            if (transform.position.x > horizontalBoundary)
            {
                transform.position = new Vector3(horizontalBoundary, transform.position.y, transform.position.z);
            }

            if (transform.position.z < verticalBoundaryBottom)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, verticalBoundaryBottom);
            }

            if (transform.position.z > verticalBoundaryTop)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, verticalBoundaryTop);
            }

            transform.Translate(Vector3.right * horizontalInput * playerSpeed * Time.deltaTime);
            transform.Translate(Vector3.forward * verticalInput * playerSpeed * Time.deltaTime);

            // Rotate player ship wing based on horizontal input.
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                if (Math.Abs(horizontalInput) > 0 && Math.Abs(playerWing.transform.rotation.z) <= maxWingRotation)
                {
                    playerWing.transform.Rotate(new Vector3(0, 0, 10) * rotationSpeed * Time.deltaTime * horizontalInput);
                }
            }
            else
            {
                if (playerWing.transform.rotation.z < 0)
                {
                    playerWing.transform.Rotate(new Vector3(0, 0, 1) * rotationSpeed * Time.deltaTime);
                }

                if (playerWing.transform.rotation.z > 0)
                {
                    playerWing.transform.Rotate(new Vector3(0, 0, -1) * rotationSpeed * Time.deltaTime);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Decrease player health count after collsion with enemy laser.
        if (other.gameObject.tag == TAG_ENEMY_LASER && !playerBarrier.activeSelf)
        {
            if (!enemyLasersAlreadyCollided.Contains(other.gameObject.GetInstanceID()))
            gameManager.playerHealth -= 1;
            gameManager.UpdateHealthColor();
            gameManager.UpdateLifeTextAndColor();
            enemyLasersAlreadyCollided.Add(other.gameObject.GetInstanceID());
        }

        // Increase player health after collsion with health container.
        if (other.gameObject.tag == TAG_HEALTH_BONUS)
        {
            playerAudio.PlayOneShot(bunusPickedSound, 0.1f);
            gameManager.playerHealth += 1;
            gameManager.healthBonusSpawned = false;
            gameManager.UpdateHealthColor();
        }

        // Pick up barrier power up.
        if (other.gameObject.tag == TAG_BARRIER)
        {
            playerBarrier.SetActive(true);
            StartCoroutine(BarrierCountDownRoutine());
        }

        Destroy(other.gameObject);
        Debug.Log("Health = " + gameManager.playerHealth);
    }

    IEnumerator BarrierCountDownRoutine()
    {
        yield return new WaitForSeconds(barrierActiveTime);
        playerBarrier.SetActive(false);
        StartCoroutine(gameManager.SpawnBarrierCountDownRoutine());
    }
}
