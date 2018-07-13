using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BouncingBall : MonoBehaviour
{

    //Health on BouncingBall enemy
    public float m_startHealth = 100;
    internal float m_currentHealth;
    internal bool m_isAlive = true;

    // Sets the different sprites and particle effects for the different stats
    public GameObject m_BallSprite_Full;
    public GameObject m_BallSprite_Half;
    public GameObject m_BallSprite_ThreeLeft;
    public GameObject m_hpBelowHalf;
    public GameObject m_hpThreeHitLeft;
    public GameObject m_ballDeathEffect;
    public bool m_hasHalfHPEffect = false;
    public bool m_has3HitLeftEffect = false;
    int m_axeDamage;

    //Setting ball speed
    public Rigidbody rb;
    public float m_XBounceSpeed = 200f;
    public float m_YBounceSpeed = 200f;

    
    //Makes sure ball don't go outside of screen
    private BoundaryHolder m_boundaryHolder;
    private float m_leftBounds;
    private float m_rightBounds;
    private float m_topBounds;
    private float m_bottomBounds;

    private Vector3 m_ballSize;

    //Image used for decreasing healthbar. Might not be used in final game.
    public Image m_healthBar;

    void Start()
    {
        m_currentHealth = m_startHealth;
        m_hpBelowHalf.SetActive(false);
        m_hpThreeHitLeft.SetActive(false);
        m_BallSprite_Full.SetActive(true);
        m_BallSprite_Half.SetActive(false);
        m_BallSprite_ThreeLeft.SetActive(false);

    //Calculation for making the ball stay within camera view. 
        m_boundaryHolder = GameController.Instance.boundaryHolder;
        m_ballSize = transform.Find("BallHP_Full").GetComponent<SpriteRenderer>().bounds.extents;
        //;GetComponent<BouncingBall>().GetComponent<SpriteRenderer>().bounds.extents;
        Debug.Log(m_ballSize);
        m_axeDamage = 10;
        //(GetComponent<ProjectileBehaviour>().m_projectileDamage);

        Vector3 m_currentVelocity = rb.velocity;

        //rb.AddForce(m_XBounceSpeed, m_YBounceSpeed, 0 * Time.deltaTime);

        //StartCoroutine(Pause());
    }
    private void FixedUpdate()
    {

        //rb.AddForce(Transform.rb.localRotation * m_XBounceSpeed);
        //rb.velocity = 5 * (rb.velocity.normalized);

        m_bottomBounds = m_boundaryHolder.ballBoundary.Down + m_ballSize.y;
        m_topBounds = m_boundaryHolder.ballBoundary.Up - m_ballSize.y;
        m_leftBounds = m_boundaryHolder.ballBoundary.Left + m_ballSize.x;
        m_rightBounds = m_boundaryHolder.ballBoundary.Right - m_ballSize.x;

        /*Debug.Log("BALL Boundary Bottom: " + m_bottomBounds + " should be Down. The boundaryHolder's value for down is: " + m_boundaryHolder.ballBoundary.Down);
        Debug.Log(" BALL Boundary Top: " + m_topBounds + " should be Up. The boundaryHolder's value for up is: " + m_boundaryHolder.ballBoundary.Up);
        Debug.Log("BALL Boundary Left: " + m_leftBounds + " should be Left. The boundaryHolder's value for left is: " + m_boundaryHolder.ballBoundary.Left);
        Debug.Log("BALL Boundary Right: " + m_rightBounds + " should be Right. The boundaryHolder's value for right is: " + m_boundaryHolder.ballBoundary.Right);
        */

        // Clamp movement
        if (transform.position.x < m_leftBounds)
        {
            //Debug.Log("My position is: " + transform.position.x + "which is less than my Left boundary value: " + m_leftBounds);
            transform.position = new Vector3(m_leftBounds, transform.position.y, transform.position.z);
           // Debug.Log("Hitting left");
            //rb.AddForce((m_XBounceSpeed*2), 0, 0 * Time.deltaTime);

        }
        if (transform.position.x > m_rightBounds)
        {
            //  Debug.Log("My position is: " + transform.position.x + "which is greater than my Right boundary value: " + m_rightBounds);
            transform.position = new Vector3(m_rightBounds, transform.position.y, transform.position.z);
            //Debug.Log("Hitting right");
            //rb.AddForce((-m_XBounceSpeed*2),0, 0 * Time.deltaTime);

        }
        if (transform.position.y < m_bottomBounds)
        {
            //Debug.Log("My position is: " + transform.position.y + "which is less than my Down boundary value: " + m_bottomBounds);
            transform.position = new Vector3(transform.position.x, m_bottomBounds, transform.position.z);
            //Debug.Log("Hitting bottom");
            //rb.AddForce(0,(m_YBounceSpeed*2), 0 * Time.deltaTime);

        }
        if (transform.position.y > m_topBounds)
        {
            //Debug.Log("My position is: " + transform.position.y + "which is greater than my Up boundary value: " + m_topBounds);
            transform.position = new Vector3(transform.position.x, m_topBounds, transform.position.z);
            //Debug.Log("Hitting top");
            //rb.AddForce(0,m_YBounceSpeed, 0 * Time.deltaTime);

        }

        if (rb.velocity.x > 15 || rb.velocity.y > 15)
        {
            Debug.Log("CHANGED VELOCITY");
            rb.velocity = new Vector3(5, 5, 0);

        }

        //Sets the second state when the ball has half its HP left
        if (m_currentHealth <= m_startHealth/2 && m_currentHealth > m_axeDamage *3 && m_hasHalfHPEffect == false)
        {
            Debug.Log("New state: The ball has " + m_currentHealth + " health left! and axedamage = "+ m_axeDamage*3);
            GetComponent<BouncingBall>().m_BallSprite_Full.SetActive(false);
            GetComponent<BouncingBall>().m_BallSprite_Half.SetActive(true);
            GetComponent<BouncingBall>().m_hpBelowHalf.SetActive(true);
            m_hasHalfHPEffect = true;
        }
        //Sets the third state when the ball can only take 3 more hits
        if (m_currentHealth < m_startHealth/2 && m_currentHealth <= m_axeDamage * 3 && m_has3HitLeftEffect == false)
        {
            Debug.Log("New state: The ball can only take 3 more hits!!");
            GetComponent<BouncingBall>().m_hpBelowHalf.SetActive(false);
            GetComponent<BouncingBall>().m_BallSprite_Half.SetActive(false);
            GetComponent<BouncingBall>().m_BallSprite_ThreeLeft.SetActive(true);
            GetComponent<BouncingBall>().m_hpThreeHitLeft.SetActive(true);

            m_hasHalfHPEffect = false;
            m_has3HitLeftEffect = true;
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
     {
        Debug.Log("This is collider: " + collisionInfo.gameObject.name);

        //Stun players touching bouncingBall.
        if (collisionInfo.gameObject.CompareTag("Player") || collisionInfo.gameObject.CompareTag("Viking"))
        {
            collisionInfo.gameObject.GetComponent<VikingController>().SetStunned(collisionInfo.gameObject.GetComponent<VikingController>().m_stunnedCoolDown);
        }

        else
        {
            Debug.Log("Hit " + collisionInfo.gameObject.tag + " Collision");
        }
       }


    /*if (collisionInfo.gameObject.CompareTag("Projectile"))
    {
        Debug.Log(collisionInfo.gameObject.tag);
        rb.AddForce(0, 200, 0 * Time.deltaTime);

        Collects the players index number. 
        int hittingPlayer = collisionInfo.gameObject.GetComponent<PlayerData>().m_PlayerIndex;

        Adds 10 points to the player when hitting ball. 
        collisionInfo.gameObject.GetComponent<PlayerData>().m_CurrentScore += m_bouncePoints;

    }*/

    public void TakeDamage(float amount)
    {
        m_currentHealth -= amount;

        m_healthBar.fillAmount = m_currentHealth / m_startHealth;

        if (m_currentHealth <= 0)
        {
            IsDead();
        }
    }

    IEnumerator Pause()
    {
        //Makes ball pause for 1 sec before starting to move.
        yield return new WaitForSeconds(1);
    }

    public void IsDead()
    {
        m_isAlive = false;
        GameObject effect = (GameObject)Instantiate(m_ballDeathEffect, transform.position, Quaternion.identity);
        m_hpThreeHitLeft.SetActive(false);
        m_has3HitLeftEffect = false;
        gameObject.SetActive(false);
        Destroy(effect, 5f);
    }

    public void Respawn()
    {
        //sets the Alive bool back to read as alive by managers
        m_isAlive = true;

        //respawns at the center of screen - this may be an issue later where it gets stuck. If we go in this direction we should set up each level "stage" individually with a unique new spawn point for the ball.
        gameObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        Debug.Log("Ball respawning at position" + (gameObject.transform.position));

        //resets health and active values 
        m_currentHealth = m_startHealth;
        m_healthBar.fillAmount = m_startHealth;
        gameObject.SetActive(true);
    }
}
