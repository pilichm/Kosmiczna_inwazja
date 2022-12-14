using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDownward : MonoBehaviour
{
    public float speed = 30.0f;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        speed /= gameManager.difficulty;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.isPaused)
        {
            transform.Translate(-Vector3.up * speed * Time.deltaTime);
        }
    }
}
