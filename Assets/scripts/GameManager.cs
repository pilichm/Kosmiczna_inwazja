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

    // Start is called before the first frame update
    void Start()
    {
        CreateEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        // Create player left and right laser.
        if (Input.GetKeyDown("space"))
        {
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
        for (int i = 0; i < 4; i++)
        {
            for (int j=0; j<15; j++)
            {
                float enemyPositionX = -21 + j * 3;
                float enemyPositionY = 0;
                float enemyPositionZ = 10 + i * 3;

                Vector3 currentEnemyPosition = new Vector3(enemyPositionX, enemyPositionY, enemyPositionZ);
                Instantiate(enemyPrefab, currentEnemyPosition, Quaternion.Euler(90, 0, 0));
            }
        }
    }
}
