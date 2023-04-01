using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float initialMoveSpeed;
    private float moveSpeed;
    private float rotationSpeed;
    private float initialX;
    private float direction;
    private float moveDirection;
    private float startDistabceToMove = 50.0f;
    private float downwardSpeed = 10.0f;

    private const string TAG_PLAYER_LASER = "PlayerLaser";

    public AudioClip explosionSound;

    public GameManager gameManager;
    
    private bool initialMovementEnded;
    private bool downwardMovementDelayEnded;
    private bool delayStarted;

    public GameObject player;

    private int maxDelayBeforeDownwardMovement = 300;
    private int enemyBasicMoveSpeed = 30;
    private int enemyBasicInitialMovementSpeed = 30;
    private int enemyBasicRotationSpeed = 30;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Randomise enemy movement - rotation, speed and direction.
        initialX = transform.position.x;

        enemyBasicMoveSpeed /= gameManager.difficulty;
        enemyBasicInitialMovementSpeed /= gameManager.difficulty;
        enemyBasicRotationSpeed /= gameManager.difficulty;

        moveDirection = UnityEngine.Random.Range(-1, 1);
        moveSpeed = UnityEngine.Random.Range(enemyBasicMoveSpeed/2, enemyBasicMoveSpeed);
        initialMoveSpeed = UnityEngine.Random.Range(enemyBasicInitialMovementSpeed, 2*enemyBasicInitialMovementSpeed);
        rotationSpeed = UnityEngine.Random.Range(1, enemyBasicRotationSpeed);
        initialMovementEnded = false;
        downwardMovementDelayEnded = false;
        delayStarted = false;

        if (moveDirection < 0)
        {
            moveSpeed *= -1;
        }

        if (initialX < 0)
        {
            direction = 1.0f;
        } else
        {
            direction = -1.0f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameManager.isPaused)
        {
            float distance = System.Math.Abs(transform.position.x - initialX);

            if (!gameManager.moreThanHalfEnemiesAreDestroyed && !delayStarted)
            {
                delayStarted = true;
                StartCoroutine(StartMovingTowardPlayerAfterRandomTime());
            }

            if (!downwardMovementDelayEnded)
            {
                if (System.Math.Abs(distance) < startDistabceToMove && !initialMovementEnded)
                {
                    transform.Translate(direction * Vector3.right * initialMoveSpeed * Time.deltaTime);
                }
                else
                {
                    initialMovementEnded = true;

                    transform.Translate(1 * Vector3.right * moveSpeed * Time.deltaTime);
                    transform.eulerAngles = new Vector3(
                        transform.eulerAngles.x,
                        transform.eulerAngles.y,
                        transform.eulerAngles.z + rotationSpeed);
                }
            }
            else
            {
                Vector3 directionTowardsPlayer = (player.transform.position - transform.position).normalized;
                transform.Translate(directionTowardsPlayer * downwardSpeed * Time.deltaTime);
                transform.rotation = player.transform.rotation;

            }
        }
    }

    // Destroy enemy on collision with player laser.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == TAG_PLAYER_LASER)
        {
            gameManager.enemyDestroyed = true;
            gameManager.updateScore();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    // Start moving toward player ship when more than half enemies are destroyed and random time has passed.
    IEnumerator StartMovingTowardPlayerAfterRandomTime()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0, maxDelayBeforeDownwardMovement));
        downwardMovementDelayEnded = true;
    }
}
