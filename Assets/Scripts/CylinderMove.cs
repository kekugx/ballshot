using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderMove : MonoBehaviour {

    float speed = 1f;
    Vector3 startPos;
    float startX;
    bool moveLeft = true;

    private void Start()
    {
        startPos = gameObject.transform.position;
        startX = startPos.x;
    }

    void Update () {
        if (gameObject.transform.position.x > (startX + 0.5f))
        {
            Debug.Log("false");
            moveLeft = false;
        }
        if (gameObject.transform.position.x < (startX - 0.5f))
        {
            Debug.Log("true");
            moveLeft = true;
        }

        // Sağa gidiş
        if (moveLeft)
        {
            transform.Translate(Vector3.down * Time.deltaTime * speed);
        }
        // Sola gidiş
        else
        {
            transform.Translate(Vector3.up * Time.deltaTime * speed);
        }
    }
}
