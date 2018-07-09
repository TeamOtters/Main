using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class DetectPickup : MonoBehaviour {

    public float m_speed = 10;

    private Transform m_carryLocation; // this is an empty gameobject childed under the Valkyrie, the character will be carried on this position
    private string m_collisionTag;
    private ValkyrieController m_valkyrie;

    Transform m_currentCarryable = null;

    private void Start()
    {
        m_collisionTag = null;
    }


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Viking is colliding with" + other.gameObject.tag);

        m_collisionTag = other.gameObject.tag;

        // pickup if it has tag "Valkyrie" and we are not carrying anything
        if (m_collisionTag == "Valkyrie" && m_currentCarryable == null)
        {
            GetPickedUp(other);
        }
        else
        {
            Debug.Log("Collided with: " + m_collisionTag + " which isn't a Valkyrie");
        }
    }


 
    //triggered on collision
    private void GetPickedUp(Collider other)
    {
        Debug.Log("Carrying viking!");
        m_valkyrie = other.gameObject.GetComponent<ValkyrieController>();

        //find the object within the colliding valkyrie with the "CarryLocation" tag and assign it as the carry location.
        m_carryLocation = m_valkyrie.gameObject.GetComponentInChildren<CarryLocation>().gameObject.transform;

        // take reference to that collided object
        m_currentCarryable = transform;

        // make it as a child of player, so it moves along with player
        m_currentCarryable.transform.parent = m_carryLocation.transform;
        //double-ensures that the carry location is assigned to the right valkyrie
        m_carryLocation.transform.parent = m_valkyrie.gameObject.transform;

        // move the carryable to the carrying point
        m_currentCarryable.position = m_carryLocation.transform.position;

        //assign the carryable rigidbody to the carrying valkyrie
       

        // Set the valkyrie to the isCarrying state
        m_valkyrie.isCarrying = true;
        m_valkyrie.heldRigidbody = m_currentCarryable.GetComponent<Rigidbody>();

        if(m_currentCarryable.GetComponent<PlayerData>() != null)
        {
            m_valkyrie.heldPlayerIndex = m_currentCarryable.GetComponent<PlayerData>().m_PlayerIndex;
        }

        // Set the viking to the isCarried state
        gameObject.GetComponent<VikingController>().SetCarried(true);


    }

    //triggered from the valkyrie script
    public void DropCarryable()
    {
        // remove as child
        m_currentCarryable.parent = null;

        //set position near player
        m_currentCarryable.position = transform.GetComponent<SpriteRenderer>().bounds.max;

        // release reference
        m_currentCarryable = null;

        // Set the viking to be un-carried
        gameObject.GetComponent<VikingController>().SetCarried(false);
    }
}
