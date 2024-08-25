using UnityEngine;

public class State_Walling_Up : State
{
    [Header("Jump")]
    [SerializeField] private float myJumpTime = 1;
    [SerializeField] private float myJumpSpeed = 3;

    [Header("Walling")]
    [SerializeField] private float myWallingSpeed = 1;

    private float myJumpTimeNext;

    public bool isJump = false;

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
        MyRigidbody.velocity = MyDirection;
    }
    #endregion

    #region State
    public override void SetState()
    {
        base.SetState();
        StateSpecial();
        MyControllerManager.SetGravity(0);
    }
    public override void StateSpecial()
    {
        SetIsDash(false);
        SetCanMove(true);
        SetCanControl(true);
        SetDashTime(0);
        myJumpTimeNext = 0;
        isJump = false;
        myWallingSpeed = 1;

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
            SetDirection(new Vector2(DirX * myWallingSpeed, DirY * myWallingSpeed));
            MyControllerManager.SetFloatAnimatiorLeg("DirX", Mathf.Abs((DirX + DirY) * myWallingSpeed));
            if (isJump)
            {
                JumpCoolDown();
                SetDirection(MyDirection * myJumpSpeed);
            }
            //else
            //{
            //    if (Input.GetKeyUp(KeyCode.Space))
            //    {
            //        // Jump yapacak
            //        Jump();
            //    }
            //    if (Input.GetKeyUp(KeyCode.V))
            //    {
            //        // Dash yapacak
            //        Dash();
            //    }
            //    else if (Input.GetKeyUp(KeyCode.B))
            //    {
            //        // Bomb atacak
            //        MyControllerManager.SetTriggerAnimatiorLeg("Bomb");
            //    }
            //}
        }
    }
    #endregion

    #region Dropping
    public override void Dropping()
    {
        MyControllerManager.SetState(StateType.Falling);
        State_Falling state_Falling = MyControllerManager.myState as State_Falling;
        if (state_Falling is not null)
        {
            if (transform.eulerAngles.y == 0)
            {
                state_Falling.SetFalling(-1);
            }
            else
            {
                state_Falling.SetFalling(1);
            }
        }
    }
    #endregion

    #region Dash
    public override void Dash()
    {
        SetIsDash(true);
        SetCanControl(false);
        SetDashTime(MyDashTime);
        SetDirection(new Vector2(0, MyDashSpeed));
        MyControllerManager.SetTriggerAnimatiorLeg("Dash");
    }
    public override void OnDashCoolDown()
    {
        StateSpecial();
    }
    #endregion

    #region Jump
    public override void Jump()
    {
        isJump = true;
        myJumpTimeNext = myJumpTime;
    }
    public void JumpCoolDown()
    {
        myJumpTimeNext -= Time.deltaTime;
        if (myJumpTimeNext <= 0)
        {
            StateSpecial();
        }
    }
    #endregion
}