using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingController : MonoBehaviour {

    private float m_vikingSpeedForce;
    private CharacterController m_vikingcCharacterController;
    private VikingProjectiles m_vikingProjectiles;
    private PlayerData m_playerData;
    public GameObject m_loadProjectile;
    private GameObject m_currentProjectile;
    public GameObject m_hand;

    public float m_vikingMovementSpeed = 6.0F;
    public float m_vikingJumpSpeed = 8.0F;
    public float m_vikingGravityForce = 20.0F;
    private Vector3 m_vikingMoveDirection = Vector3.zero;

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
    private bool m_collided;
    private bool m_turnedLeft;
    private bool m_isRetracting;

    public float m_stunnedCoolDown = 1f;

    private int m_thisPlayerIndex;
    private string m_playerIndexString;


    // Use this for initialization
    void Start () {

        m_vikingcCharacterController = GetComponent<CharacterController>();
        m_playerData = GetComponentInParent<PlayerData>();

        if (m_playerData != null)
            m_thisPlayerIndex = m_playerData.m_PlayerIndex;

        m_playerIndexString = m_thisPlayerIndex.ToString();
       
    }

    // Update is called once per frame
    void Update()
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

        if(m_isRetracting)
            ProjectileRetractingUpdate();

      

        // Only do work if meaningful
        /*  if (vNewInput.sqrMagnitude < 0.1f)
          {
              return;
          }*/
  
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
            var magnitude = 3;
            // calculate force vector
            //var force = transform.position - hit.transform.position;
            // normalize force vector to get direction only and trim magnitude
            //force.Normalize();
            //gameObject.GetComponent<Rigidbody>().AddForce(force * magnitude);
            m_collided = true;

            m_vikingMoveDirection.y -= m_vikingGravityForce * magnitude *  Time.deltaTime;
            m_vikingcCharacterController.Move(m_vikingMoveDirection * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Projectile"))
        {
            if(collision.gameObject.GetComponent<ProjectileBehaviour>().m_playerID != m_thisPlayerIndex)
             SetStunned();

            if (collision.gameObject.GetComponent<ProjectileBehaviour>().m_hit == true)
            {
                //m_currentAmmo = 1;
                //Destroy(collision.gameObject);
                //m_currentProjectile = null;
            }
        }

        if(collision.collider.CompareTag("Viking"))
        {
           Physics.IgnoreCollision(GetComponent<Collider>(),collision.collider, true);
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

    public void SetCarried (bool enable)
    {
        m_isCarried = enable;
        if(m_isCarried)
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

    private void VikingFire ()
    {
        Vector3 vNewInput = new Vector3(Input.GetAxis("Horizontal_P" + m_playerIndexString), Input.GetAxis("Vertical_P" + m_playerIndexString), 0.0f);
        var angle = Mathf.Atan2(Input.GetAxis("Horizontal_P" + m_playerIndexString), Input.GetAxis("Vertical_P" + m_playerIndexString)) * Mathf.Rad2Deg;
        Debug.Log("Fire");
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
                m_currentProjectile.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(-m_projectileForceX, m_projectileForceY, 0) * m_projectileSpeed );
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
                m_currentProjectile.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(m_projectileForceX, m_projectileForceY, 0) * m_projectileSpeed );
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

            //if (m_projectile == null)
            var projectile = Instantiate(Resources.Load(m_loadProjectile.name, typeof(GameObject)), new Vector3(transform.position.x, transform.position.y - 2f, transform.position.z), transform.rotation) as GameObject;
            m_currentProjectile = projectile;
            // m_projectile.SetActive(true);
            //m_projectile.transform.position = new Vector3(transform.position.x, transform.position.y - 2f, transform.position.z);
            //m_currentProjectile.transform.rotation = transform.rotation;
            m_currentProjectile.transform.rotation = Quaternion.Euler(0, 0, 0);
            m_currentProjectile.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(0, m_projectileForceY * -1, 0) * m_projectileSpeed );
            m_currentProjectile.gameObject.GetComponent<ProjectileBehaviour>().m_playerData = m_playerData;

            }

            m_fireCooldownOn = true;
            StartCoroutine("FireCoolDown");
            m_currentAmmo = 0;


    }


    private void RetractProjectile ()
    {
        if (m_currentAmmo == 1)
            return;

        if (m_currentProjectile != null)
            m_currentProjectile.GetComponent<ProjectileBehaviour>().EnableRagdoll();

        m_currentProjectile.GetComponent<ProjectileBehaviour>().SetRetractingState(true);

        m_isRetracting = true;

    }

    private void ProjectileRetractingUpdate ()
    {
        
        m_currentProjectile.transform.position = Vector3.Lerp(m_currentProjectile.transform.position, transform.position, Time.deltaTime / m_projectileRetractTime);
        // m_currentProjectile.transform.position = transform.position;



        //ector3 move = new Vector3(0, target.y * m_panSpeed * Time.deltaTime, 0);

        //transform.Translate(move, Space.World);

        if (Mathf.Clamp(m_currentProjectile.transform.position.x, transform.position.x - 0.5f, transform.position.x + 0.5f) == m_currentProjectile.transform.position.x)
        {
            m_currentAmmo = 1;
            m_currentProjectile.SetActive(false);
            m_currentProjectile.GetComponent<ProjectileBehaviour>().SetRetractingState(false);
            m_isRetracting = false;
        }
    }

    private void VikingMovement ()
    {
        Vector3 vNewInput = new Vector3(Input.GetAxis("Horizontal_P" + m_playerIndexString), Input.GetAxis("Vertical_P" + m_playerIndexString), 0.0f);
        var angle = Mathf.Atan2(Input.GetAxis("Horizontal_P" + m_playerIndexString), Input.GetAxis("Vertical_P" + m_playerIndexString)) * Mathf.Rad2Deg;

        if (m_vikingcCharacterController.isGrounded && !m_isCarried)
        {
            m_collided = false;
            m_vikingMoveDirection = new Vector3(Input.GetAxis("Horizontal_P" + m_playerIndexString), 0, 0);
            m_vikingMoveDirection = transform.TransformDirection(m_vikingMoveDirection);
            m_vikingMoveDirection *= m_vikingMovementSpeed;

            if (Input.GetButtonDown("Jump_P" + m_playerIndexString))
                m_vikingMoveDirection.y = m_vikingJumpSpeed;

        }

        //Right
        if (Mathf.Clamp(angle, 10, 170) == angle)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_turnedLeft = false;
        }

        //Left
        else if (Mathf.Clamp(angle, -170, -10) == angle)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_turnedLeft = true;
        }

            // m_hand.transform.Translate(new Vector3 (angle, 0, 0) * 10f * Time.deltaTime);
            //m_hand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            m_vikingMoveDirection.y -= m_vikingGravityForce * Time.deltaTime;
        m_vikingcCharacterController.Move(m_vikingMoveDirection * Time.deltaTime);
    }


    IEnumerator FireCoolDown ()
    {
        yield return new WaitForSeconds(m_rapidFireSpeed);
        m_fireCooldownOn = false;
    }

    

    public void SetStunned ()
    {
        m_isStunned = true;
        GetComponent<SpriteRenderer>().color = Color.white;
        Invoke("StunnedCooldown", m_stunnedCoolDown);
        Debug.Log("STUNNED");
    }

    private void StunnedCooldown ()
    {
        Debug.Log("NotStunned!");
        m_isStunned = false;
        GetComponent<SpriteRenderer>().color = Color.clear;
    }
}
