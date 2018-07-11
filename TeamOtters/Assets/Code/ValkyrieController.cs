using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkyrieController : MonoBehaviour
{

    public float m_speed;
    public float m_flightForce;
    public float m_wrapScreenDelay = 0.5f;
    public float m_attackDuration = 1f;
    public float m_attackSpeed = .01f;
    public BoxCollider m_attackCollision;
    private GameController m_gameController;

    internal bool isCarrying;
    internal bool isAttacking;
    internal bool isShielding;
    internal Rigidbody heldRigidbody;
    internal int heldPlayerIndex;

    internal int m_playerIndex;
    private Rigidbody m_player;
    private Rigidbody m_otherPlayer;
    private Vector3 m_playerSize;
    private Vector3 m_heldCharacterSize;
    private BoundaryHolder m_boundaryHolder;

    private float m_leftBounds;
    private float m_rightBounds;
    private float m_topBounds;
    private float m_bottomBounds;

    //Go Through Platform
    private int m_playersLayer = 10;
    private int m_goThroughPlatformLayer = 11;
    private float m_currentVerticalPos;
    private float m_previousVerticalPos;
    private bool m_layerIsSet;
    private bool m_isPressingJump;

    public GameObject m_highestScoreEffect;

    // Use this for initialization
    void Start()
    {
        m_gameController = GameController.Instance;
        m_playerIndex = transform.parent.GetComponent<PlayerData>().m_PlayerIndex;
        m_player = GameController.Instance.player.GetComponent<Rigidbody>();
        m_boundaryHolder = GameController.Instance.boundaryHolder;
        m_playerSize = GameController.Instance.player.GetComponentInChildren(typeof(ValkyrieController), true).GetComponent<SpriteRenderer>().bounds.extents;
        StartCoroutine("ContiniouslyEvaluateScore");
    }

    IEnumerator ContiniouslyEvaluateScore()
    {
        yield return null;
        while (true)
        {
            yield return null;
            GiveContionousScore();
            yield return new WaitForSeconds(1f);
        }
    }

    private void GiveContionousScore()
    {
        if (heldRigidbody != null && m_gameController.m_currentPhaseState == 2)
            m_gameController.m_scoreManager.AddToScore(2, m_playerIndex);
    }

    private void FixedUpdate()
    {
        //set the bounds value every frame to go with updated camera movement
        m_bottomBounds = m_boundaryHolder.playerBoundary.Down + m_playerSize.y;// + m_heldCharacterSize.y;
        m_topBounds = m_boundaryHolder.playerBoundary.Up - m_playerSize.y;
        m_leftBounds = m_boundaryHolder.playerBoundary.Left + m_playerSize.x;
        m_rightBounds = m_boundaryHolder.playerBoundary.Right - m_playerSize.x;// + m_heldCharacterSize.x;

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

            if (!m_layerIsSet)
            {
                gameObject.layer = m_goThroughPlatformLayer;
                m_layerIsSet = true;
            }

            m_isPressingJump = true;

        }

        // Melee attack/drop Viking mechanic
        if (Input.GetButtonDown("Fire1_P" + m_playerIndex))
        {
            if (isAttacking == false)
            {
                Invoke("AttackStart", m_attackSpeed);

                if (heldRigidbody != null)
                {
                    DropPickup();
                }

                Invoke("AttackStop", m_attackDuration);
            }
        }

        // Shield/drop Viking mechanic TODO
        if (Input.GetButtonDown("Fire2"))
        {
            if (heldRigidbody != null)
            {
                //DropPickup();
            }
            else
            {
                Shield();
            }
        }

        // Move
        //transform.Translate(x, y, transform.position.z);	
        Vector3 movement = new Vector3(x, y, 0f);


        foreach (Rigidbody rigidbody in myRigidbodies)
        {

            rigidbody.AddForce(movement * m_speed);
            /*if (rigidbody.velocity.y > m_maxVelocity)
                rigidbody.Sleep();
            else
                rigidbody.AddForce(movement * m_speed);*/
        }

        //ClampVelocity();

        // Wrap screen enables once the camera gets above the "ground level"/Phase 1 screen
        /*if (m_bottomBounds > GameController.Instance.boundaryHolder.initialCameraBounds.Up)
        {
            m_canWrapScreen = true;
        }*/

        // Clamp movement and wrap screen logic
        if (transform.position.x < m_leftBounds)
        {
            //GetComponent<SpriteRenderer>().enabled = false; //hide character in foreground
            //Invoke("WrapScreenLeftToRight", m_wrapScreenDelay);            
            //GetComponent<SpriteRenderer>().enabled = true; //show character appearing after delay

            WrapScreenLeftToRight();
        }
        if (transform.position.x > m_rightBounds)
        {
            //GetComponent<SpriteRenderer>().enabled = false; //hide character in foreground
            //Invoke("WrapScreenRightToLeft", m_wrapScreenDelay);
            //GetComponent<SpriteRenderer>().enabled = true; //show character appearing after delay

            WrapScreenRightToLeft();
        }
        if (transform.position.y < m_bottomBounds)
        {
                transform.position = new Vector3(transform.position.x, m_bottomBounds, transform.position.z);
        }
        if (transform.position.y > m_topBounds)
        {
                transform.position = new Vector3(transform.position.x, m_topBounds, transform.position.z);
        }


        CheckIfGoingDown();
       

    }

    private void CheckIfGoingDown ()
    {
        m_currentVerticalPos = transform.position.y;
        float travel = m_currentVerticalPos - m_previousVerticalPos;

        if (m_currentVerticalPos < m_previousVerticalPos)
        {
            if (m_isPressingJump)
            {
                if (m_layerIsSet)
                {
                    m_layerIsSet = false;
                    gameObject.layer = m_playersLayer;
                    m_isPressingJump = false;
                }
            }
        }
    }

    private void LateUpdate()
    {
        m_previousVerticalPos = m_currentVerticalPos;
    }


    public void WrapScreenLeftToRight()
    {
        transform.position = new Vector3(m_rightBounds, transform.position.y, transform.position.z);        
    }

    public void WrapScreenRightToLeft()
    {
        transform.position = new Vector3(m_leftBounds, transform.position.y, transform.position.z);        
    }


    /*public void ClampVelocity()
    {
        float speed = Vector3.Magnitude(m_player.velocity);  // test current object speed

        if (speed > m_maxVelocity)
        {
            float brakeSpeed = speed - m_maxVelocity;  // calculate the speed decrease

            Vector3 normalisedVelocity = m_player.velocity.normalized;
            Vector3 brakeVelocity = normalisedVelocity * brakeSpeed;  // make the brake Vector3 value

            m_player.AddForce(-brakeVelocity);  // apply opposing brake force
        }
    }*/

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

    public void Shield()
    {
        Debug.Log("Valkyrie Shield");
    }

    public void AttackStart()
    {
        Debug.Log("Valkyrie Attack Started");

        isAttacking = true;
        m_attackCollision.enabled = true;
    }

    public void AttackStop()
    {
        Debug.Log("Valkyrie Attack Stopped");

        m_attackCollision.enabled = false;

        Debug.Log("Valkyrie Attack Box Collision Enable = " + m_attackCollision.enabled);

        isAttacking = false;
    }

    public void GetStunned()
    {
        Debug.Log("Valkyrie Stunned!");
    }
}



