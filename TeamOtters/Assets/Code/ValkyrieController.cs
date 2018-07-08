using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkyrieController : MonoBehaviour {

    public float m_speed = 2.0f;
    public float m_flightForce = 300;
    public GameController m_gameController;

    private int m_playerIndex;
    private Rigidbody m_character;
    private BoundaryHolder m_boundaryHolder;
    private Vector3 m_playerSize;


    // Use this for initialization
    void Start ()
    {
        m_playerIndex          = transform.parent.GetComponent<PlayerData>().m_PlayerIndex;
        m_character            = GetComponent<Rigidbody>();
        m_boundaryHolder       = m_gameController.boundaryHolder;
        m_playerSize           = GetComponent<SpriteRenderer>().bounds.extents;
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
        if (transform.position.x < m_boundaryHolder.playerBoundary.Left + m_playerSize.x)
        {
            Debug.Log("My position is: " + transform.position.x + "which is less than my Left boundary value: " + m_boundaryHolder.playerBoundary.Left);
            transform.position = new Vector3(m_boundaryHolder.playerBoundary.Left + m_playerSize.x, transform.position.y, transform.position.z);
        }
        if (transform.position.x > m_boundaryHolder.playerBoundary.Right - m_playerSize.x)
        {
            Debug.Log("My position is: " + transform.position.x + "which is greater than my Right boundary value: " + m_boundaryHolder.playerBoundary.Right);
            transform.position = new Vector3(m_boundaryHolder.playerBoundary.Right - m_playerSize.x, transform.position.y, transform.position.z);
        }
        if (transform.position.y < m_boundaryHolder.playerBoundary.Down + m_playerSize.y)
        {
            Debug.Log("My position is: " + transform.position.y + "which is less than my Down boundary value: " + m_boundaryHolder.playerBoundary.Down);
            transform.position = new Vector3(transform.position.x, m_boundaryHolder.playerBoundary.Down + m_playerSize.y, transform.position.z);
        }
        if (transform.position.y > m_boundaryHolder.playerBoundary.Up - m_playerSize.y)
        {
            Debug.Log("My position is: " + transform.position.y + "which is greater than my Up boundary value: " + m_boundaryHolder.playerBoundary.Up);
            transform.position = new Vector3(transform.position.x, m_boundaryHolder.playerBoundary.Up - m_playerSize.y, transform.position.z);
        }
        
        // Move
        transform.Translate(x, y, transform.position.z);		
    }
}
