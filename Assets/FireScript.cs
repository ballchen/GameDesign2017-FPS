using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SendMessage("HitByFire", true);   
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.SendMessage("HitByFire", false);
    }

}
