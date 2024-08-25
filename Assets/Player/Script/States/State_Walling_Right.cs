using UnityEngine;

public class State_Walling_Right : State
{
    [Header("Jump")]
    [SerializeField] private float myJumpSpeed = 5;

    [Header("Walling")]
    [SerializeField] private float myWallingSpeed = 1;

    private float jumpSpeed;

    private bool isWallRightSide = false;

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

    #region Dropping
    public void Dropping()
    {
        MyControllerManager.SetState(StateType.Falling);
        State_Falling state_Falling = MyControllerManager.myState as State_Falling;
        if (state_Falling is not null)
        {
            if (isWallRightSide)
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
    }
    public void SetWallSide(bool wallRightSide)
    {
        isWallRightSide = wallRightSide;
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
            if (DirX == 1)
            {
                MyControllerManager.SetAngleY(0);
            }
            else if (DirX == -1)
            {
                MyControllerManager.SetAngleY(180);
            }
            SetDirection(new Vector2(0, DirY * myWallingSpeed));
            MyControllerManager.SetFloatAnimatiorLeg("DirX", Mathf.Abs(DirY * myWallingSpeed));
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (isWallRightSide) // Duvar sağda
                {
                    if (transform.eulerAngles.y == 0) // Adam sağa bakıyır
                    {
                        jumpSpeed = -1;
                        MyControllerManager.SetState(StateType.Falling);

                        State_Falling state_Falling = MyControllerManager.myState as State_Falling;
                        if (state_Falling is not null)
                        {
                            state_Falling.SetFalling(-1);
                        }
                        return;
                    }
                    else                             // Adam sola bakıyır
                    {
                        jumpSpeed = myJumpSpeed;
                        if (DirX == -1)    // Adam sola yönleniyor
                        {
                            jumpSpeed *= 2;
                        }
                    }
                }
                else // Duvar solda
                {
                    if (transform.eulerAngles.y == 0) // Adam sağa bakıyır
                    {
                        jumpSpeed = myJumpSpeed;
                        if (DirX == 1)    // Adam sağa yönleniyor
                        {
                            jumpSpeed *= 2;
                        }
                    }
                    else                             // Adam sola bakıyır
                    {
                        jumpSpeed = -1;
                        MyControllerManager.SetState(StateType.Falling);
                        State_Falling state_Falling = MyControllerManager.myState as State_Falling;
                        if (state_Falling is not null)
                        {
                            state_Falling.SetFalling(1);
                        }
                        return;
                    }
                }
                // Jumpla
                MyControllerManager.SetState(StateType.Jumping);
                State_Jumping state_Jumping = MyControllerManager.myState as State_Jumping;
                if (state_Jumping is not null)
                {
                    state_Jumping.WallingRightJump(jumpSpeed);
                }
            }
            else if (Input.GetKeyUp(KeyCode.V))
            {
                // Dash yapacak
                Dash();
            }
            else if (Input.GetKeyUp(KeyCode.B))
            {
                // Bomb atacak
                MyControllerManager.SetTriggerAnimatiorLeg("Bomb");
            }
        }
    }
    #endregion

    #region Dash
    public void Dash()
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
}