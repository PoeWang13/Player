using UnityEngine;

public class State_Roping_Right : State
{
    [Header("Roping")]
    [SerializeField] private float myRopingSpeed = 1;

    private bool somersault; // Takla atmak

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

    #region Takla
    public void TaklaFinish()
    {
        TaklaDurum(false);
    }
    private void TaklaDurum(bool isStart)
    {
        somersault = isStart;
        MyControllerManager.SetCollider(!isStart);
    }
    public bool GetSomersault()
    {
        return somersault;
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
        SetIsDash(false);
        SetCanMove(true);
        SetCanControl(true);
        TaklaDurum(false);
        SetDashTime(0);
        MyControllerManager.SetGravity(0);
    }
    public override void StateSpecial()
    {
        TaklaDurum(false);
    }
    public override void StopState()
    {
        isActive = false;
        MyControllerManager.SetGravity(1);
    }
    public override void UpdateState()
    {
       if (DirY == -1)
        {
            MyControllerManager.SetGravity(1);
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
        if (DirX == 1)
        {
            MyControllerManager.SetAngleY(0);
        }
        else if (DirX == -1)
        {
            MyControllerManager.SetAngleY(180);
        }
        SetDirection(new Vector2(DirX * myRopingSpeed, 0));
        MyControllerManager.SetFloatAnimatiorLeg("DirX", Mathf.Abs(DirX * myRopingSpeed));
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Jumpa geç
            // İpin etrafında takla atlayacak - Sunset Riders daki gibi
            TaklaDurum(true);
            MyControllerManager.SetTriggerAnimatiorLeg("Dash");
        }
        else if (Input.GetKeyUp(KeyCode.B))
        {
            // Bomb atacak
            MyControllerManager.SetTriggerAnimatiorLeg("Bomb");
        }
    }
    #endregion
}