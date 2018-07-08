using UnityEngine;
using UnityEngine.UI;

public class BouncingBall : MonoBehaviour {

    public Rigidbody rb;

    //Making the ball bounce diagonally
    public float XBounceSpeed = 0f;
    public float YBounceSpeed = 0f;

    //Health on BouncingBall enemy
    public float startHealth = 100;
    private float health;

    //Image used for decreasing heltbar. Might not be used in final game.
    public Image healthBar; 

    void Start()
    {
        rb.velocity = new Vector2(XBounceSpeed, YBounceSpeed);
        health = startHealth;
    }

    void OnCollisionEnter(Collision collisionInfo)
     {
     if (collisionInfo.gameObject.CompareTag ("Axe"))
       {
          Debug.Log(collisionInfo.gameObject.tag);
            rb.AddForce(0, 200, 0 * Time.deltaTime);
       }
     }

    public void TakeDamage(float amount)
    {
        health -= amount;

        healthBar.fillAmount = health / startHealth;

        if (health <= 0)
        {
           Destroy(gameObject);
        }
    }
}
