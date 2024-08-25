using UnityEngine;

public class State_Falling : State
{
    private float myJumpTime;
    private float fallingPower;
    private float stoppingChangingDirection;

    private bool stopChangingDirection;

    #region Unity
    private void FixedUpdate()
    {
        if (!isActive)
        {
            return;
        }
        if (!CanMove)
        {
            return;
        }
        //Check Ground
        myJumpTime += Time.deltaTime;
        if (myJumpTime > 0.25f)
        {
            bool isGroundLeft = Physics2D.Raycast(transform.position + Vector3.left * 0.45f, Vector2.down, 0.1f, MyControllerManager.GrondMask);
            bool isGroundCenter = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, MyControllerManager.GrondMask);
            bool isGroundRight = Physics2D.Raycast(transform.position + Vector3.right * 0.45f, Vector2.down, 0.1f, MyControllerManager.GrondMask);
            JumpReset(isGroundLeft, isGroundCenter, isGroundRight);
        }
        MyRigidbody.velocity = MyDirection;
    }
    #endregion

    #region Jump
    public void JumpReset(bool isGroundLeft, bool isGroundCenter, bool isGroundRight)
    {
        if (isGroundLeft || isGroundCenter || isGroundRight)
        {
            ResetJump();
        }
    }
    private void ResetJump()
    {
        MyControllerManager.SetState(StateType.Walking);
        JumpFinish();
    }
    public void JumpFinish()
    {
        myJumpTime = 0;
        SetDirection(Vector2.zero);
        MyRigidbody.velocity = MyDirection;
    }
    #endregion

    #region State
    public override void SetState()
    {
        base.SetState();
        StateSpecial();
    }
    public override void StateSpecial()
    {
        SetDashTime(0);
        SetIsDash(false);
        SetCanMove(true);
        SetCanControl(true);
        MyControllerManager.SetGravity(1);
    }
    public override void StopState()
    {
        isActive = false;
        StateSpecial();
    }
    public void SetFalling(int stoppingPower)
    {
        MyControllerManager.SetGravity(1);
        stopChangingDirection = true;
        stoppingChangingDirection = 0.15f;
        fallingPower = stoppingPower;
        SetDirection(new Vector2(stoppingPower, -3));
        MyRigidbody.velocity = MyDirection;
    }
    public override void UpdateState()
    {
        if (IsDash)
        {
            DashCoolDown();
        }
        else
        {
            if (!CanControl)
            {
                return;
            }
            if (stopChangingDirection)
            {
                stoppingChangingDirection -= Time.deltaTime;
                if (stoppingChangingDirection < 0)
                {
                    stopChangingDirection = false;
                    fallingPower = Mathf.Abs(fallingPower);
                }
                return;
            }
            if (DirX == 1)
            {
                MyControllerManager.SetAngleY(0);
            }
            else if (DirX == -1)
            {
                MyControllerManager.SetAngleY(180);
            }
            SetDirection(new Vector2(DirX * fallingPower, MyRigidbody.velocity.y));
            MyControllerManager.SetFloatAnimatiorLeg("DirX", Mathf.Abs(DirX * fallingPower));
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    MyControllerManager.SetState(StateType.Jumping);
            //    // Jumpla
            //    State_Jumping state_Jumping = MyControllerManager.myState as State_Jumping;
            //    if (state_Jumping is not null)
            //    {
            //        state_Jumping.JumpingJump();
            //    }
            //}
            //else if (Input.GetKeyDown(KeyCode.V))
            //{
            //    // Dash yapacak
            //    Dash();
            //}
            //else if (Input.GetKeyUp(KeyCode.B))
            //{
            //    // Bomb atacak
            //    MyControllerManager.SetTriggerAnimatiorLeg("Bomb");
            //}
        }
    }
    #endregion

    #region Jump
    public override void Jump()
    {
        MyControllerManager.SetState(StateType.Jumping);
        // Jumpla
        State_Jumping state_Jumping = MyControllerManager.myState as State_Jumping;
        if (state_Jumping is not null)
        {
            state_Jumping.JumpingJump();
        }
    }
    #endregion

    #region Dash
    public override void Dash()
    {
        SetIsDash(true);
        SetCanControl(false);
        SetDashTime(MyDashTime);
        MyControllerManager.SetGravity(0);
        if (transform.eulerAngles.y == 0)
        {
            SetDirection(new Vector2(MyDashSpeed, 0));
        }
        else
        {
            SetDirection(new Vector2(-MyDashSpeed, 0));
        }
        MyControllerManager.SetTriggerAnimatiorLeg("Dash");
    }
    public override void OnDashCoolDown()
    {
        StateSpecial();
    }
    #endregion
}