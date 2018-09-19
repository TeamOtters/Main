using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InGameMenu : MonoBehaviour
{
    public Canvas m_InGameMenu;

    public object buttons { get; private set; }

    public void Start()
    {
        Time.timeScale = 1.0f;
        m_InGameMenu.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetButtonDown("Pause") && m_InGameMenu.isActiveAndEnabled)
        {

            Debug.Log("Hitting pause again");
            TogglePause();
            m_InGameMenu.gameObject.SetActive(false);
            

        }

        else if (Input.GetButtonDown("Pause"))
        {

            Debug.Log("Hitting pause");
            TogglePause();
            m_InGameMenu.gameObject.SetActive(true);

        }



    }
    public void TogglePause()
    {
        Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
    }
}