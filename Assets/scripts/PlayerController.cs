using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 15.0f;
    public float horizontalInput;
    public float verticalInput;
    private float horizontalBoundary = 18.0f;
    private float verticalBoundaryTop = 6.0f;
    private float verticalBoundaryBottom = -1.0f;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
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
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
        gameManager.playerHealth -= 1;
        gameManager.UpdateHealthColor();
    }
}
