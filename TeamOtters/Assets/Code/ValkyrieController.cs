using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkyrieController : MonoBehaviour {

    public float m_speed = 2.0f;
    public float m_flightForce = 300;
    public Transform m_boundaryHolder;

    private int m_playerIndex;
    private Vector3 m_playerSize;
    private Rigidbody m_character;    

    Boundary m_playerBoundary;

    struct Boundary
    {
        public float Down, Up, Left, Right;

        public Boundary(float down, float up, float left, float right)
        {
            Down = down; Up = up; Left = left; Right = right;
        }
    }

    // Use this for initialization
    void Start ()
    {
        m_playerIndex = transform.parent.GetComponent<PlayerData>().myPlayerIndex;
        m_character   = GetComponent<Rigidbody>();
        m_playerSize  = GetComponent<SpriteRenderer>().bounds.extents;

        m_playerBoundary = new Boundary(m_boundaryHolder.GetChild(0).position.y + m_playerSize.y,  //down
                                        m_boundaryHolder.GetChild(1).position.y - m_playerSize.y,  //up
                                        m_boundaryHolder.GetChild(2).position.x + m_playerSize.x,  //left
                                        m_boundaryHolder.GetChild(3).position.x - m_playerSize.x); //right

        /*Debug.Log("Boundary Index 0: " + m_playerBoundary.Down + " should be Down");
        Debug.Log("Boundary Index 1: " + m_playerBoundary.Up + " should be Up");
        Debug.Log("Boundary Index 2: " + m_playerBoundary.Left + " should be Left");
        Debug.Log("Boundary Index 3: " + m_playerBoundary.Right + " should be Right");*/        

    }
	
	// Update is called once per frame
	void Update ()
    {
        // Basic movement input
        var x = Input.GetAxis("Horizontal_P" + m_playerIndex.ToString()) * m_speed * Time.deltaTime;
        var y = Input.GetAxis("Vertical_P"   + m_playerIndex.ToString()) * m_speed * Time.deltaTime;

        // Flight movement input
        if (Input.GetButtonDown("Jump_P" + m_playerIndex.ToString()))
        {
            GetComponent<Rigidbody>().AddForce(Vector2.up * m_flightForce);
        }
        
        // Clamp movement
        if (transform.position.x < m_playerBoundary.Left)
        {
            Debug.Log("My position is: " + transform.position.x + "which is less than my Left boundary value: " + m_playerBoundary.Left);
            transform.position = new Vector3(m_playerBoundary.Left, transform.position.y, transform.position.z);
        }
        if (transform.position.x > m_playerBoundary.Right)
        {
            Debug.Log("My position is: " + transform.position.x + "which is greater than my Right boundary value: " + m_playerBoundary.Right);
            transform.position = new Vector3(m_playerBoundary.Right, transform.position.y, transform.position.z);
        }
        if (transform.position.y < m_playerBoundary.Down)
        {
            Debug.Log("My position is: " + transform.position.y + "which is less than my Down boundary value: " + m_playerBoundary.Down);
            transform.position = new Vector3(transform.position.x, m_playerBoundary.Down, transform.position.z);
        }
        if (transform.position.y > m_playerBoundary.Up)
        {
            Debug.Log("My position is: " + transform.position.y + "which is greater than my Up boundary value: " + m_playerBoundary.Up);
            transform.position = new Vector3(transform.position.x, m_playerBoundary.Up, transform.position.z);
        }

        // Clamp valkyrie movement within bounds of screen
        /*Vector2 clampedScreenPos = new Vector2(Mathf.Clamp(x, m_playerBoundary.Left,
                                                              m_playerBoundary.Right),
                                               Mathf.Clamp(y, m_playerBoundary.Down,
                                                              m_playerBoundary.Up));*/

        // Move
        transform.Translate(x, y, transform.position.z);		
    }
}
