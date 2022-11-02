using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomShoot : MonoBehaviour
{
    public GameObject enemyLaserPrefab;
    private System.Random rnd;
    private int spawnDelay;
    private int spawnInterval;
    private int maxIntervalTime = 30;

    // Start is called before the first frame update
    void Start()
    {
        rnd = new System.Random();
        spawnDelay = rnd.Next(0, maxIntervalTime);
        spawnInterval = rnd.Next(0, maxIntervalTime);
        InvokeRepeating("Shoot", spawnDelay, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Shoot()
    {
        Instantiate(enemyLaserPrefab, transform.position, Quaternion.Euler(90, 0, 0));
    }
}
