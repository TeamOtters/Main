﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour {

    private Rigidbody m_rigidBody;
    private Collider m_collider;
    public int m_playerID;
    public float m_rotationSpeed;
    public bool m_hit;
    public int m_projectileDamage = 10;
    private bool m_retracting;
    public PlayerData m_playerData;
    private GameController m_gameController;
    private ScoreManager m_scoreManager;
    private int m_platformLayer;

    internal float m_leftBounds;
    internal float m_rightBounds;
    internal float m_topBounds;
    internal float m_bottomBounds;

    private BoundaryHolder m_boundaryHolder;
    private Vector3 m_projectileSize;

    //public Collider m_meshCollider; 


    // Use this for initialization
    void Awake ()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
        EnableRagdoll();
        m_gameController = GameController.Instance;

        m_platformLayer = LayerMask.GetMask("Platform");
        m_scoreManager = m_gameController.m_scoreManager;

        m_boundaryHolder = GameController.Instance.boundaryHolder;
        m_projectileSize = GetComponent<SpriteRenderer>().bounds.extents;
        StartCoroutine(LateAwake());
        StartCoroutine("ContinouslySetBoundaries");

     
    }

    IEnumerator LateAwake()
    {
        yield return new WaitForEndOfFrame();
        m_playerID = m_playerData.m_PlayerIndex;
        Debug.Log("My player is player " + m_playerID);
    }

    IEnumerator ContinouslySetBoundaries()
    {
        yield return null;
        while (true)
        {
            yield return null;
            //Set the boundaries to camera
            SetBoundaries();
            yield return new WaitForEndOfFrame();
        }
    }

    //End of Frame(after camera has rendered )
    private void SetBoundaries()
    {
        //set the bounds value every frame to go with updated camera movement
        m_bottomBounds = m_boundaryHolder.playerBoundary.Down + m_projectileSize.y;
        m_topBounds = m_boundaryHolder.playerBoundary.Up - m_projectileSize.y;
        m_leftBounds = m_boundaryHolder.playerBoundary.Left + m_projectileSize.x;
        m_rightBounds = m_boundaryHolder.playerBoundary.Right - m_projectileSize.x;

        //Set position to bounds
        if (transform.position.x < m_leftBounds)
            transform.position = new Vector3(m_leftBounds, transform.position.y, transform.position.z);

        if (transform.position.x > m_rightBounds)
            transform.position = new Vector3(m_rightBounds, transform.position.y, transform.position.z);

        if (transform.position.y < m_bottomBounds )
            transform.position = new Vector3(transform.position.x, m_bottomBounds, transform.position.z);

        if (transform.position.y > m_topBounds)
            transform.position = new Vector3(transform.position.x, m_topBounds, transform.position.z);

        if (transform.position.x == m_rightBounds || transform.position.x == m_leftBounds || transform.position.y == m_topBounds || transform.position.y == m_bottomBounds)
            DisableRagdoll();
    }


    private void Update()
    {
        //transform.rotation = Quaternion.Euler(50 * Time.deltaTime,0, 0);
        if(!m_hit)
            transform.Rotate(0, 0, m_rotationSpeed * Time.deltaTime, Space.Self);

       

    }
    

    private void OnCollisionEnter(Collision collision)
    {
        if (!m_retracting && !collision.collider.CompareTag("Viking") && !collision.collider.CompareTag("Valkyrie") && !collision.collider.CompareTag("Scoreable") && !collision.collider.CompareTag("BouncingBall"))
            AudioManager.Instance.PlayerAxeHitSound();

        if(collision.collider.CompareTag("Viking") && collision.collider.CompareTag("Valkyrie"))
            AudioManager.Instance.PlayerGotHitSound();


        if (m_retracting)
        {
            if (collision.collider.gameObject.layer == m_platformLayer)
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider, true);
            }
        }
            

        if (!collision.collider.CompareTag("Viking") && !collision.collider.CompareTag("Valkyrie"))
        {
            DisableRagdoll();
        }

        if(collision.collider.CompareTag("Viking"))
        {
            if (collision.collider.gameObject.GetComponentInParent<PlayerData>().m_PlayerIndex == m_playerID)
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider, true);
            }
        }

        if(collision.collider.CompareTag("Scoreable"))
        {
            Debug.Log("CollectScore!");
        }

        if (collision.collider.CompareTag("BouncingBall"))
        {
            Debug.Log("damage dealt");

            //Calling script on BouncingBall to drain HP
            collision.gameObject.GetComponent<BouncingBall>().TakeDamage(m_projectileDamage);

            if (m_scoreManager != null)
            {
                Debug.Log("I want to give score to player " + m_playerID);
                m_scoreManager.AddToScore( ScorePointInfo.bounceHit, m_playerID);
            }

            if (m_scoreManager == null)
                Debug.Log("Couldn't find scoreManager");
        }
        if (collision.collider.CompareTag("Valkyrie"))
        {
            if (collision.collider.gameObject.GetComponentInParent<PlayerData>().m_PlayerIndex != m_playerID)
            {
                GameObject vikingController = m_playerData.GetComponentInChildren(typeof(VikingController), true).gameObject;
                if (!vikingController.GetComponent<VikingController>().m_isCarried)
                {
                    m_scoreManager.AddToScore(ScorePointInfo.hitOpponent, m_playerID); // this bugs out when carried
                    Debug.Log("Viking hit valkyrie");
                }
                else if(collision.collider.gameObject.GetComponentInParent<PlayerData>().m_PlayerIndex != vikingController.GetComponent<DetectPickup>().m_valkyrie.GetComponentInParent<PlayerData>().m_PlayerIndex)
                {
                    m_scoreManager.AddToScore(ScorePointInfo.hitOpponent, m_playerID); // this bugs out when carried
                    Debug.Log("Viking hit valkyrie");
                }
                else
                {
                    Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
                }
            }
        }
    }

    public void EnableRagdoll()
    {
        if (m_rigidBody != null)
        {
            m_rigidBody.isKinematic = false;
            m_rigidBody.detectCollisions = true;
            m_collider.enabled = true;
            m_hit = false;

        }
    }


     public void DisableRagdoll()
    {
        if (m_rigidBody != null && !m_retracting)
        {
            if(!m_hit)
             AudioManager.Instance.PlayerAxeHitSound();
            m_rigidBody.isKinematic = true;
            m_rigidBody.detectCollisions = false;
            // StartCoroutine("DestroyProjectileTimer");
            m_collider.enabled = false;
            
            m_hit = true;
           
        }
    }

    IEnumerator DestroyProjectileTimer()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

    public void SetRetractingState (bool enable)
    {
        m_retracting = enable;
    }

}
