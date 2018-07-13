using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloatingPlatforms : MonoBehaviour {

    public PlatformType m_platformType;
    public UnityEvent m_onCollisionEvent;
    private float m_originalPos;
    private bool m_bounce;
    private float cloudMoveDist = 0.5f;


    public enum PlatformType
    {
        None,
        Cloud

    }

    private void Start()
    {
        m_originalPos = this.transform.position.y;
    }

    private void Update()
    {
        if (m_platformType == PlatformType.Cloud)
           CloudBounce();
            

       
    }

    private void OnTriggerEnter(Collider col)
    {
        
        if (col.gameObject.CompareTag("Viking"))
        {
            //m_bounce = true;
            m_onCollisionEvent.Invoke();
            cloudMoveDist = 1f;
          
          

            Debug.Log("CLOUD");
            
        }
    }

    public void SetBounceTrue ()
    {
       m_bounce = false;
    }

    private bool m_invoked;
    private void CloudBounce()
    {
        cloudMoveDist = 0.5f;

        if (!m_bounce)
        {
            transform.position = new Vector3(transform.position.x,
            m_originalPos + ((float)Mathf.Sin(Time.time) * cloudMoveDist),
            0);
        }
        
    }

  

   
}
