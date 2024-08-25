using UnityEngine;

public class State_Spacing_Right : State
{
    [Header("Spacing")]
    [SerializeField] private float mySpaceSpeed = 1;

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
            MyControllerManager.SetFloatAnimatiorLeg("DirX", Mathf.Abs(DirY * mySpaceSpeed));
            if (Input.GetKeyUp(KeyCode.V))
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