using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectOutOfBoundary : MonoBehaviour
{
    public float maxTopZ = 25.0f;
    public float minBottomZ = -10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.z < minBottomZ || gameObject.transform.position.z > maxTopZ)
        {
            Destroy(gameObject);
        }
    }
}
