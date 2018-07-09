using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOut : MonoBehaviour {
    public Canvas m_TimeOut;
    

    void Start()
    {
        StartCoroutine("Wait");
        Time.timeScale = 0.0f;
             
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        m_TimeOut.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        
    }


   /* void Update () {
        if (gameObject.)
        {
            m_TimeOut.gameObject.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }*/
    

}

   
