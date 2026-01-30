using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Swap the '0' and the 'v' so vertical input affects the Y axis
        Vector3 move = new Vector3(h, v, 0).normalized;

        transform.position += move * speed * Time.deltaTime;
    }
}

