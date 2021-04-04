using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetupUtil : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera camera = Camera.main;
        if (camera != null)
        {
            camera.orthographic = true;
            camera.orthographicSize = Screen.height / 2.0f;
        }

        Screen.SetResolution(Screen.width, Screen.height, true);
    }
}