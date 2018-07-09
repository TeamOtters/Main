using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SnapablePlayers
{
    public GameObject m_viking;
    public GameObject m_valkyrie;

}

public class SnapPositionController : MonoBehaviour {

    public SnapablePlayers[] m_snapables;
    public GameObject m_scoreBall;

    private List<GameObject> m_positionsZ;

	// Use this for initialization
	void Start () {
        m_positionsZ = new List<GameObject>();

        var snapablesCount = m_snapables.Length;

        for (int i = 0; i < snapablesCount; i++)
        {
            m_positionsZ.Add(m_snapables[i].m_valkyrie.gameObject);
            m_positionsZ.Add(m_snapables[i].m_viking.gameObject);
        }       
    }
	
	// Update is called once per frame
	void Update ()
    {	
        for (int i = 0; i < m_positionsZ.Count; i++)
        {
            m_positionsZ[i].transform.position = new Vector3(m_positionsZ[i].transform.position.x, m_positionsZ[i].transform.position.y, GameController.Instance.snapGridZ);
        }

        if(m_scoreBall != null)
             m_scoreBall.transform.position = new Vector3(m_scoreBall.transform.position.x, m_scoreBall.transform.position.y, GameController.Instance.snapGridZ);
    }
}
