using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSensor : MonoBehaviour {
    private int TouchedCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        TouchedCount++;
    }

    private void OnTriggerExit(Collider other)
    {
        TouchedCount--;
    }

    public bool IsCanJump()
    {
        return TouchedCount > 0; 
    }
}
