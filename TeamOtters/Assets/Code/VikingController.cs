using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingController : MonoBehaviour {

    private float m_vikingSpeedForce;
    private CharacterController m_vikingcCharacterController;
    private VikingProjectiles m_vikingProjectiles;
    public GameObject m_projectile;

    public float m_vikingMovementSpeed = 6.0F;
    public float m_vikingJumpSpeed = 8.0F;
    public float m_vikingGravityForce = 20.0F;
    private Vector3 m_vikingMoveDirection = Vector3.zero;

    public float m_projectileForceX;
    public float m_projectileForceY;
    public float m_projectileSpeed;


    private float m_rapidFireSpeed = 0.2f;
    private bool m_fireCooldownOn;
    private int m_playerIndex;


    // Use this for initialization
    void Start () {

        m_vikingcCharacterController = GetComponent<CharacterController>();
        m_playerIndex = transform.parent.GetComponent<PlayerData>().m_PlayerIndex;
    }
	
	// Update is called once per frame
	void Update () {


        if (Input.GetButtonDown("Fire1_P" + m_playerIndex))
        {
            if (m_fireCooldownOn)
                return;

               var projectile = Instantiate(Resources.Load(m_projectile.name, typeof(GameObject)), new Vector3 (transform.position.x + 0.4f, transform.position.y +0.4f, transform.position.z), transform.rotation) as GameObject;

            float angle = Mathf.Atan2(Input.GetAxis("Horizontal_P" + m_playerIndex), Input.GetAxis("Vertical_P" + m_playerIndex));
            //char.transform.eulerAngles = new vector3(char.transform.eulerAngles.x, Mathf.atan2(x, y) * Mathf.rad2deg, char.transform.eulerAngles.z);
            projectile.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            projectile.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(m_projectileForceX, m_projectileForceY, 0) * m_projectileSpeed);
            




            Debug.Log(angle);



            StartCoroutine("DestroyProjectileTimer",projectile);

            m_fireCooldownOn = true;
            StartCoroutine("FireCoolDown");

        } 


      

        if (m_vikingcCharacterController.isGrounded)
        {
            m_vikingMoveDirection = new Vector3(Input.GetAxis("Horizontal_P" + m_playerIndex), 0,0);
            m_vikingMoveDirection = transform.TransformDirection(m_vikingMoveDirection);
            m_vikingMoveDirection *= m_vikingMovementSpeed;

            if (Input.GetButtonDown("Jump_P" + m_playerIndex))
                m_vikingMoveDirection.y = m_vikingJumpSpeed;

        }
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
        Destroy(_objectToDestroy);
    }
}
