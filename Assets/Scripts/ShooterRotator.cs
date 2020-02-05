using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterRotator : MonoBehaviour
{
    private enum RotateState    // 포신의 상태
    {
        Idle, Vertical, Horizontal, Ready
    }

    private RotateState state = RotateState.Idle;   // 시작은 Idle

    public float verticalRotateSpeed = 360.0f;      // 초당 360도  
    public float horizontalRotateSpeed = 360.0f;

    private void Update()
    {
        switch(state)
        {
            case RotateState.Idle:
                if(Input.GetButtonDown("Fire1"))
                {
                    state = RotateState.Horizontal;
                }
                break;

            case RotateState.Horizontal:
                if (Input.GetButton("Fire1"))
                {
                    transform.Rotate(new Vector3(0, horizontalRotateSpeed * Time.deltaTime, 0));
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    state = RotateState.Vertical;
                }
                break;

            case RotateState.Vertical:
                if (Input.GetButton("Fire1"))
                {
                    transform.Rotate(new Vector3(-verticalRotateSpeed * Time.deltaTime, 0, 0));
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    state = RotateState.Ready;
                }
                break;

            case RotateState.Ready:
                break;
        }
    }
}
