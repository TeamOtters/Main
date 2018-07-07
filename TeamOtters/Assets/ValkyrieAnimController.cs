using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkyrieAnimController : MonoBehaviour {

    public float m_speed = 2.0f;
    public float force = 300;

    private Rigidbody2D m_character;

	// Use this for initialization
	void Start () {

        m_character = GetComponent<Rigidbody2D>();
        m_character.velocity = Vector2.up * m_speed;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * force);
        }
		
	}

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        m_character.AddForce(movement * m_speed);
    }
}
