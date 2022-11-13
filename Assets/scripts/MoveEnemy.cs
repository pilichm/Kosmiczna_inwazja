using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    private float horizontalMoveBoundary = 10.0f;
    private float initialMoveSpeed;
    private float moveSpeed;
    private float rotationSpeed;
    private float initialX;
    private float direction;
    private float moveDirection;
    private const string TAG_PLAYER_LASER = "PlayerLaser";
    public AudioClip explosionSound;
    public GameManager gameManager;
    private float startDistabceToMove = 50.0f;
    private bool initialMovementEnded;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Randomise enemy movement - rotation, speed and direction.
        initialX = transform.position.x;
        moveDirection = UnityEngine.Random.Range(-1, 1);
        moveSpeed = UnityEngine.Random.Range(5, 10);
        initialMoveSpeed = UnityEngine.Random.Range(10, 20);
        rotationSpeed = UnityEngine.Random.Range(1, 10);
        initialMovementEnded = false;

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
    void Update()
    {
        float distance = System.Math.Abs(transform.position.x - initialX);

        if (System.Math.Abs(distance) < startDistabceToMove && !initialMovementEnded)
        {
            transform.Translate(direction * Vector3.right * initialMoveSpeed * Time.deltaTime);
        } else
        {
            initialMovementEnded = true;

            transform.Translate(1 * Vector3.right * moveSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                transform.eulerAngles.y + rotationSpeed,
                transform.eulerAngles.z);
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
}
