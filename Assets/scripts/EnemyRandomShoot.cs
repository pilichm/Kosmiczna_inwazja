using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomShoot : MonoBehaviour
{
    public GameObject enemyLaserPrefab;

    public GameManager gameManager;

    private System.Random rnd;

    private int shootDelay;
    private int shootInterval;
    private int maxIntervalTime = 30;

    private AudioSource enemyAudio;

    public AudioClip laserSound;

    // Start is called before the first frame update
    void Start()
    {
        // Randomise time when enemy starts shooting and delay between each shot.
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        shootDelay = UnityEngine.Random.Range(0, maxIntervalTime);
        shootInterval = UnityEngine.Random.Range(0, maxIntervalTime);
        InvokeRepeating("Shoot", shootDelay, shootInterval);
        enemyAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Make enemy shoot laser down the screen.
    void Shoot()
    {
        if (!gameManager.isPaused && !gameManager.gameStartedFromButton)
        {
            enemyAudio.PlayOneShot(laserSound, 0.1f);
            Instantiate(enemyLaserPrefab, transform.position, Quaternion.Euler(90, 0, 0));
        }
    }
}
