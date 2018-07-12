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
    private Vector3 m_targetPosition;
	// Use this for initialization
	void Start ()
    {
        m_gameController = GameController.Instance;
        m_vikingController = GetComponent<VikingController>();
	}

    private void Update()
    {
        if(m_hasRespawned)
        {
            //m_vikingController.enabled = false;
        }
    }

    public void Respawn()
    {
        Debug.Log("Respawn called");
        transform.position = new Vector3(Mathf.Lerp(m_vikingController.m_leftBounds, m_vikingController.m_rightBounds, 0.5f), m_vikingController.m_topBounds + 5f, m_gameController.snapGridZ);
        transform.rotation = Quaternion.identity;

        //we want isCarried to be false because we want the viking to be pickup-able when they respawn
        m_vikingController.SetCarried(false);

        //and then we want to assign the rigidbody stuff to exactly what we need for them to be pickupable when they respawn
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<Rigidbody>().detectCollisions = true;
        gameObject.GetComponent<CharacterController>().enabled = false;

        //we also want to make sure that the player is not immune to being picked up when respawning
        gameObject.GetComponent<DetectPickup>().m_immuneToPickUp = false;

        m_hasRespawned = true;
        StartCoroutine(RespawnDuration(m_respawnDuration));
    }

    IEnumerator RespawnDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        //reset everything to their original values yaaay
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        gameObject.GetComponent<Rigidbody>().detectCollisions = true;
        gameObject.GetComponent<CharacterController>().enabled = true;
        m_hasRespawned = false;
        //m_vikingController.enabled = true;
    }

}
