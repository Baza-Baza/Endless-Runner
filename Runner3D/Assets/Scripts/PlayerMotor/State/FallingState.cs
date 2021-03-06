using UnityEngine;

public class FallingState : BaseState
{
    public override void Construct()
    {
        motor.anim?.SetTrigger("Fall");
    }
    public override void Transition()
    {
        if (motor.isGrounded)
            motor.ChangeState(GetComponent<RunningState>());    
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
}
