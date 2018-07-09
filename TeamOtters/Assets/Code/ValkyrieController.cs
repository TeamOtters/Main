using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkyrieController : MonoBehaviour {

    public float m_speed;
    public float m_flightForce;

    internal bool isCarrying;
    internal Rigidbody heldRigidbody;
    internal int heldPlayerIndex;

    internal int m_playerIndex;
    private Rigidbody m_player;
    private Vector3 m_playerSize;
    private Vector3 m_heldCharacterSize;
    private BoundaryHolder m_boundaryHolder;

    private float m_leftBounds;
    private float m_rightBounds;
    private float m_topBounds;
    private float m_bottomBounds;

    // Use this for initialization
    void Start ()
    {
        m_playerIndex          = transform.parent.GetComponent<PlayerData>().m_PlayerIndex;
        m_player               = GameController.Instance.player.GetComponent<Rigidbody>();
        m_boundaryHolder       = GameController.Instance.boundaryHolder;
        m_playerSize           = GameController.Instance.player.GetComponentInChildren(typeof(ValkyrieController),true).GetComponent<SpriteRenderer>().bounds.extents;        
    }

    private void FixedUpdate()
    {
        //set the bounds value every frame to go with updated camera movement
        m_bottomBounds = m_boundaryHolder.playerBoundary.Down + m_playerSize.y;// + m_heldCharacterSize.y;
        m_topBounds = m_boundaryHolder.playerBoundary.Up - m_playerSize.y;
        m_leftBounds = m_boundaryHolder.playerBoundary.Left + m_playerSize.x;
        m_rightBounds = m_boundaryHolder.playerBoundary.Right - m_playerSize.x;// + m_heldCharacterSize.x;

        /*
        Debug.Log("Boundary Bottom: " + m_bottomBounds + " should be Down. The boundaryHolder's value for down is: " + m_boundaryHolder.playerBoundary.Down);
        Debug.Log("Boundary Top: " + m_topBounds + " should be Up. The boundaryHolder's value for up is: " + m_boundaryHolder.playerBoundary.Up);
        Debug.Log("Boundary Left: " + m_leftBounds + " should be Left. The boundaryHolder's value for left is: " + m_boundaryHolder.playerBoundary.Left);
        Debug.Log("Boundary Right: " + m_rightBounds + " should be Right. The boundaryHolder's value for right is: " + m_boundaryHolder.playerBoundary.Right);
        */

        // Basic movement input
        var x = Input.GetAxis("Horizontal_P" + m_playerIndex.ToString()) * m_speed * Time.deltaTime;
        var y = Input.GetAxis("Vertical_P" + m_playerIndex.ToString()) * m_speed * Time.deltaTime;

        Rigidbody[] myRigidbodies = GetComponents<Rigidbody>();

        // Flight movement input
        if (Input.GetButtonDown("Jump_P" + m_playerIndex.ToString()))
        {
            Debug.Log("Valkyrie Jump");
            foreach (Rigidbody rigidbody in myRigidbodies)
            {
                rigidbody.AddForce(Vector2.up * m_flightForce);
            }
           
        }
        if (Input.GetButtonDown("Fire2"))
        {
            if (heldRigidbody != null)
            {
                DropPickup();
            }
        }

        // Move
        //transform.Translate(x, y, transform.position.z);	
        Vector3 movement = new Vector3(x, y, 0f);
        foreach (Rigidbody rigidbody in myRigidbodies)
        {
            rigidbody.AddForce(movement * m_speed);

        }

        // Clamp movement
        if (transform.position.x < m_leftBounds)
        {
            Debug.Log("My position is: " + transform.position.x + "which is less than my Left boundary value: " + m_leftBounds);
            transform.position = new Vector3(m_leftBounds, transform.position.y, transform.position.z);
        }
        if (transform.position.x > m_rightBounds)
        {
            Debug.Log("My position is: " + transform.position.x + "which is greater than my Right boundary value: " + m_rightBounds);
            transform.position = new Vector3(m_boundaryHolder.playerBoundary.Right - m_playerSize.x, transform.position.y, transform.position.z);
        }
        if (transform.position.y < m_bottomBounds)
        {
            Debug.Log("My position is: " + transform.position.y + "which is less than my Down boundary value: " + m_bottomBounds);
            transform.position = new Vector3(transform.position.x, m_bottomBounds, transform.position.z);
        }
        if (transform.position.y > m_topBounds)
        {
            Debug.Log("My position is: " + transform.position.y + "which is greater than my Up boundary value: " + m_topBounds);
            transform.position = new Vector3(transform.position.x, m_topBounds, transform.position.z);
        }

    }
    public void DropPickup()
    {
        if (isCarrying == true)
        {
            // Set the valkyrie to not be isCarrying
            heldRigidbody.GetComponent<DetectPickup>().Dropped();
            isCarrying = false;
            heldRigidbody = null;
            heldPlayerIndex = 0;
        }

    }
   
}
   

