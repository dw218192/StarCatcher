using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

public class Discard : MonoBehaviour
{
    Grabbable grabbable;
    void Awake()
    {
        grabbable = GetComponent<Grabbable>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the B button on the right controller is pressed
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            if (grabbable.IsHeld())
            {
                grabbable.ForceHandsRelease();
            }
        }
    }
}
