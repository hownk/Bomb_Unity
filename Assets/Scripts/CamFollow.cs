using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
  public enum State
    {
        Idle, Ready, Tracking
    }

    private State state
    {
        set 
        { 
            switch(value)
            {
                case State.Idle:
                    targetZoomSize = roundReadyZoomSize;
                    break;
                case State.Ready:
                    targetZoomSize = readyShotZoomSize;
                    break;
                case State.Tracking:
                    targetZoomSize = trackingZoomSize;
                    break;
            }
        }
    }

    private Transform target;   // 추적할 타겟
    private Vector3 targetPosition; // 타겟의 포지션
    private Vector3 lastmovingVelocity; // 카메라 댐핑을 위한 변수
    
    public float smoothTime = 0.2f; // 지연시간

    // 카메라, 현재 줌 사이즈
    private Camera cam;
    private float targetZoomSize = 5.0f;

    // state별 카메라 줌 사이즈 (고정값)
    private const float roundReadyZoomSize = 14.5f;
    private const float readyShotZoomSize = 5.0f;
    private const float trackingZoomSize = 10.0f;

    private float lastZoomSpeed;    // 줌인/아웃 댐핑을 위한 변수

    // 자식으로있는 카메라 가져오고 Idle 상태로 시작
    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        state = State.Idle;
    }

    // Vector3.SmoothDamp() 함수를 이용해 댐핑을 주면서 타겟으로 카메라 이동
    private void Move()
    {
        targetPosition = target.transform.position;

        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref lastmovingVelocity , smoothTime);

        transform.position = smoothPosition;
    }

    // Mathf.SmoothDamp() 함수를 이용해 댐핑을 주면서 줌인/아웃
    private void Zoom()
    {
        float smoothZoomSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoomSize,ref lastZoomSpeed , smoothTime);

        cam.orthographicSize = smoothZoomSize;
    }

    // FixedUpdate()에서 Move()와 Zoom() 실행
    private void FixedUpdate()
    {
        if(target != null)
        {
            Move();
            Zoom();
        }
    }

    // 리셋, Idle로 설정
    public void Reset()
    {
        state = State.Idle;
    }

    public void SetTarget(Transform newTarget, State newState)
    {
        target = newTarget;
        state = newState;
    }
}
