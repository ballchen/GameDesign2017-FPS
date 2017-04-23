using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{

    public float FlyingSpeed;
    public float LifeTime;
    public GameObject explosion;
    public AudioSource bulletSound;

    public void InitAndShoot(Vector3 Direction)
    {
        Rigidbody rigidbody = this.GetComponent<Rigidbody>();

        rigidbody.velocity = Direction * FlyingSpeed;

        Invoke("KillYourself", LifeTime);
    }

    public void KillYourself()
    {
        GameObject.Destroy(this.gameObject);
    }

    public float damageValue = 15;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.gameObject.transform.GetChild(0).GetChild(0).SendMessage("Hit", 10);
        } else
        {
            other.gameObject.SendMessage("Hit", damageValue);
        }

        explosion.gameObject.transform.parent = null;
        explosion.gameObject.SetActive(true);
        bulletSound.pitch = Random.Range(0.8f, 1);

        KillYourself();
    }

}