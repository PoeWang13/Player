using UnityEngine;

public class State_Jumping : State
{
    [Header("Jump")]
    [SerializeField] private int myMaxJumping = 1;
    [SerializeField] private float myJumpPowerX = 5;
    [SerializeField] private float myJumpPowerY = 5;

    private bool myJumped;

    private int myJumpCount;

    private float jumpPowerX;
    private float myJumpTime;
    private float myControlTime;

    private Elevator myElevator;

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
        // Check Ground
        if (myJumped)
        {
            myJumpTime += Time.deltaTime;
            if (myJumpTime > 0.5f)
            {
                bool isGroundLeft = Physics2D.Raycast(transform.position + Vector3.left * 0.45f, Vector2.down, 0.1f, MyControllerManager.GrondMask);
                bool isGroundCenter = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, MyControllerManager.GrondMask);
                bool isGroundRight = Physics2D.Raycast(transform.position + Vector3.right * 0.45f, Vector2.down, 0.1f, MyControllerManager.GrondMask);
                JumpReset(isGroundLeft, isGroundCenter, isGroundRight);
            }
        }
        MyRigidbody.velocity = MyDirection;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Elevator"))
        {
            if (collision.transform.TryGetComponent(out Elevator elevator))
            {
                myElevator = elevator;
            }
        }
    }
    #endregion

    #region State
    public override void SetState()
    {
        base.SetState();
        MyControllerManager.SetGravity(1);
        StateSpecial();
    }
    public override void StateSpecial()
    {
        SetIsDash(false);
        SetCanMove(true);
        SetCanControl(true);
        SetDashTime(0);
        myJumped = false;
        myJumpTime = 0;
        myJumpCount = 0;
    }
    public override void StopState()
    {
        isActive = false;
        StateSpecial();
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
                myControlTime -= Time.deltaTime;
                if (myControlTime < 0)
                {
                    SetCanControl(true);
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
            SetDirection(new Vector2(DirX * jumpPowerX, MyRigidbody.velocity.y));
            MyControllerManager.SetFloatAnimatiorLeg("DirX", Mathf.Abs(DirX * jumpPowerX));
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    if (myMaxJumping > myJumpCount)
            //    {
            //        JumpingJump();
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

    #region State Jumps
    public void WalkingJump()
    {
        myJumpCount++;
        myJumped = true;
        jumpPowerX = myJumpPowerX;
        SetDirection(Vector2.zero);
        MyRigidbody.velocity = MyDirection;
        if (myElevator is null)
        {
            SetDirection(new Vector2(DirX * jumpPowerX, myJumpPowerY));
        }
        else
        {
            if (myElevator.MyRigidbody.velocity.y < 0)
            {
                SetDirection(new Vector2(DirX * jumpPowerX, myJumpPowerY));
            }
            else
            {
                SetDirection(new Vector2(DirX * jumpPowerX, myJumpPowerY) + myElevator.MyRigidbody.velocity);
            }
        }
        MyControllerManager.MyRigidbody.AddForce(MyDirection, ForceMode2D.Impulse);
    }
    public void SittingJump()
    {
        myJumpCount++;
        myJumped = true;
        jumpPowerX = myJumpPowerX;
        SetDirection(Vector2.zero);
        MyRigidbody.velocity = MyDirection;
        if (myElevator is null)
        {
            SetDirection(new Vector2(DirX * jumpPowerX, myJumpPowerY));
        }
        else
        {
            if (myElevator.MyRigidbody.velocity.y < 0)
            {
                SetDirection(new Vector2(DirX * jumpPowerX, myJumpPowerY));
            }
            else
            {
                SetDirection(new Vector2(DirX * jumpPowerX, myJumpPowerY) + myElevator.MyRigidbody.velocity);
            }
        }
        MyControllerManager.MyRigidbody.AddForce(MyDirection, ForceMode2D.Impulse);
    }
    public override void Jump()
    {
        if (myMaxJumping <= myJumpCount)
        {
            return;
        }
        JumpingJump();
    }
    public void JumpingJump()
    {
        myJumpCount++;
        myJumped = true;
        jumpPowerX = myJumpPowerX;
        SetDirection(new Vector2(MyRigidbody.velocity.x, 0));
        MyRigidbody.velocity = MyDirection;
        SetDirection(new Vector2(DirX * jumpPowerX, myJumpPowerY));
        MyControllerManager.MyRigidbody.AddForce(new Vector2(DirX * jumpPowerX, myJumpPowerY), ForceMode2D.Impulse);
    }
    public void RopingUpJump()
    {
        myJumpCount++;
        myJumped = true;
        MyRigidbody.velocity = Vector2.zero;
        SetDirection(new Vector2(DirX * jumpPowerX, myJumpPowerY));
        MyControllerManager.MyRigidbody.AddForce(new Vector2(DirX * jumpPowerX, myJumpPowerY), ForceMode2D.Impulse);
    }
    public void WallingRightJump(float jumpPower)
    {
        jumpPowerX = jumpPower;
        myJumpCount++;
        myJumped = true;
        SetCanControl(false);
        myControlTime = 0.5f;
        if (transform.eulerAngles.y == 0)
        {
            SetDirX(1);
        }
        else
        {
            SetDirX(-1);
        }
        MyRigidbody.velocity = Vector2.zero;
        SetDirection(new Vector2(DirX * jumpPower, 3));
        MyControllerManager.MyRigidbody.AddForce(new Vector2(DirX * jumpPower, 3), ForceMode2D.Impulse);
    }
    #endregion

    #region Jump
    public bool CanJump()
    {
        return myMaxJumping > myJumpCount;
    }
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
        myJumped = false;
        myJumpTime = 0;
        myJumpCount = 0;
        SetDirection(Vector2.zero);
        MyRigidbody.velocity = MyDirection;
    }
    public override void Dropping()
    {
        myJumped = true;
        myJumpTime = 0;
        myJumpCount = 0;
        SetDirection(Vector2.zero);
        MyRigidbody.velocity = MyDirection;
    }
    #endregion
}