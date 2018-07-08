using UnityEngine;
using UnityEngine.UI;

public class BouncingBall : MonoBehaviour {

    public Rigidbody rb;
    public float XBounceSpeed = 0f;
    public float YBounceSpeed = 0f;



    public float startHealth = 100;
    private float health; 
    public int worth = 50;
    public GameObject deathEffect;

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
       }
     }

    public void TakeDamage(float amount)
    {
        health -= amount;

        healthBar.fillAmount = health / startHealth;

        if (health <= 0)
        {
            Die();
        }
    }
}
