using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    private float horizontalMoveBoundary = 10.0f;
    private float speed = 2.0f;
    private float initialX;

    // Start is called before the first frame update
    void Start()
    {
        initialX = transform.position.x;
        Debug.Log(initialX);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(-1 * Vector3.right * speed * Time.deltaTime);
        
    }
}
