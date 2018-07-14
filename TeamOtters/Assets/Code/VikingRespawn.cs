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
    private CameraBehaviourManager m_cameraMan;
    private float m_cameraOriginalSpeed;

    private List<Transform> m_respawnPoints = new List<Transform>();

	// Use this for initialization
	void Start ()
    {
        m_gameController = GameController.Instance;
        m_vikingController = GetComponent<VikingController>();

        m_cameraMan = m_gameController.cameraManager;
        m_cameraOriginalSpeed = m_cameraMan.m_panSpeedY;

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
        //initialize variables for respawn
        m_vikingController.SetStunned(m_respawnDuration);
        m_hasRespawned = true;
        m_vikingController.GetComponent<Rigidbody>().velocity = Vector3.zero;
        m_vikingController.GetComponent<Rigidbody>().velocity = Vector3.zero;

        //find new position
        transform.position = new Vector3(transform.position.x, m_vikingController.m_topBounds + 1.5f, m_gameController.snapGridZ);
        transform.rotation = Quaternion.identity;
        m_targetTransform = FindClosestRespawnPointToTransform(transform);
        transform.position = new Vector3(m_targetTransform.position.x, m_vikingController.m_topBounds + 2f, m_gameController.snapGridZ);

        //assign new x position
        m_xPos = transform.position.x;

        //make sure you can get picked up
        m_vikingController.SetCarried(false);
        gameObject.GetComponent<DetectPickup>().m_immuneToPickUp = false;

        //tell cameraMan to stop and look at you
        m_cameraMan.m_panSpeedY = 0f;

        //move on
        StartCoroutine(RespawnDuration(m_respawnDuration));
    }

    IEnumerator RespawnDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        StopRespawn();
    }

    public void StopRespawn()
    {
        m_hasRespawned = false;
        m_cameraMan.m_panSpeedY = m_cameraOriginalSpeed;
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
