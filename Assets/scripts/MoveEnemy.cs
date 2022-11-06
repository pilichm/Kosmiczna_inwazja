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

    // Start is called before the first frame update
    void Start()
    {
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
}
