using System.Collections;
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


    // Use this for initialization
    void Start ()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
        EnableRagdoll();
        m_playerID = m_playerData.m_PlayerIndex;
        m_gameController = GameController.Instance;

        m_platformLayer = LayerMask.GetMask("Platform");
        m_scoreManager = m_gameController.m_scoreManager;

    }

    private void Update()
    {
        //transform.rotation = Quaternion.Euler(50 * Time.deltaTime,0, 0);
        if(!m_hit)
            transform.Rotate(0, 0, m_rotationSpeed * Time.deltaTime, Space.Self);

        
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        if(m_retracting)
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
                m_scoreManager.AddToScore( m_gameController.bounceHit, m_playerID); 
            }

            if (m_scoreManager == null)
                Debug.Log("Couldn't find scoreManager");
        }
        if (collision.collider.CompareTag("Valkyrie"))
        {
            if (collision.collider.gameObject.GetComponentInParent<PlayerData>().m_PlayerIndex != m_playerID)
            {
                m_scoreManager.AddToScore(m_gameController.hitOpponent, m_playerID);
                Debug.Log("Viking hit valkyrie");
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
