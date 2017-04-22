using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private Animator animatorController;
    public Transform rotateYTransform;
    public Transform rotateXTransform;
    public float rotateSpeed;
    public float currentRotateX = 0;
    public float MoveSpeed;
    public int currentGun = 0;
    public List<Transform> PlayerGuns;
    float currentSpeed = 0;

    public Rigidbody rigidBody;

    public JumpSensor JumpSensor;
    public float JumpSpeed;
    public GunManager gunManager;
    public FireGunManager FireGunManager;
    public GameUIManager uiManager;
    public int hp = 100;

    public AudioSource footStepSound;
    private bool footStepPlaying = false;

    // Use this for initialization
    void Start()
    {
        animatorController = this.GetComponent<Animator>();
    }

    public void Hit(int value)
    {
        if (hp <= 0)
        {
            return;
        }

        hp -= value;
        uiManager.SetHP(hp);

        if (hp > 0)
        {
            uiManager.PlayHitAnimation();
        }
        else
        {
            uiManager.PlayerDiedAnimation();

            rigidBody.gameObject.GetComponent<Collider>().enabled = false;
            rigidBody.useGravity = false;
            rigidBody.velocity = Vector3.zero;
            this.enabled = false;
            rotateXTransform.transform.DOLocalRotate(new Vector3(-60, 0, 0), 0.5f);
            rotateYTransform.transform.DOLocalMoveY(-1.5f, 0.5f).SetRelative(true);
        }
    }

    public void ChangeGun(int gunCode)
    {
        foreach(Transform gun in PlayerGuns)
        {
            gun.gameObject.SetActive(false);
        }

        PlayerGuns[gunCode].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


        if(Input.GetKey(KeyCode.Alpha1))
        {
            currentGun = 0;
            ChangeGun(currentGun);
        } else if(Input.GetKey(KeyCode.Alpha2))
        {
            currentGun = 1;
            ChangeGun(currentGun);
        }

        if(currentGun == 0)
        {
            if (Input.GetMouseButton(0))
            {
                gunManager.TryToTriggerGun();
            }
        } else if(currentGun == 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                FireGunManager.TriggerFireGun();
            }

            if (Input.GetMouseButtonUp(0))
            {
                FireGunManager.StopFireGun();
            }
        }

        //決定鍵盤input的結果
        Vector3 movDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (!footStepPlaying)
            {
                footStepPlaying = true;
                footStepSound.time = 5.0f;
                footStepSound.Play();
            }
            
        } else
        {
            footStepPlaying = false;
            footStepSound.Stop();
        }

        if (Input.GetKey(KeyCode.W)) { movDirection.z += 1; }
        if (Input.GetKey(KeyCode.S)) { movDirection.z -= 1; }
        if (Input.GetKey(KeyCode.D)) { movDirection.x += 1; }
        if (Input.GetKey(KeyCode.A)) { movDirection.x -= 1; }
        movDirection = movDirection.normalized;

        //決定要給Animator的動畫參數
        if (movDirection.magnitude == 0 || !JumpSensor.IsCanJump()) { currentSpeed = 0; }
        else
        {
            if (movDirection.z < 0) { currentSpeed = -MoveSpeed; }
            else { currentSpeed = MoveSpeed; }
        }
        animatorController.SetFloat("Speed", currentSpeed);

        //轉換成世界座標的方向
        Vector3 worldSpaceDirection = movDirection.z * rotateYTransform.transform.forward +
                                      movDirection.x * rotateYTransform.transform.right;
        Vector3 velocity = rigidBody.velocity;
        velocity.x = worldSpaceDirection.x * MoveSpeed;
        velocity.z = worldSpaceDirection.z * MoveSpeed;

        if (Input.GetKey(KeyCode.Space) && JumpSensor.IsCanJump())
        {
            velocity.y = JumpSpeed;
        }

        rigidBody.velocity = velocity;

        //計算滑鼠
        rotateYTransform.transform.localEulerAngles += new Vector3(0, Input.GetAxis("Horizontal"), 0) * rotateSpeed;
        currentRotateX += Input.GetAxis("Vertical") * rotateSpeed;

        if (currentRotateX > 90)
        {
            currentRotateX = 90;
        }
        else if (currentRotateX < -90)
        {
            currentRotateX = -90;
        }
        rotateXTransform.transform.localEulerAngles = new Vector3(currentRotateX, 0, 0);

    }
}