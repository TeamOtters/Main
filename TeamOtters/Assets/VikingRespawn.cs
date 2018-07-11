using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingRespawn : MonoBehaviour
{
    private VikingController m_vikingController;
    private GameController m_gameController;

	// Use this for initialization
	void Start ()
    {
        m_gameController = GameController.Instance;
        m_vikingController = GetComponent<VikingController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
