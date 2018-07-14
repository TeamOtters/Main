using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingRespawn : MonoBehaviour
{

    public float m_respawnDuration;

    private VikingController m_vikingController;
    private GameController m_gameController;
    internal bool m_hasRespawned = false;
    private Vector3 m_startPosition;
    private Transform m_targetTransform;
    private float m_xPos;

    private List<Transform> m_respawnPoints = new List<Transform>();

	// Use this for initialization
	void Start ()
    {
        m_gameController = GameController.Instance;
        m_vikingController = GetComponent<VikingController>();
        RespawnPoint[] respawnPointsInScene = FindObjectsOfType<RespawnPoint>();
        foreach(RespawnPoint respawnPointInScene in respawnPointsInScene)
        {
            m_respawnPoints.Add(respawnPointInScene.gameObject.transform);
        }

	}

    private void Update()
    {
        if(m_hasRespawned)
        {
            transform.position = new Vector3(m_xPos, transform.position.y, 0f);
        }

    }

    private Transform FindClosestRespawnPointToTransform(Transform transform)
    {

        float closestDistanceSqr = Mathf.Infinity;
        Transform closestTransform = null;
        foreach (Transform respawnPoint in m_respawnPoints)
        {
            Vector3 directionToTarget = respawnPoint.position - transform.position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestTransform= respawnPoint;
            }
        }
        return closestTransform;
    }

    public void Respawn()

    {
        m_vikingController.SetStunned(m_respawnDuration);
        m_hasRespawned = true;
        m_vikingController.GetComponent<Rigidbody>().velocity = Vector3.zero;
        m_vikingController.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Debug.Log("I am respawning!");
        transform.position = new Vector3(transform.position.x, m_vikingController.m_topBounds + 1.5f, m_gameController.snapGridZ);
        transform.rotation = Quaternion.identity;
        m_targetTransform = FindClosestRespawnPointToTransform(transform);
        Debug.Log("Found " + m_targetTransform.gameObject.name + "at position " + m_targetTransform.position);
        transform.position = new Vector3(m_targetTransform.position.x, m_vikingController.m_topBounds + 2f, m_gameController.snapGridZ);
        m_xPos = transform.position.x;
        //we also want to make sure that the player is not immune to being picked up when respawning
        m_vikingController.SetCarried(false);
        gameObject.GetComponent<DetectPickup>().m_immuneToPickUp = false;
        //and make sure that they cannot collide with the goal 

        /*
        //and then we want to assign the rigidbody stuff to exactly what we need for them to be pickupable when they respawn
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<Rigidbody>().detectCollisions = true;
        gameObject.GetComponent<CharacterController>().enabled = false;
        
        */

       
        StartCoroutine(RespawnDuration(m_respawnDuration));
    }

    IEnumerator RespawnDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        m_hasRespawned = false;
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if(m_hasRespawned && collision.gameObject.tag == "Goal")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            Debug.Log("Ignored collision");
        }
    }
    */

}
