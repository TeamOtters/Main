using UnityEngine;
using UnityEngine.UI;

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

    private int m_bouncePoints;
    private int m_goalPoints;
    private int m_opponentPoints;

    //Image used for decreasing healthbar. Might not be used in final game.
    public Image m_healthBar;

    void Start()
    {
        m_bouncePoints = m_gameController.bounceHit;
        m_goalPoints = m_gameController.firstReachGoal;
        m_opponentPoints = m_gameController.hitOpponent;

        rb.velocity = new Vector2(m_XBounceSpeed, m_YBounceSpeed);
        m_health = m_startHealth;
    }

    void OnCollisionEnter(Collision collisionInfo)
     {
        if (collisionInfo.gameObject.CompareTag("Axe"))
        {
            Debug.Log(collisionInfo.gameObject.tag);
            rb.AddForce(0, 200, 0 * Time.deltaTime);

            //Collects the players index number. 
           // int hittingPlayer = collisionInfo.gameObject.GetComponent<PlayerData>().m_PlayerIndex;

            //Adds 10 points to the player when hitting ball. 
          //  collisionInfo.gameObject.GetComponent<PlayerData>().m_CurrentScore += m_bouncePoints;

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
}
