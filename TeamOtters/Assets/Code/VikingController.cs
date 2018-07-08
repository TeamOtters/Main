using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingController : MonoBehaviour {

    private float m_vikingSpeedForce;
    private CharacterController m_vikingcCharacterController;
    private VikingProjectiles m_vikingProjectiles;
    private PlayerData m_playerData;
    public GameObject m_projectile;
    public GameObject m_hand;

    public float m_vikingMovementSpeed = 6.0F;
    public float m_vikingJumpSpeed = 8.0F;
    public float m_vikingGravityForce = 20.0F;
    private Vector3 m_vikingMoveDirection = Vector3.zero;

    public float m_projectileForceX;
    public float m_projectileForceY;
    public float m_projectileSpeed;

    private float m_rapidFireSpeed = 0.2f;
    private bool m_fireCooldownOn;
    public bool m_isStunned;

    public float m_stunnedCoolDown = 1f;

    private int m_thisPlayerIndex;


    // Use this for initialization
    void Start () {

        m_vikingcCharacterController = GetComponent<CharacterController>();
        m_playerData = GetComponentInParent<PlayerData>();
        if(m_playerData == null)
         m_playerData = GetComponent<PlayerData>();

        if (m_playerData != null)
            m_thisPlayerIndex = m_playerData.myPlayerIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isStunned)
        {
            VikingFire();
            VikingMovement();
        }

        // Only do work if meaningful
        /*  if (vNewInput.sqrMagnitude < 0.1f)
          {
              return;
          }*/
  
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Projectile"))
        {
            if(collision.gameObject.GetComponent<ProjectileBehaviour>().m_playerID != m_thisPlayerIndex)
             SetStunned();

            
        }
    }

    private void VikingFire ()
    {

        Vector3 vNewInput = new Vector3(Input.GetAxis("Horizontal_P1"), Input.GetAxis("Vertical_P1"), 0.0f);
        var angle = Mathf.Atan2(Input.GetAxis("Horizontal_P1"), Input.GetAxis("Vertical_P1")) * Mathf.Rad2Deg;
        //  Debug.Log(angle);

        //Fire Projectile
        if (Input.GetButtonDown("Fire1_P1"))
        {
            if (m_fireCooldownOn)
                return;

            //char.transform.eulerAngles = new vector3(char.transform.eulerAngles.x, Mathf.atan2(x, y) * Mathf.rad2deg, char.transform.eulerAngles.z);


            //Up
            if (Mathf.Clamp(angle, -10, 10) == angle)
            {
                
                var projectile = Instantiate(Resources.Load(m_projectile.name, typeof(GameObject)), new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.rotation) as GameObject;



                //projectile.transform.rotation = Quaternion.Euler(angle, 0, 0);
                projectile.transform.rotation = Quaternion.Euler(0, 0, 0);
                projectile.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(angle, m_projectileForceY, 0) * m_projectileSpeed * m_projectileForceY);
                projectile.gameObject.GetComponent<ProjectileBehaviour>().m_playerID = m_thisPlayerIndex;

               
                StartCoroutine("DestroyProjectileTimer", projectile);

                //Debug.Log("Up");
                //Debug.Log(angle);
            }

            //Right
            else if (Mathf.Clamp(angle, 10, 170) == angle)
            {
                var projectile = Instantiate(Resources.Load(m_projectile.name, typeof(GameObject)), new Vector3(transform.position.x + 1f, transform.position.y + 0.5f, transform.position.z), transform.rotation) as GameObject;
                //projectile.transform.rotation = Quaternion.Euler(angle, 0, 0);
                projectile.transform.rotation = Quaternion.Euler(0, 0, 0);
                projectile.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(angle, m_projectileForceY, 0) * m_projectileSpeed);
                projectile.gameObject.GetComponent<ProjectileBehaviour>().m_playerID = m_thisPlayerIndex;

                projectile.GetComponent<SpriteRenderer>().flipX = true;
                
                StartCoroutine("DestroyProjectileTimer", projectile);

                // Debug.Log("Right");
                // Debug.Log(angle);
            }

            //Left
            else if (Mathf.Clamp(angle, -170, -10) == angle)
            {
                var projectile = Instantiate(Resources.Load(m_projectile.name, typeof(GameObject)), new Vector3(transform.position.x - 1f, transform.position.y + 0.5f, transform.position.z), transform.rotation) as GameObject;
                //projectile.transform.rotation = Quaternion.Euler(angle, 0, 0);
                projectile.transform.rotation = Quaternion.Euler(0, 0, 0);
                projectile.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(angle, m_projectileForceY, 0) * m_projectileSpeed);
                projectile.gameObject.GetComponent<ProjectileBehaviour>().m_playerID = m_thisPlayerIndex;
                projectile.GetComponent<SpriteRenderer>().flipX = false;

               
                // Debug.Log("Left");
                // Debug.Log(angle);

                StartCoroutine("DestroyProjectileTimer", projectile);
            }

            //Down
            else if (angle == 180)
            {
                if (m_vikingcCharacterController.isGrounded)
                    return;

                var projectile = Instantiate(Resources.Load(m_projectile.name, typeof(GameObject)), new Vector3(transform.position.x, transform.position.y - 2f, transform.position.z), transform.rotation) as GameObject;
                // projectile.transform.rotation = Quaternion.Euler(angle, 0, 0);
                projectile.transform.rotation = Quaternion.Euler(0, 0, 0);
                projectile.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(0, m_projectileForceY * -1, 0) * m_projectileSpeed * m_projectileForceY);
                projectile.gameObject.GetComponent<ProjectileBehaviour>().m_playerID = m_thisPlayerIndex;
                
                //  Debug.Log("Down");
                // Debug.Log(angle);

                StartCoroutine("DestroyProjectileTimer", projectile);
            }

            m_fireCooldownOn = true;
            StartCoroutine("FireCoolDown");

        }
    }

    private void VikingMovement ()
    {
        if (m_vikingcCharacterController.isGrounded)
        {
            m_vikingMoveDirection = new Vector3(Input.GetAxis("Horizontal_P1"), 0, 0);
            m_vikingMoveDirection = transform.TransformDirection(m_vikingMoveDirection);
            m_vikingMoveDirection *= m_vikingMovementSpeed;

            if (Input.GetButtonDown("Jump_P1"))
                m_vikingMoveDirection.y = m_vikingJumpSpeed;

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

    IEnumerator DestroyProjectileTimer(GameObject _objectToDestroy)
    {
        yield return new WaitForSeconds(5f);
        DestroyProjectile(_objectToDestroy);
    }

    private void DestroyProjectile(GameObject _objectToDestroy)
    {
        if(_objectToDestroy != null)
         Destroy(_objectToDestroy);
    }

    public void SetStunned ()
    {
        m_isStunned = true;
        Invoke("StunnedCooldown", m_stunnedCoolDown);
        Debug.Log("STUNNED");
    }

    private void StunnedCooldown ()
    {
        Debug.Log("NotStunned!");
        m_isStunned = false;
    }
}
