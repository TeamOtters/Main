﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPickup : MonoBehaviour {

    public float m_speed = 10;
    public Transform m_carryLocation; // this is an empty gameobject childed under the Valkyrie, the character will be carried on this position

    private string m_collisionTag;
    private ValkyrieController m_valkyrie;

    Transform m_currentCarryable = null;

    private void Start()
    {
        m_collisionTag = null;
    }

    private void Update()
    {
        // press to drop viking/item, if carrying one
        if (Input.GetButtonDown("Fire2"))
        {
            if (m_currentCarryable != null)
            {
                // remove as child
                m_currentCarryable.parent = null;

                //set position near player
                m_currentCarryable.position = transform.GetComponent<SpriteRenderer>().bounds.max;

                // release reference
                m_currentCarryable = null;

                // Set the valkyrie to not be isCarrying
                m_valkyrie.isCarrying = false;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.tag);

        m_collisionTag = other.gameObject.tag;   
    
        // pickup if it has tag "Valkyrie" and we are not carrying anything
        if (m_collisionTag == "Valkyrie" && m_currentCarryable == null)
        {
            m_valkyrie = other.gameObject.GetComponent<ValkyrieController>();

            // take reference to that collided object
            m_currentCarryable = transform;

            // move it to carrying point
            m_currentCarryable.position = m_carryLocation.position;

            // make it as a child of player, so it moves along with player
            m_currentCarryable.parent = m_carryLocation.transform;

            // Give the valkyrie the rigidbody of the viking
            m_valkyrie.heldCharacter = GetComponentInParent<Rigidbody>();

            // Set the valkyrie to the isCarrying state
            m_valkyrie.isCarrying = true;
        }
        else
        {
            Debug.Log("Collided with: " + m_collisionTag + " which isn't a Valkyrie");
        }
    }
}
