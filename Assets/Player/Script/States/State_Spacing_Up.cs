using UnityEngine;

public class State_Spacing_Up : State
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
        SetDashTime(0);
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
            SetDirection(new Vector2(DirX * mySpaceSpeed, DirY * mySpaceSpeed));
            MyControllerManager.SetFloatAnimatiorLeg("DirX", Mathf.Abs(DirX * mySpaceSpeed));
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
        SetDirection(new Vector2(0, MyDashSpeed));
        MyControllerManager.SetTriggerAnimatiorLeg("Dash");
    }
    public override void OnDashCoolDown()
    {
        StateSpecial();
    }
    #endregion
}