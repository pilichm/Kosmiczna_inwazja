using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed = 15.0f;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        speed *= gameManager.difficulty;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameManager.isPaused)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }
}
