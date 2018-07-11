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
            m_vikingController.enabled = false;
        }
    }

    public void Respawn()
    {
        transform.position = new Vector3(Mathf.Lerp(m_vikingController.m_leftBounds, m_vikingController.m_rightBounds, 0.5f), m_vikingController.m_topBounds + 5f, transform.position.z);
        m_hasRespawned = true;
        StartCoroutine(RespawnDuration(m_respawnDuration));
    }

    IEnumerator RespawnDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        m_hasRespawned = false;
        m_vikingController.enabled = true;
    }

}
