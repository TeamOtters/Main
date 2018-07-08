using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    //Amount of damage dealt. This script should be placed on axe projectiles, not the players. 
    public float enemyDamage = 25f; 

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("BouncingBall"))
        {
            Debug.Log("damage dealt");

            //Calling script on BouncingBall to drain HP
            collisionInfo.gameObject.GetComponent<BouncingBall>().TakeDamage(enemyDamage);
        }
    }


}
