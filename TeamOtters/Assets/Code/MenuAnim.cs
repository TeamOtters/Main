using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class MenuAnim : Selectable
{
    BaseEventData m_base;
    private bool m_menuShine = false;

    protected override void Start()
    {
        base.Start();
        m_menuShine = GetComponentInChildren<Button>(); 
    }

    public void Update() 
    {
        if (IsHighlighted(m_base))
            Debug.Log ( gameObject.name + "is Highlighted");
     
    }

}

