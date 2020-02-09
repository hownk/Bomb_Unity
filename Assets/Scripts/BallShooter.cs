using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallShooter : MonoBehaviour
{
    // 포탄 발사시 카메라 타겟을 포탄으로
    public CamFollow cam;   

    // 생성할 볼, 발사위치, 슬라이더UI
    public Rigidbody ball;
    public Transform firePosition;
    public Slider powerSlider;

    // 게이지 충전소리, 발사소리
    public AudioSource shootingAudio;
    public AudioClip fireClip;
    public AudioClip chargingClip;

    // 슬라이더 최솟값, 최댓값, 차징시간
    public float minForce = 15.0f;
    public float maxForce = 30.0f;
    public float chargingTime = 0.75f;

    // 현재 힘, 차징속도, 발사여부판단
    private float currentForce;
    private float chargeSpeed;
    private bool fired;

    // 매번 비활성화 되있다가 활성화 시킬 것이기 때문에
    // OnEnalbe() 함수에서 초기화를 해 준다.
    private void OnEnable()
    {
        currentForce = minForce;
        powerSlider.value = minForce;
        fired = false;
    }

    private void Start()
    {
        chargeSpeed = (maxForce - minForce) / chargingTime;
    }


    private void Update()
    {
        if (fired == true)  // 발사 한번 했으면 이제 안함
            return;

        // 최대힘을 넘어가면 자동으로 발사
        if(currentForce >= maxForce && !fired)
        {
            currentForce = maxForce;
            Fire();
        }
        // 발사키 누르면 차징시작
        else if(Input.GetButtonDown("Fire1"))
        {
            currentForce = minForce;
            shootingAudio.clip = chargingClip;  // 충전 소리
            shootingAudio.Play();
        }
        // 발사키 계속 누르고 있으면 계속 차징
        else if(Input.GetButton("Fire1") && !fired)
        {
            currentForce += chargeSpeed * Time.deltaTime;
            powerSlider.value = currentForce;
        }
        // 발사키 떼면 발사
        else if(Input.GetButtonUp("Fire1") && !fired)
        {
            Fire();
        }
    }


    private void Fire() // 발사 기능
    {
        // 볼 프리팹 생성 후 전방벡터에 Force를 곱해주어 날려보낸다.
        // 발사 오디오 플레이 후 일부 값은 초기화
        fired = true;
        Rigidbody ballInstance = Instantiate(ball, firePosition.position, firePosition.rotation);
        ballInstance.velocity = currentForce * firePosition.forward;
        shootingAudio.clip = fireClip;
        shootingAudio.Play();

        currentForce = minForce;
        powerSlider.value = minForce;

        cam.SetTarget(ballInstance.transform, CamFollow.State.Tracking);
    }
}
