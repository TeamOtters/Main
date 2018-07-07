using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkyrieMovementController : MonoBehaviour
{

    public float m_speed = 2.0f;
    public float force = 300;

    // Use this for initialization
    void Start()
    {

        GetComponent<Rigidbody2D>().velocity = Vector2.up * m_speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * force);
        }

    }
}
