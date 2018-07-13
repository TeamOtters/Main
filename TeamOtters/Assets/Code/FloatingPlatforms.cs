using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloatingPlatforms : MonoBehaviour {

    public PlatformType m_platformType;
    public UnityEvent m_onCollisionEvent;
    private float m_originalPos;
    private bool m_bounce;


    public enum PlatformType
    {
        None,
        Cloud

    }

    private void Start()
    {
        m_originalPos = this.transform.position.y;
        m_rb = GetComponent<Rigidbody>();
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
            m_bounce = true;
            //m_onCollisionEvent.Invoke();

          
          

            Debug.Log("CLOUD");
            
        }
    }

    private bool m_invoked;
    private void CloudBounce()
    {
        var cloudMoveDist = 0.5f;
 

            transform.position = new Vector3(transform.position.x,
            m_originalPos + ((float)Mathf.Sin(Time.time) * cloudMoveDist),
            0);
    }

  

   
}
