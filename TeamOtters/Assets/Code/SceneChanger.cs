using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour {

	
    public void OnClick()
 

        {
            //The SceneManager loads your new Scene as a single Scene (not overlapping). This is Single mode.
            SceneManager.LoadScene("Menu_Screen", LoadSceneMode.Single);
        }
   




    
}
