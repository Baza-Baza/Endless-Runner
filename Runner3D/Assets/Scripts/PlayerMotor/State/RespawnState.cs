﻿using UnityEngine;

public class RespawnState : BaseState
{
    [SerializeField] float verticalDistance = 25.0f;
    [SerializeField] float immunityTime = 1.0f;

    private float startTime;

    public override void Construct()
    {
        startTime = Time.time;
        motor.controller.enabled = false;
        motor.transform.position = new Vector3(0, verticalDistance, motor.transform.position.z);
        motor.controller.enabled = true;

        motor.verticalVelocity = 0.0f;
        motor.currentLine = 0;
        motor.anim?.SetTrigger("Respawn");

        GameManager.Instance.ChangeCamera(GameCamera.Respawn);
    }
    public override void Destruct()
    {
        GameManager.Instance.ChangeCamera(GameCamera.Game);
    }
    public override Vector3 ProcessMotion()
    {
        motor.ApplyGravity();

        Vector3 m = Vector3.zero;

        m.x = motor.SnapToLane();
        m.y = motor.verticalVelocity;
        m.z = motor.baseRunSpeed;
        return m;
    }
    public override void Transition()
    {
        if (motor.isGrounded && (Time.time - startTime) > immunityTime)
            motor.ChangeState(GetComponent<RunningState>());
        if (InputManager.Instance.SwipeLeft)
        {
            motor.ChangeLane(-1);
        }
        if (InputManager.Instance.SwipeRight)
        {
            motor.ChangeLane(1);
        }
    }
        
}