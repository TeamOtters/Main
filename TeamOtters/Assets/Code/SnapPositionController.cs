using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SnapablePlayers
{
    public List<GameObject> m_viking { get; set; }
    public List<GameObject> m_valkyrie { get; set; }

   /* public  SnapablePlayers (GameObject viking, GameObject valkyrie)
    {
       
    }*/
}



public class SnapPositionController : MonoBehaviour {

    public SnapablePlayers m_snapables;
    public GameObject m_scoreBall;

   public List<GameObject> m_positionsZ = new List<GameObject>();

	// Use this for initialization
	void Start () {
       // m_positionsZ = new List<GameObject>();
        //Populate();

        

     


        //var snapablesCount = m_snapables.Length;



       /* for (int i = 0; i < snapablesCount; i++)
        {
            //m_positionsZ.Add(m_snapables[i].m_valkyrie.;
            //m_positionsZ.Add(m_snapables[i].m_viking.gameObject);
        }    */
        
       

    }

   /* IEnumerator LateStart ()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 4; i++)
        {
            //m_positionsZ.Add(m_snapables.m_viking[i].gameObject);
            //m_positionsZ.Add(m_snapables.m_valkyrie[i].gameObject);
        } 
    }*/
	/*
    private void Populate ()
    {
        int players = 4;

        for (int i = 0; i < players; i++)
        {
           var data = (PlayerData)FindObjectOfType(typeof(PlayerData));
           var index = data.m_PlayerIndex;
           if (index == i)
           {
                m_snapables[i].m_viking = data.gameObject.GetComponentInChildren<VikingController>().gameObject;
                m_snapables[i].m_valkyrie = data.gameObject.GetComponentInChildren<ValkyrieController>().gameObject;
            }
        }
    }*/

	// Update is called once per frame
	void Update ()
    {	
        for (int i = 0; i < m_positionsZ.Count; i++)
        {
            m_positionsZ[i].transform.position = new Vector3(m_positionsZ[i].transform.position.x, m_positionsZ[i].transform.position.y, GameController.Instance.snapGridZ);
        }

       /* for(int i = 0; i < 4; i++)
        {
            m_snapables.m_valkyrie[i].transform.position = new Vector3(m_snapables.m_valkyrie[i].transform.position.x, m_snapables.m_valkyrie[i].transform.position.y, GameController.Instance.snapGridZ);
            m_snapables.m_viking[i].transform.position = new Vector3(m_snapables.m_viking[i].transform.position.x, m_snapables.m_viking[i].transform.position.y, GameController.Instance.snapGridZ);
        }*/

        if(m_scoreBall != null)
             m_scoreBall.transform.position = new Vector3(m_scoreBall.transform.position.x, m_scoreBall.transform.position.y, GameController.Instance.snapGridZ);
    }
}
