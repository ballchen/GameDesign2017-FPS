using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGunManager : MonoBehaviour {

    public ParticleSystem fireParticle;

    public void TriggerFireGun()
    {
        if(!fireParticle.gameObject.active)
        {
            fireParticle.gameObject.SetActive(true);
        }
        
    }

    public void StopFireGun()
    {
        fireParticle.gameObject.SetActive(false);
    }
}
