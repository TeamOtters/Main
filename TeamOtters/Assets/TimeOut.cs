using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOut : MonoBehaviour {
    public Canvas m_TimeOut;

    private void Start()
    {
        Time.timeScale = 0.0f;
    }

    void Update () {
        if (Time.frameCount == 35f)
        {
            m_TimeOut.gameObject.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }
    

}

   
