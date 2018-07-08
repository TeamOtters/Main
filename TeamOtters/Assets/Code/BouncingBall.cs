using UnityEngine;
using UnityEngine.UI;

public class BouncingBall : MonoBehaviour {

    public Rigidbody rb;

    //Making the ball bounce diagonally
    public float m_XBounceSpeed = 20f;
    public float m_YBounceSpeed = 20f;

    //Health on BouncingBall enemy
    public float m_startHealth = 100;
    private float m_health;

    //Image used for decreasing heltbar. Might not be used in final game.
    public Image m_healthBar;

    void Start()
    {
        rb.velocity = new Vector2(m_XBounceSpeed, m_YBounceSpeed);
        m_health = m_startHealth;
    }

    void OnCollisionEnter(Collision collisionInfo)
     {
     if (collisionInfo.gameObject.CompareTag ("Player"))
       {
            Debug.Log("Ball took damage");
            rb.AddForce(0, 200, 0 * Time.deltaTime);

            //Collects the players index number. 
            int hittingPlayer = collisionInfo.gameObject.GetComponent<PlayerData>().m_PlayerIndex;

            //Adds 10 points to the player when hitting ball. 
            collisionInfo.gameObject.GetComponent<PlayerData>().m_CurrentScore +=10;
            
        }
     }

    public void TakeDamage(float amount)
    {
        m_health -= amount;

        m_healthBar.fillAmount = m_health / m_startHealth;

        if (m_health <= 0)
        {
           Destroy(gameObject);
        }
    }
}
