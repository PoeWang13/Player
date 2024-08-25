using Unity.VisualScripting;
using UnityEngine;

public class State_Roping_Up : State
{
    [Header("Roping")]
    [SerializeField] private float myRopingSpeed = 1;

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
            if (transform.eulerAngles.y == 0)
            {
                state_Falling.SetFalling(1);
            }
            else
            {
                state_Falling.SetFalling(-1);
            }
        }
    }
    #endregion

    #region State
    public override void SetState()
    {
        base.SetState();
        SetDirection(Vector2.zero);
        MyControllerManager.SetGravity(0);
    }
    public override void StopState()
    {
        isActive = false;
        MyControllerManager.SetGravity(1);
    }
    public override void StateSpecial()
    {
        SetIsDash(false);
        SetCanMove(true);
        SetCanControl(true);
        SetDashTime(0);
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
            SetDirection(new Vector2(0, DirY * myRopingSpeed));
            MyControllerManager.SetFloatAnimatiorLeg("DirX", Mathf.Abs(DirY * myRopingSpeed));
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (DirX != 0)
                {
                    // Jumpa geç
                    MyControllerManager.SetState(StateType.Jumping);
                    // Jumpla
                    State_Jumping state_Jumping = MyControllerManager.myState as State_Jumping;
                    state_Jumping.RopingUpJump();
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