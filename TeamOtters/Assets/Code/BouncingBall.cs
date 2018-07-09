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

    //get right score for hitting ball
    private int m_bouncePoints;

    //Image used for decreasing healthbar. Might not be used in final game.
    public Image m_healthBar;

    void Start()
    {
        m_bouncePoints = m_gameController.bounceHit;

        m_health = m_startHealth;

        //StartCoroutine(Pause());
    }

    void OnCollisionEnter(Collision collisionInfo)
     {

        rb.AddForce(m_XBounceSpeed, m_YBounceSpeed, 0 * Time.deltaTime);

        /*if (collisionInfo.gameObject.CompareTag("Projectile"))
        {
            Debug.Log(collisionInfo.gameObject.tag);
            //rb.AddForce(0, 200, 0 * Time.deltaTime);

            //Collects the players index number. 
           // int hittingPlayer = collisionInfo.gameObject.GetComponent<PlayerData>().m_PlayerIndex;

            //Adds 10 points to the player when hitting ball. 
          //  collisionInfo.gameObject.GetComponent<PlayerData>().m_CurrentScore += m_bouncePoints;

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
