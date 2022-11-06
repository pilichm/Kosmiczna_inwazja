using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomShoot : MonoBehaviour
{
    public GameObject enemyLaserPrefab;
    private System.Random rnd;
    private int shootDelay;
    private int shootInterval;
    private int maxIntervalTime = 30;

    // Start is called before the first frame update
    void Start()
    {
        // Randomise time when enemy starts shooting and delay between each shot.
        shootDelay = UnityEngine.Random.Range(0, maxIntervalTime);
        shootInterval = UnityEngine.Random.Range(0, maxIntervalTime);
        InvokeRepeating("Shoot", shootDelay, shootInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Make enemy shoot laser down the screen.
    void Shoot()
    {
        Instantiate(enemyLaserPrefab, transform.position, Quaternion.Euler(90, 0, 0));
    }
}
