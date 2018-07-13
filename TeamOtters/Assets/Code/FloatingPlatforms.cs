using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloatingPlatforms : MonoBehaviour {

    public PlatformType m_platformType;
    public UnityEvent m_onCollisionEvent;

    public enum PlatformType
    {
        None,
        Cloud

    }

	// Use this for initialization
	void Start () {

        if (m_onCollisionEvent == null)
            m_onCollisionEvent = new UnityEvent();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.CompareTag("Viking"))
        {
            if(m_platformType == PlatformType.Cloud)
            {
                m_onCollisionEvent.Invoke();
                Debug.Log("CLOUD");
            }
        }
    }

   
}
