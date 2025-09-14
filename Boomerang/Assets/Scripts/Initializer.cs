using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    private bool initialized;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = func.FRAMERATE;
        ClearData.Initialize();
        initialized = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!initialized)
        {
            Fader.SetFader(0, true, "Title");
            initialized = true;
        }
    }
}
