using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour {

    private Rigidbody m_rigidBody;
    private Collider m_collider;
    public int m_playerID;
    public float m_rotationSpeed;
    private bool m_hit;

    // Use this for initialization
    void Start ()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        EnableRagdoll();
        m_collider = GetComponent<Collider>();
       
    }

    private void Update()
    {

        //transform.rotation = Quaternion.Euler(50 * Time.deltaTime,0, 0);
        if(!m_hit)
            transform.Rotate(0, 0, m_rotationSpeed * Time.deltaTime, Space.Self);

        // transform.Rotate(new Vector3(1, 0, 0));
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player"))
        {
            DisableRagdoll();
           
            //    Physics.IgnoreCollision(collision.collider, this.transform.GetComponent<Collider>());


        }

        if(collision.collider.CompareTag("Player"))
        {
            if (collision.collider.gameObject.GetComponent<PlayerData>().myPlayerIndex == m_playerID)
                Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider, true);
        }

        if(collision.collider.CompareTag("Scoreable"))
        {
            Debug.Log("CollectScore!");
        }
    }
    
    private void EnableRagdoll()
    {
        if (m_rigidBody != null)
        {
            m_rigidBody.isKinematic = false;
            m_rigidBody.detectCollisions = true;
        }
    }
     private void DisableRagdoll()
    {
        if (m_rigidBody != null)
        {
            m_rigidBody.isKinematic = true;
            m_rigidBody.detectCollisions = false;
            Destroy(m_collider);
            m_hit = true;
        }
    }

}
