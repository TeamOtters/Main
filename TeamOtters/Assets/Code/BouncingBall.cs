using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BouncingBall : MonoBehaviour
{

    public Rigidbody rb;

    //Making the ball bounce diagonally
    public float m_XBounceSpeed = 20f;
    public float m_YBounceSpeed = 20f;

    //Health on BouncingBall enemy
    public float m_startHealth = 100;
    private float m_health;

    public GameController m_gameController;

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
        m_health = m_startHealth;

        m_boundaryHolder = GameController.Instance.boundaryHolder;
        m_ballSize = GetComponent<BouncingBall>().GetComponent<SpriteRenderer>().bounds.extents;

        Debug.Log(GetComponent<BouncingBall>().GetComponent<SpriteRenderer>().bounds.extents);

        int directionX = Random.Range(-1, 2);
        int directionY = Random.Range(-1, 2);

        if (directionX == 0)
        {
            directionX = 1;
        }

        rb.velocity = new Vector2(0f, 0f);
        rb.velocity = new Vector2(6f * directionX, 6f * directionY);


        //StartCoroutine(Pause());
    }

    void Update()
    {
        //Debug.Log(m_boundaryHolder.gameObject.name);

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
            Debug.Log("Hitting left");
            rb.AddForce(m_XBounceSpeed,  m_YBounceSpeed, 0 * Time.deltaTime);
        }
        if (transform.position.x > m_rightBounds)
        {
          //  Debug.Log("My position is: " + transform.position.x + "which is greater than my Right boundary value: " + m_rightBounds);
            transform.position = new Vector3(m_rightBounds, transform.position.y, transform.position.z);
            Debug.Log("Hitting right");
            rb.AddForce(-m_XBounceSpeed, 0, 0 * Time.deltaTime);
        }
        if (transform.position.y < m_bottomBounds)
        {
            //Debug.Log("My position is: " + transform.position.y + "which is less than my Down boundary value: " + m_bottomBounds);
            transform.position = new Vector3(transform.position.x, m_bottomBounds, transform.position.z);
            Debug.Log("Hitting bottom");
           rb.AddForce(Random.Range(-m_XBounceSpeed, m_XBounceSpeed), m_YBounceSpeed, 0 * Time.deltaTime);
        }
        if (transform.position.y > m_topBounds)
        {
            //Debug.Log("My position is: " + transform.position.y + "which is greater than my Up boundary value: " + m_topBounds);
            transform.position = new Vector3(transform.position.x, m_topBounds, transform.position.z);
            Debug.Log("Hitting top");
            rb.AddForce(m_XBounceSpeed, Random.Range(-m_YBounceSpeed, m_YBounceSpeed), 0 * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
     {

        rb.AddForce(m_XBounceSpeed, m_YBounceSpeed, 0 * Time.deltaTime);

        /*if (collisionInfo.gameObject.CompareTag("Projectile"))
        {
            Debug.Log(collisionInfo.gameObject.tag);
            rb.AddForce(0, 200, 0 * Time.deltaTime);

            Collects the players index number. 
            int hittingPlayer = collisionInfo.gameObject.GetComponent<PlayerData>().m_PlayerIndex;

            Adds 10 points to the player when hitting ball. 
            collisionInfo.gameObject.GetComponent<PlayerData>().m_CurrentScore += m_bouncePoints;

        }*/

        //Stun player. Calls temporary Stunned script.
        if (collisionInfo.gameObject.CompareTag("Player") || collisionInfo.gameObject.CompareTag("Viking"))
        {
            collisionInfo.gameObject.GetComponent<VikingController>().SetStunned();
        }

        else
        {
            Debug.Log("didn't hit but hit "+ collisionInfo.gameObject.tag);
        }
       }

    public void TakeDamage(float amount)
    {
        m_health -= amount;

        m_healthBar.fillAmount = m_health / m_startHealth;

        if (m_health <= 0)
        {
           gameObject.SetActive (false);
        }
    }

    IEnumerator Pause()
    {

        //Sends the ball off in a random direction
        int directionX = Random.Range(-1, 2);
        int directionY = Random.Range(-1, 2);

        if (directionX ==0)
        {
            directionX = 1;
        }

        rb.velocity = new Vector2(0f, 0f);

        //Makes ball oause for 1 sec before starting to move.
        yield return new WaitForSeconds(1);
        rb.velocity = new Vector2(6f * directionX, 6f * directionY);
    }
}
