using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkyrieController : MonoBehaviour
{
    private Animator m_bodyAnimator;
    private Animator m_wingAnimator;        
    private GameController m_gameController;

    public BoxCollider m_attackCollision;
    public float m_speed;
    public float m_flightForce;
    public float m_wrapScreenDelay = 0.5f;
    public float m_attackDuration = 1f;
    public float m_attackSpeed = .01f;


    public SpriteRenderer m_bodySprite;
    public BoxCollider m_attackCollision;
    private GameController m_gameController;

    internal bool isIdle;
    internal bool isCarrying;
    internal bool isAttacking;
    internal bool isStunned;
    internal bool isDropping;
    internal bool isGliding;
    internal bool isFlapping;
    internal bool isCloseToViking;
    internal bool isDiving;
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
    private float m_currentHorizontalPos;
    private float m_previousHorizontalPos;
    private bool m_layerIsSet;
    private bool m_isPressingJump;
    private bool m_isFacingRight;
    private float m_thisScale;    

    public GameObject m_highestScoreEffect;

    // Use this for initialization
    void Start()
    {
        m_gameController = GameController.Instance;
        m_playerIndex = transform.parent.GetComponent<PlayerData>().m_PlayerIndex;
        m_player = GameController.Instance.player.GetComponent<Rigidbody>();
        m_boundaryHolder = GameController.Instance.boundaryHolder;
        m_playerSize = m_bodySprite.bounds.size;

        //wing animator
        m_wingAnimator = GetComponent<Animator>();

        //assign the valkyries body animator controller based on player index
        m_bodyAnimator = transform.Find("body_sprite").GetComponent<Animator>();
        m_bodyAnimator.runtimeAnimatorController = Resources.Load("Valkyrie_P" + m_playerIndex.ToString()) as RuntimeAnimatorController;

        // Set Idle animation state
        isIdle = true;
        SetValkyrieAnimationState(0); // Idle
        m_thisScale = transform.localScale.x;

        StartCoroutine("ContiniouslyEvaluateScore");
        StartCoroutine("ContinouslySetBoundaries");

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

    private void GiveContionousScore()
    {
        if (heldRigidbody != null && m_gameController.m_currentPhaseState == 2)
            m_gameController.m_scoreManager.AddToScore(ScorePointInfo.valkyrieContiniousScore, m_playerIndex);
    }

    private void Update()
    {
        //Anim States

        // Idle
        if(isIdle)
            SetValkyrieAnimationState(0);

        // Gliding
        if (isGliding)
            SetValkyrieAnimationState(1);

        // Flapping
        if (isFlapping)
            SetValkyrieAnimationState(2);

        // Carrying/Not carrying
        if (isCarrying)
            SetValkyrieAnimationState(3);

        // Grabby hands!
        if (isCloseToViking)
            SetValkyrieAnimationState(4);

        // Dive attack
        if (isDiving)
            SetValkyrieAnimationState(5);

        // Diving/melee attack
        if (isAttacking)
            SetValkyrieAnimationBool("isAttacking", true); 
        else
            SetValkyrieAnimationBool("isAttacking", false); 

        // Stunned
        if (isStunned)
            SetValkyrieAnimationBool("isStunned", true);
        else
            SetValkyrieAnimationBool("isStunned", false);

        // Dropping a viking
        if (isDropping)
            SetValkyrieAnimationBool("isDropping", true);
        else
            SetValkyrieAnimationBool("isDropping", false);

    }

    private void SetBoundaries()
    {
        //set the bounds value every frame to go with updated camera movement
        m_bottomBounds = m_boundaryHolder.playerBoundary.Down + m_playerSize.y;// + m_heldCharacterSize.y;
        m_topBounds = m_boundaryHolder.playerBoundary.Up - m_playerSize.y;
        m_leftBounds = m_boundaryHolder.playerBoundary.Left + m_playerSize.x;
        m_rightBounds = m_boundaryHolder.playerBoundary.Right - m_playerSize.x;// + m_heldCharacterSize.x;
   
        // Clamp movement and wrap screen logic
        if (transform.position.x < m_leftBounds)
            WrapScreenLeftToRight();

        if (transform.position.x > m_rightBounds)
            WrapScreenRightToLeft();

        if (transform.position.y < m_bottomBounds)
            transform.position = new Vector3(transform.position.x, m_bottomBounds, m_gameController.snapGridZ);
      
        if (transform.position.y > m_topBounds)
            transform.position = new Vector3(transform.position.x, m_topBounds, m_gameController.snapGridZ);
    }

    private void FixedUpdate()
    {
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
			isFlapping = true;

        }
		else
		isFlapping = false;

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

        m_currentHorizontalPos = transform.position.x;
        float travelxLeft = m_currentHorizontalPos - m_previousHorizontalPos;
        float travelxRight = m_currentHorizontalPos + m_previousHorizontalPos;

        // Actually moving
        if (travelxLeft != m_previousHorizontalPos && travelxRight != m_previousHorizontalPos)
        {
            isGliding = true;
            isIdle = false;
        }

        if (m_currentHorizontalPos == m_previousHorizontalPos)
        {
            isIdle = true;
            isGliding = false;
        }

        //ClampVelocity();

        // Wrap screen enables once the camera gets above the "ground level"/Phase 1 screen
        /*if (m_bottomBounds > GameController.Instance.boundaryHolder.initialCameraBounds.Up)
        {
            m_canWrapScreen = true;
        }*/
      

        CheckIfGoingDown();

        var angle = Mathf.Atan2(Input.GetAxis("Horizontal_P" + m_playerIndex.ToString()), Input.GetAxis("Vertical_P" + m_playerIndex.ToString())) * Mathf.Rad2Deg;

        //Right - Sprite Flip
        if (Mathf.Clamp(angle, 10, 170) == angle)
        {
            Vector3 scale = transform.localScale;
            scale.x = -m_thisScale;
            transform.localScale = scale;
          
            SetValkyrieAnimationBool("facingRight", true); // Facing right
           
        }

        //Left - Sprite Flip
        else if (Mathf.Clamp(angle, -170, -10) == angle)
        {
            Vector3 scale = transform.localScale;
            scale.x = +m_thisScale;
            transform.localScale = scale;

            SetValkyrieAnimationBool("facingRight",false); // Facing left
            
        }


    }

    private void SetValkyrieAnimationBool(string boolString, bool enable)
    {
        m_wingAnimator.SetBool(boolString, enable); 
        m_bodyAnimator.SetBool(boolString, enable); 
    }

    private void SetValkyrieAnimationState(int state)
    {
        m_wingAnimator.SetInteger("State", state);
        m_bodyAnimator.SetInteger("State", state);
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
            isDropping = true;
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



