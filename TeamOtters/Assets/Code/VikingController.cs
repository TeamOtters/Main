using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingController : MonoBehaviour {

    private float m_vikingSpeedForce;
    private CharacterController m_vikingcCharacterController;
    private VikingProjectiles m_vikingProjectiles;
    public GameObject m_projectile;

    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    public float forceX;
    public float forceY;
    public float projectileSpeed;


    private float m_rapidFireSpeed = 0.2f;
    private bool m_fireCooldownOn;

    private


    // Use this for initialization
    void Start () {

        m_vikingcCharacterController = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {


        if (Input.GetButtonDown("Fire1_P1"))
        {
            if (m_fireCooldownOn)
                return;
            //if (m_projectile != null)
            //{
               var projectile = Instantiate(Resources.Load(m_projectile.name, typeof(GameObject)), new Vector3 (transform.position.x + 0.4f, transform.position.y +0.4f, transform.position.z), transform.rotation) as GameObject;

           // Debug.Log(m_vikingProjectiles.m_projectile.name);
               projectile.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(forceX, forceY, 0) * projectileSpeed);
           // StartCoroutine("DestroyProjectileTimer",(projectile));

            m_fireCooldownOn = true;
            StartCoroutine("FireCoolDown");
            //}


           
 
        }


      

        if (m_vikingcCharacterController.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal_P1"), 0,0);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButtonDown("Jump_P1"))
                moveDirection.y = jumpSpeed;

        }
        moveDirection.y -= gravity * Time.deltaTime;
        m_vikingcCharacterController.Move(moveDirection * Time.deltaTime);

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
        Destroy(this);
    }
}
