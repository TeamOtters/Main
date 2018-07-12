using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingController : MonoBehaviour
{
    private Animator m_animator;
    private float m_vikingSpeedForce;
    private CharacterController m_vikingcCharacterController;
    private VikingProjectiles m_vikingProjectiles;
    private PlayerData m_playerData;
    public GameObject m_loadProjectile;
    private GameObject m_currentProjectile;
    public GameObject m_hand;
    private Rigidbody m_rb;
    private GameController m_gameController;

    public float m_vikingMovementSpeed = 6.0F;
    public float m_vikingJumpSpeed = 8.0F;
    public float m_vikingGravityForce = 20.0F;
    private Vector3 m_vikingMoveDirection = Vector3.zero;
    public float m_isGroundedOffset;
    private float m_isGroundedTimer;


    public float m_projectileForceX;
    public float m_projectileForceY;
    public float m_projectileSpeed;
    public float m_projectileRetractTime;

    private float m_rapidFireSpeed = 0.2f;
    private bool m_fireCooldownOn;
    // private int m_allowedAmmo = 1;
    private int m_currentAmmo = 1;

    public bool m_isStunned;
    private bool m_isCarried;
    private bool m_isIdle;
    private bool m_collided;
    private bool m_turnedLeft;
    private bool m_isRetracting;
    private bool m_isGrounded;
    private bool m_askForGoundedOffset;
    private bool m_isJumping;
    private bool m_goThroughPlatformDown;
    private bool m_isWallJumping;
    private bool m_layerIsSet;
    private bool m_isWallClinging;


    public float m_stunnedCoolDown = 1f;
    private float m_wallStickCoolDown = 0.3f;

    private int m_thisPlayerIndex;
    private string m_playerIndexString;

    public GameObject m_highestScoreEffect;
    private int m_playersLayer;
    private int m_goThroughPlatformLayer;

    private float m_currentVerticalPos;
    private float m_previousVerticalPos;
    private float m_currentHorizontalPos;
    private float m_previousHorizontalPos;

    private BoundaryHolder m_boundaryHolder;

    internal float m_leftBounds;
    internal float m_rightBounds;
    internal float m_topBounds;
    internal float m_bottomBounds;
    private Vector3 m_playerSize;
    private VikingRespawn m_vikingRespawn;
    private DetectPickup m_detectPickup;





    // Use this for initialization
    void Start()
    {   
        m_vikingcCharacterController = GetComponent<CharacterController>();
        m_playerData = GetComponentInParent<PlayerData>();
        m_gameController = GameController.Instance;

        if (m_playerData != null)
            m_thisPlayerIndex = m_playerData.m_PlayerIndex;

        m_playerIndexString = m_thisPlayerIndex.ToString();

        m_rb = GetComponent<Rigidbody>();
        m_isGroundedTimer = m_isGroundedOffset;

        m_playersLayer = 10;
        m_goThroughPlatformLayer = 11;


        m_playerSize = GameController.Instance.player.GetComponentInChildren(typeof(VikingController), true).GetComponent<SpriteRenderer>().bounds.extents;
        m_boundaryHolder = GameController.Instance.boundaryHolder;
        m_vikingRespawn = GetComponent<VikingRespawn>();
        m_detectPickup = GetComponent<DetectPickup>();

        //assign an animator controller based on player index
        m_animator = transform.gameObject.GetComponent<Animator>();
        m_animator.runtimeAnimatorController = Resources.Load("Viking_P" + m_playerIndexString) as RuntimeAnimatorController;

        m_animator.SetInteger("State", 0); // Idle
        StartCoroutine("ContiniouslyEvaluateScore");
        StartCoroutine("ContinouslySetBoundaries");
    }

    private void Update()
    {
        //Set the boundaries to camera


        if (m_askForGoundedOffset)
                m_isGroundedTimer -= Time.deltaTime;

        if(m_isGroundedTimer <= 0.0f)
        {
            m_isGrounded = false;
            m_askForGoundedOffset = false;
            m_isGroundedTimer = m_isGroundedOffset;
        }


        //float travel = m_currentVerticalPos - m_previousVerticalPos;

        if (m_currentVerticalPos < m_previousVerticalPos)
        {
             if (m_isJumping)
             {
                if (m_layerIsSet)
                {
                    m_layerIsSet = false;
                    gameObject.layer = m_playersLayer;
                    m_isJumping = false;
                }
             }

            CheckWallClinging();
        }

        if(m_goThroughPlatformDown)
        {
            if(m_vikingcCharacterController.isGrounded)
            {
                m_goThroughPlatformDown = false;
                gameObject.layer = m_playersLayer;
                m_layerIsSet = false;
            }
        }


    }

    IEnumerator ContinouslySetBoundaries()
    {
        yield return null;
        while (true)
        {
            yield return null;
            //Set the boundaries to camera
            if (!m_isCarried)
            SetBoundaries();
            yield return new WaitForEndOfFrame();
        }
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
        if (!m_isCarried && m_gameController.m_currentPhaseState == 2)
            m_gameController.m_scoreManager.AddToScore(ScorePointInfo.playerContiniousScore, m_thisPlayerIndex);
    }

    void FixedUpdate()
    {

        if (!m_isStunned)
        {
            if (Input.GetButtonDown("Fire1_P" + m_playerIndexString))
            {
                if (m_currentAmmo != 0)
                    VikingFire();
                else if (m_currentAmmo == 0 && !m_isRetracting)
                    RetractProjectile();
            }

            VikingMovement();

        }

        else
        {
            m_vikingMoveDirection.y -= m_vikingGravityForce * Time.deltaTime;
            m_vikingcCharacterController.Move(m_vikingMoveDirection * Time.deltaTime);
        }

        if (m_isRetracting)
            ProjectileRetractingUpdate();

        m_currentVerticalPos = transform.position.y;

        

            // float travel = m_currentVerticalPos - m_previousVerticalPos;


            // Only do work if meaningful
            /*  if (vNewInput.sqrMagnitude < 0.1f)
              {
                  return;
              }*/


        }

    private void LateUpdate()
    {
        m_previousVerticalPos = m_currentVerticalPos;
        m_previousHorizontalPos = m_currentHorizontalPos;
    }



    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Viking"))
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), hit.collider, true);
        }

        if (!m_vikingcCharacterController.isGrounded)
        {
            //m_collided = true;
            GetComponent<Rigidbody>().AddForce(transform.position * 10f);
        }

        if (!m_vikingcCharacterController.isGrounded && !m_collided)
        {
            // how much the character should be knocked back
            var magnitude = 5; 
            m_collided = true;

            m_vikingMoveDirection.y -= m_vikingGravityForce * magnitude * Time.deltaTime;
            m_vikingcCharacterController.Move(m_vikingMoveDirection * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            if (collision.gameObject.GetComponent<ProjectileBehaviour>().m_playerID != m_thisPlayerIndex)
                SetStunned();

        }

        if (collision.collider.CompareTag("Viking"))
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider, true);
        }

        if (collision.contacts[0].normal.y > 0.1)
        {
            print("First normal of the point that collide: " + collision.contacts[0].normal);

            if (m_isJumping)
            {
                if (m_layerIsSet)
                {
                    m_layerIsSet = false;
                    gameObject.layer = m_playersLayer;
                    m_isJumping = false;
                }
            }
        }

        /*  // force is how forcefully we will push the player away from the enemy.
          float force = 50;

          // If the object we hit is the enemy
          if (!m_vikingcCharacterController.isGrounded)
          {
              // Calculate Angle Between the collision point and the player
              Vector3 dir = collision.contacts[0].point - transform.position;
              // We then get the opposite (-Vector3) and normalize it
              dir = -dir.normalized;
              // And finally we add force in the direction of dir and multiply it by force. 
              // This will push back the player
              GetComponent<Rigidbody>().AddForce(dir * force);
              m_collided = true;
          }*/
    }

    public void SetCarried(bool enable)
    {
        m_isCarried = enable;
        if (m_isCarried)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<Rigidbody>().detectCollisions = false;
            gameObject.GetComponent<CharacterController>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<Rigidbody>().detectCollisions = true;
            gameObject.GetComponent<CharacterController>().enabled = true;
        }

    }


    //From Fixed Update
    private void SetBoundaries()
    {
		//set the bounds value every frame to go with updated camera movement
        m_bottomBounds = m_boundaryHolder.playerBoundary.Down + m_playerSize.y;
        m_topBounds = m_boundaryHolder.playerBoundary.Up - m_playerSize.y;
        m_leftBounds = m_boundaryHolder.playerBoundary.Left + m_playerSize.x;
        m_rightBounds = m_boundaryHolder.playerBoundary.Right - m_playerSize.x;

        //Set position to bounds
        if (transform.position.x < m_leftBounds)
        {
            transform.position = new Vector3(m_leftBounds, transform.position.y, transform.position.z);
        }
        if (transform.position.x > m_rightBounds)
        {
            transform.position = new Vector3(m_rightBounds, transform.position.y, transform.position.z);
        }

        if (transform.position.y < m_bottomBounds && !m_vikingRespawn.m_hasRespawned && !m_detectPickup.m_isPickedUp)
        {
            m_vikingRespawn.Respawn();// this is where fall penalty should go
        }
        
        if (transform.position.y > m_topBounds)
        {
            transform.position = new Vector3(transform.position.x, m_topBounds, transform.position.z);
        }
    }


    //Called From Fixed Update
    private void VikingFire()
    {
        Vector3 vNewInput = new Vector3(Input.GetAxis("Horizontal_P" + m_playerIndexString), Input.GetAxis("Vertical_P" + m_playerIndexString), 0.0f);
        var angle = Mathf.Atan2(Input.GetAxis("Horizontal_P" + m_playerIndexString), Input.GetAxis("Vertical_P" + m_playerIndexString)) * Mathf.Rad2Deg;
        Debug.Log("Fire");

        //m_animator.SetInteger("State", 3); //Throwing

        if (m_fireCooldownOn)
            return;


        //char.transform.eulerAngles = new vector3(char.transform.eulerAngles.x, Mathf.atan2(x, y) * Mathf.rad2deg, char.transform.eulerAngles.z);

        //Idle
        if (angle < 0.1)
        {

            if (m_turnedLeft)
            {
                //if(m_projectile == null )
                var projectile = Instantiate(Resources.Load(m_loadProjectile.name, typeof(GameObject)), new Vector3(transform.position.x - 1f, transform.position.y + 0.5f, transform.position.z), transform.rotation) as GameObject;
                m_currentProjectile = projectile;
                // m_projectile.SetActive(true);
                //m_projectile.transform.position = new Vector3(transform.position.x - 1f, transform.position.y + 0.5f, transform.position.z);
                //m_projectile.transform.rotation = transform.rotation;
                m_currentProjectile.transform.rotation = Quaternion.Euler(0, 0, 0);
                m_currentProjectile.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(-m_projectileForceX, m_projectileForceY, 0) * m_projectileSpeed);
                m_currentProjectile.gameObject.GetComponent<ProjectileBehaviour>().m_playerData = m_playerData;
                m_currentProjectile.GetComponent<SpriteRenderer>().flipX = false;
            }

            else
            {
                //if (m_projectile == null)
                var projectile = Instantiate(Resources.Load(m_loadProjectile.name, typeof(GameObject)), new Vector3(transform.position.x + 1f, transform.position.y + 0.5f, transform.position.z), transform.rotation) as GameObject;
                m_currentProjectile = projectile;
                //  m_projectile.SetActive(true);
                // m_projectile.transform.position = new Vector3(transform.position.x + 1f, transform.position.y + 0.5f, transform.position.z);
                //m_projectile.transform.rotation = transform.rotation;
                m_currentProjectile.transform.rotation = Quaternion.Euler(0, 0, 0);
                m_currentProjectile.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(m_projectileForceX, m_projectileForceY, 0) * m_projectileSpeed);
                m_currentProjectile.gameObject.GetComponent<ProjectileBehaviour>().m_playerData = m_playerData;

                m_currentProjectile.GetComponent<SpriteRenderer>().flipX = true;

            }
        }
        /* //Up
         if (Mathf.Clamp(angle, -10, 10) == angle && angle != -1)
         {

             var projectile = Instantiate(Resources.Load(m_projectile.name, typeof(GameObject)), new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.rotation) as GameObject;

             //projectile.transform.rotation = Quaternion.Euler(angle, 0, 0);
             projectile.transform.rotation = Quaternion.Euler(0, 0, 0);
             projectile.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(angle, m_projectileForceY, 0) * m_projectileSpeed * m_projectileForceY);
            // projectile.gameObject.GetComponent<ProjectileBehaviour>().m_playerID = m_thisPlayerIndex;
             projectile.gameObject.GetComponent<ProjectileBehaviour>().m_playerData = m_playerData;




             //Debug.Log("Up");
             //Debug.Log(angle);
         }*/

        //Right
        else if (Mathf.Clamp(angle, 10, 170) == angle)
        {
            //  if (m_projectile == null)
            var projectile = Instantiate(Resources.Load(m_loadProjectile.name, typeof(GameObject)), new Vector3(transform.position.x + 1f, transform.position.y + 0.5f, transform.position.z), transform.rotation) as GameObject;
            m_currentProjectile = projectile;
            //m_projectile.SetActive(true);
            // m_projectile.transform.position = new Vector3(transform.position.x + 1f, transform.position.y + 0.5f, transform.position.z);
            //m_projectile.transform.rotation = transform.rotation;
            m_currentProjectile.transform.rotation = Quaternion.Euler(0, 0, 0);
            m_currentProjectile.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(angle, m_projectileForceY, 0) * m_projectileSpeed);
            m_currentProjectile.gameObject.GetComponent<ProjectileBehaviour>().m_playerData = m_playerData;

            m_currentProjectile.GetComponent<SpriteRenderer>().flipX = true;

        }

        //Left
        else if (Mathf.Clamp(angle, -170, -10) == angle)
        {
            //if (m_projectile == null)
            var projectile = Instantiate(Resources.Load(m_loadProjectile.name, typeof(GameObject)), new Vector3(transform.position.x - 1f, transform.position.y + 0.5f, transform.position.z), transform.rotation) as GameObject;
            m_currentProjectile = projectile;

            // m_projectile.SetActive(true);
            //m_projectile.transform.position = new Vector3(transform.position.x - 1f, transform.position.y + 0.5f, transform.position.z);
            // m_currentProjectile.transform.rotation = transform.rotation;
            m_currentProjectile.transform.rotation = Quaternion.Euler(0, 0, 0);
            m_currentProjectile.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(angle, m_projectileForceY, 0) * m_projectileSpeed);
            m_currentProjectile.gameObject.GetComponent<ProjectileBehaviour>().m_playerData = m_playerData;
            m_currentProjectile.GetComponent<SpriteRenderer>().flipX = false;
        }

        //Down
        else if (angle == 180)
        {
            if (m_vikingcCharacterController.isGrounded)
                return;

           // if (m_currentProjectile == null)
            var projectile = Instantiate(Resources.Load(m_loadProjectile.name, typeof(GameObject)), new Vector3(transform.position.x, transform.position.y - 2f, transform.position.z), transform.rotation) as GameObject;

            m_currentProjectile = projectile;
            // m_projectile.SetActive(true);
            //m_projectile.transform.position = new Vector3(transform.position.x, transform.position.y - 2f, transform.position.z);
            //m_currentProjectile.transform.rotation = transform.rotation;
            m_currentProjectile.transform.rotation = Quaternion.Euler(0, 0, 0);
            m_currentProjectile.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(0, m_projectileForceY * -1, 0) * m_projectileSpeed);
            m_currentProjectile.gameObject.GetComponent<ProjectileBehaviour>().m_playerData = m_playerData;

        }

        m_fireCooldownOn = true;
        StartCoroutine("FireCoolDown");
        m_currentAmmo = 0;


    }


    //Called From Fixed Update
    private void RetractProjectile()
    {
        if (m_currentAmmo == 1)
            return;

        if (m_currentProjectile != null)
            m_currentProjectile.GetComponent<ProjectileBehaviour>().EnableRagdoll();

        m_currentProjectile.GetComponent<ProjectileBehaviour>().SetRetractingState(true);

        m_isRetracting = true;

        //m_animator.SetInteger("State", 4); // Catching

    }

    //Called From Fixed Update
    private void ProjectileRetractingUpdate()
    {

        m_currentProjectile.transform.position = Vector3.Lerp(m_currentProjectile.transform.position, transform.position, Time.deltaTime / m_projectileRetractTime);

        if (Mathf.Clamp(m_currentProjectile.transform.position.x, transform.position.x - 0.5f, transform.position.x + 0.5f) == m_currentProjectile.transform.position.x)
        {
            m_currentAmmo = 1;
            m_currentProjectile.SetActive(false);
            m_currentProjectile.GetComponent<ProjectileBehaviour>().SetRetractingState(false);
            m_isRetracting = false;
        }
    }

    //Called From Fixed Update
    private void VikingMovement()
    {
        Vector3 vNewInput = new Vector3(Input.GetAxis("Horizontal_P" + m_playerIndexString), Input.GetAxis("Vertical_P" + m_playerIndexString), 0.0f);
        var angle = Mathf.Atan2(Input.GetAxis("Horizontal_P" + m_playerIndexString), Input.GetAxis("Vertical_P" + m_playerIndexString)) * Mathf.Rad2Deg;

        //MovementGround
        if (m_vikingcCharacterController.isGrounded && !m_isCarried)
        {
            m_collided = false;
            m_vikingMoveDirection = new Vector3(Input.GetAxis("Horizontal_P" + m_playerIndexString), 0, 0);
            m_vikingMoveDirection = transform.TransformDirection(m_vikingMoveDirection);
            m_vikingMoveDirection *= m_vikingMovementSpeed;

            m_currentHorizontalPos = transform.position.x;
            float travelxLeft = m_currentHorizontalPos - m_previousHorizontalPos;
            float travelxRight = m_currentHorizontalPos + m_previousHorizontalPos;

            // Actually moving
            if (travelxLeft != m_previousHorizontalPos && travelxRight != m_previousHorizontalPos)
            {
                m_animator.SetInteger("State", 1); //Walking
            }
            
            if (m_currentHorizontalPos == m_previousHorizontalPos)
            {                
                m_isIdle = true;
                m_animator.SetInteger("State", 0); // Idle
            }
            //m_vikingcCharacterController.SimpleMove(m_vikingMoveDirection * m_vikingMovementSpeed);
        }
        

        if (m_vikingcCharacterController.isGrounded)
        {
            m_isGrounded = true;
            m_askForGoundedOffset = false;

			if (m_isWallJumping)
                m_isWallJumping = false;

            if (m_isJumping)
                m_isJumping = false;


        }

        if (!m_vikingcCharacterController.isGrounded)
        {
            m_askForGoundedOffset = true;

        }

        //Jumping
        if (m_isGrounded && !m_isCarried || transform.position.y == m_bottomBounds && !m_isCarried)
        {
            if (Input.GetButtonDown("Jump_P" + m_playerIndexString) && angle != 180)
            {
               
                m_vikingMoveDirection.y =  m_vikingJumpSpeed;
                if (!m_layerIsSet)
                {
                    gameObject.layer = m_goThroughPlatformLayer;
                    m_layerIsSet = true;
                }

                m_isJumping = true;

                m_animator.SetInteger("State",2); //Jumping
                // m_rb.AddForce(transform.TransformDirection(0f,1f,0) * 500);

                //  m_rb.AddForce (transform.TransformDirection(transform.position.x, transform.position.y + 1, transform.position.z) * m_vikingJumpSpeed);
                //m_jumping = true;
                // m_vikingcCharacterController.Move( new Vector3 (transform.position.x, m_vikingMoveDirection.y * m_vikingMovementSpeed * Time.deltaTime));
            }

        }

        //Press Down From Platform
        if (m_vikingcCharacterController.isGrounded && !m_isCarried)
        {
            if (Input.GetButtonDown("Jump_P" + m_playerIndexString) && angle == 180)
            {
               
                if (!m_layerIsSet)
                {
                    Debug.Log("go");
                    gameObject.layer = m_goThroughPlatformLayer;
                    m_layerIsSet = true;
                }

                m_goThroughPlatformDown = true;
                m_animator.SetInteger("State", 0); //Idle
                
            }

        }

        //Movement Air
        if (!m_vikingcCharacterController.isGrounded && !m_isCarried)
        {
            m_vikingMoveDirection.x = transform.TransformDirection(vNewInput).x * m_vikingMovementSpeed;
            m_animator.SetInteger("State", 3); //Falling
        }


        //Wall Jump
        if (Input.GetButtonDown("Jump_P" + m_playerIndexString))
        {
            if (!m_vikingcCharacterController.isGrounded && !m_isCarried && !m_isWallJumping)
            {
                {
                    if (transform.position.x == m_leftBounds || transform.position.x == m_rightBounds)
                    {
                        m_vikingMoveDirection.y = m_vikingJumpSpeed - 2f;

                        if (!m_layerIsSet)
                        {
                            gameObject.layer = m_goThroughPlatformLayer;
                            m_layerIsSet = true;
                        }

                        m_isJumping = true;
                        m_isWallJumping = true;
                    }
                }
            }
        }


        //Release Wall Cling
        if (Input.GetButtonUp("Jump_P" + m_playerIndexString) && m_isWallClinging)
        {
            if (transform.position.x == m_leftBounds || transform.position.x == m_rightBounds)
            {
                m_isWallClinging = false;
            }
        }

        //Right - Sprite Flip
        if (Mathf.Clamp(angle, 10, 170) == angle)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_turnedLeft = false;
            m_animator.SetBool("facingRight", true); //Facing Right
        }

        //Left - Sprite Flip
        else if (Mathf.Clamp(angle, -170, -10) == angle)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_turnedLeft = true;
            m_animator.SetBool("facingRight", false); //Facing Left
        }

        // m_hand.transform.Translate(new Vector3 (angle, 0, 0) * 10f * Time.deltaTime);
        //m_hand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (!m_isWallClinging)
        {
            m_vikingMoveDirection.y -= m_vikingGravityForce * Time.deltaTime;
            m_vikingcCharacterController.Move(m_vikingMoveDirection * Time.deltaTime);
        }
    }

    private void CheckWallClinging ()
    {
        //Wall Cling
        if (Input.GetButton("Jump_P" + m_playerIndexString) )
        {
            if (!m_vikingcCharacterController.isGrounded && !m_isCarried)
            {
                if (transform.position.x == m_leftBounds || transform.position.x == m_rightBounds)
                {
                    m_isWallClinging = true;
                }
            }
        } 
    }

   
    IEnumerator FireCoolDown()
    {
        yield return new WaitForSeconds(m_rapidFireSpeed);
        m_fireCooldownOn = false;
    }

    public void SetStunned()
    {
        m_isStunned = true;
        //m_animator.SetInteger("State", 6); // Stunned
        GetComponent<SpriteRenderer>().color = Color.white;
        Invoke("StunnedCooldown", m_stunnedCoolDown);
    }

    private void StunnedCooldown()
    {
        m_animator.SetInteger("State", 1); // Idle, not stunned
        m_isStunned = false;
        GetComponent<SpriteRenderer>().color = Color.clear;
    }
}
