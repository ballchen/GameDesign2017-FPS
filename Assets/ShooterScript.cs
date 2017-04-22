using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterScript : MonoBehaviour {

    public Animator animator;
    private float MinimumHitPeriod = 1f;
    private float HitCounter = 0;
    public float CurrentHP = 100;
    public bool isHitByFire = false;

    public float MoveSpeed;
    public GameObject FollowTarget;
    private Rigidbody rigidBody;
    public CollisionListScript PlayerSensor;
    public CollisionListScript AttackSensor;

    public void AttackPlayer()
    {
        if (AttackSensor.CollisionObjects.Count > 0)
        {
            AttackSensor.CollisionObjects[0].transform.GetChild(0).GetChild(0).SendMessage("Hit", 10);

        }
    }

    // Use this for initialization
    void Start()
    {
        rigidBody = this.GetComponent<Rigidbody>();
        FollowTarget = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {

       


        if (CurrentHP > 0 && HitCounter > 0)
        {
            HitCounter -= Time.deltaTime;
        }
        else
        {
            if (isHitByFire)
            {
                if (HitCounter <= 0)
                {
                    HitCounter = 0.1f;
                    CurrentHP -= 5;

                    animator.SetFloat("HP", CurrentHP);

                    if (CurrentHP <= 0)
                    {
                        animator.SetTrigger("Hit");
                        BuryTheBody();
                    }
                }
            }

            if (CurrentHP > 0)
            {
                if (FollowTarget != null)
                {
                    Vector3 lookAt = FollowTarget.gameObject.transform.position;
                    lookAt.y = this.gameObject.transform.position.y;
                    this.transform.LookAt(lookAt);
                    animator.SetBool("Run", true);

                }
            }
            else
            {
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }

    public void Hit(float value)
    {
        if (HitCounter <= 0)
        {
            FollowTarget = GameObject.FindGameObjectWithTag("Player");
            HitCounter = MinimumHitPeriod;
            CurrentHP -= value;

            animator.SetFloat("HP", CurrentHP);
            animator.SetTrigger("Hit");

            if (CurrentHP <= 0) { BuryTheBody(); }
        }
    }

    public void HitByFire(bool isHit)
    {
        isHitByFire = isHit;
    }

    void BuryTheBody()
    {
        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<Collider>().enabled = false;
        this.transform.DOMoveY(-0.8f, 1f).SetRelative(true).SetDelay(1).OnComplete(() =>
        {
            this.transform.DOMoveY(-0.8f, 1f).SetRelative(true).SetDelay(3).OnComplete(() =>
            {
                GameObject.Destroy(this.gameObject);
            });
        });
    }
}
