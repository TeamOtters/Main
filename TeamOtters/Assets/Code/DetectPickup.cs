using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class DetectPickup : MonoBehaviour {

    public float m_speed = 10;
    public float m_immuneAfterDropDuration = 5f;

    private Transform m_carryLocation; // this is an empty gameobject childed under the Valkyrie, the character will be carried on this position
    private string m_collisionTag;
    private ValkyrieController m_valkyrie;
    internal bool m_immuneToPickUp = false;
    private GameObject m_ourParent; // Should be PLAYER DATA 
    private GameObject m_ourGrandparentLogic; // SHOULD BE LOGIC LAYER
    internal bool m_isPickedUp = false;
    

    private void Start()
    {
        m_collisionTag = string.Empty;
        m_ourParent = transform.parent.gameObject;
        m_ourGrandparentLogic = transform.parent.parent.gameObject;

    }


    void OnTriggerStay(Collider other)
    {
        Debug.Log("Viking is colliding with" + other.gameObject.tag);

        m_collisionTag = other.gameObject.tag;

        // pickup if it has tag "Valkyrie" and we are not carrying anything
        if (m_collisionTag == "Valkyrie" && m_immuneToPickUp == false)
        {
            m_valkyrie = other.gameObject.GetComponent<ValkyrieController>();
            if (m_valkyrie.isCarrying == false)
            {
                GetPickedUp(other);
            }
        }
    }


    

    //triggered on collision
    private void GetPickedUp(Collider other)
    {
        Debug.Log("Carrying viking!");

        m_isPickedUp = true;
        // Set the valkyrie to the isCarrying state and assign the carryable rigidbody to the carrying valkyrie
        m_valkyrie.isCarrying = true;
        m_valkyrie.heldRigidbody = GetComponent<Rigidbody>();

        //find the object within the colliding valkyrie with the "CarryLocation" tag and assign it as the carry location.
       // m_carryLocation = m_valkyrie.gameObject.GetComponentInChildren<CarryLocation>().gameObject.transform;
        m_carryLocation = m_valkyrie.gameObject.GetComponentInChildren(typeof(CarryLocation), true).gameObject.transform;

        // move the carryable to the carrying point
         transform.position = m_carryLocation.position;
       

        // make it as a child of player, so it moves along with player
        m_ourParent.transform.SetParent(m_carryLocation, true);

        //double-ensures that the carry location is assigned to the right valkyrie
      //  m_carryLocation.SetParent(m_valkyrie.gameObject.transform, true);
        

        
        
        if(GetComponentInParent<PlayerData>() != null)
        {
            m_valkyrie.heldPlayerIndex = GetComponentInParent<PlayerData>().m_PlayerIndex;
            Debug.Log("We are not null");
        }

        // Set the viking to the isCarried state
        gameObject.GetComponent<VikingController>().SetCarried(true);

        // Set immune to pickup to true so that it cannot be picked up by something else
        m_immuneToPickUp = true;


    }

    //triggered from the valkyrie script
    public void Dropped()
    { 
        // remove as child
        m_ourParent.transform.SetParent(m_ourGrandparentLogic.transform,true);
        
        // Set the viking to be un-carried
        m_isPickedUp= false;
        gameObject.GetComponent<VikingController>().SetCarried(false);
        transform.position = new Vector3(transform.position.x, transform.position.y - gameObject.GetComponent<SpriteRenderer>().bounds.extents.y, transform.position.z);
        StartCoroutine(ImmuneAfterDrop(m_immuneAfterDropDuration));
    }

    IEnumerator ImmuneAfterDrop(float myImmunityDuration)
    {
        yield return new WaitForSeconds(myImmunityDuration);
        m_immuneToPickUp = false;
    }
}
