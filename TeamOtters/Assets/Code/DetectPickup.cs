﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPickup : MonoBehaviour {

    public float m_speed = 10;
    public Transform m_carryLocation; // this is an empty gameobject childed under the Valkyrie, the character will be carried on this position

    Transform m_currentCarryable = null;

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
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.tag);
        // pickup if it has tag "Item" and we are not carrying anything
        if (other.rigidbody.CompareTag("Viking") && m_currentCarryable == null)
        {
            // take reference to that collided object
            m_currentCarryable = other.transform;

            // move it to carrying point
            m_currentCarryable.position = m_currentCarryable.position;

            // make it as a child of player, so it moves along with player
            m_currentCarryable.parent = transform;
        }
    }
}
