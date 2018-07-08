using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkyrieAnimController : MonoBehaviour {

    public float m_speed = 2.0f;
    public float force = 300;

    private int m_playerIndex;
    private Rigidbody m_character;

	// Use this for initialization
	void Start ()
    {
        m_playerIndex = GetComponent<PlayerData>().myPlayerIndex;
        m_character = GetComponent<Rigidbody>();
        m_character.velocity = Vector2.up * m_speed;
	}
	
	// Update is called once per frame
	void Update ()
    {        
        // basic movement
        var x = Input.GetAxis("Horizontal_P1") * m_speed * Time.deltaTime;
        var y = Input.GetAxis("Vertical_P"   + m_playerIndex.ToString()) * m_speed * Time.deltaTime;

        transform.Translate(x, y, 0);

        Debug.Log("x: " + x);
        Debug.Log("y: " + y);

        if (Input.GetButtonDown("Jump_P" + m_playerIndex.ToString()))
        {
            GetComponent<Rigidbody>().AddForce(Vector2.up * force);
        }
		
	}
}
