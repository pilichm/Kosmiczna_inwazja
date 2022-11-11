using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    private float horizontalMoveBoundary = 10.0f;
    private float speed;
    private float rotationSpeed;
    private float initialX;
    private float moveDirection;
    private const string TAG_PLAYER_LASER = "PlayerLaser";
    public AudioClip explosionSound;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Randomise enemy movement - rotation, speed and direction.
        initialX = transform.position.x;
        moveDirection = UnityEngine.Random.Range(-1, 1);
        speed = UnityEngine.Random.Range(2, 6);
        rotationSpeed = UnityEngine.Random.Range(1, 10);

        if (moveDirection < 0)
        {
            speed *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(1 * Vector3.right * speed * Time.deltaTime);
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y + rotationSpeed,
            transform.eulerAngles.z);

    }

    // Destroy enemy on collision with player laser.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == TAG_PLAYER_LASER)
        {
            gameManager.enemyDestroyed = true;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
