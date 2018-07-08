using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InGameMenu : MonoBehaviour
{
    public Canvas m_InGameMenu;
    public Canvas m_Overlay;
    private void Update()
    {
        if (Input.GetKeyDown("joystick button 7"))
        {
            Debug.Log("Hitting pause");
            TogglePause();
            m_InGameMenu.gameObject.SetActive(true);
            m_Overlay.gameObject.SetActive(false);
            Debug.Log(message: "This canvas is " + m_InGameMenu.name);
        }
    }
    public void TogglePause()
    {
        Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
    }
}