using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShots : MonoBehaviour
{


	void Update ()
    {

        if (Input.GetKeyDown(KeyCode.S))
        {
            ScreenCapture.CaptureScreenshot("Es aquest Alex", 4);
            Debug.Log("Aparcao");
        }



	}

}
