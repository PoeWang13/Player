using UnityEngine;

public class State_Walking : State
{
    [Header("Slope")]
    [SerializeField] private int maxSlope = 45;

    [Header("Walking")]
    [SerializeField] private float myWalkingSpeed = 5;

    private float moveX;
    private float moveY;
    private float slopeAngle;
    private float slopeSpeed = 10;

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
        if (myElevator is null)
        {
            SetDirection(new Vector2(moveX, moveY));
        }
        else
        {
            SetDirection((new Vector2(moveX, 0) + myElevator.MyRigidbody.velocity));
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
        else if (collision.transform.CompareTag("Gravity"))
        {
            myElevator = null;
            MyControllerManager.SetState(StateType.Gravity);
        }
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
            if (DirY == -1)
            {
                MyControllerManager.SetState(StateType.Sitting);
            }
            RaycastHit2D groundLeft = Physics2D.Raycast(transform.position + Vector3.left * 0.495f, Vector2.down, 0.1f, MyControllerManager.GrondMask);
            Debug.DrawLine(transform.position + Vector3.left * 0.495f, transform.position + Vector3.left * 0.495f + Vector3.down * 0.1f, Color.blue);
            RaycastHit2D groundCenter = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, MyControllerManager.GrondMask);
            RaycastHit2D groundRight = Physics2D.Raycast(transform.position + Vector3.right * 0.495f, Vector2.down, 0.1f, MyControllerManager.GrondMask);
            Debug.DrawLine(transform.position + Vector3.right * 0.495f, transform.position + Vector3.right * 0.495f + Vector3.down * 0.1f, Color.red);

            moveX = DirX * myWalkingSpeed;
            moveY = 0;
            if (DirX == 1)
            {
                MyControllerManager.SetAngleY(0);
                if (groundRight)
                {
                    SetSlopeSameDirection(groundRight.normal);
                }
                else if (groundLeft)
                {
                    SetSlopeDifferentDirection(groundLeft.normal);
                }
                else if (!groundCenter)
                {
                    Dropping();
                }
            }
            else if (DirX == -1)
            {
                MyControllerManager.SetAngleY(180);
                if (groundLeft)
                {
                    SetSlopeSameDirection(groundLeft.normal);
                }
                else if (groundRight)
                {
                    SetSlopeDifferentDirection(groundRight.normal);
                }
                else if (!groundCenter)
                {
                    Dropping();
                }
            }
            else if (DirX == 0)
            {
                if (transform.eulerAngles.y == 0)
                {
                    if (groundRight)
                    {
                        slopeAngle = Vector2.Angle(groundRight.normal, Vector2.up);
                        if (slopeAngle != 0)
                        {
                            if (Mathf.Abs(slopeAngle) > maxSlope)
                            {
                                moveY = -Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * slopeSpeed;
                                moveX = -Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * slopeSpeed * Mathf.Sign(moveX);
                            }
                        }
                    }
                    else if (!groundCenter)
                    {
                        if (groundLeft)
                        {
                            // Bir ayağı yerde düşüyormuş gibi yap.
                            //MyControllerManager.SetFloatAnimation("Speed", Mathf.Abs(moveX));
                            // Bool true olursa düşme animi başlar ve animde kalır, false olursa geri gider
                        }
                        else
                        {
                            Dropping();
                        }
                    }
                }
                else
                {
                    if (groundLeft)
                    {
                        slopeAngle = Vector2.Angle(groundLeft.normal, Vector2.up);
                        if (slopeAngle != 0)
                        {
                            if (Mathf.Abs(slopeAngle) > maxSlope)
                            {
                                moveY = -Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * slopeSpeed;
                                moveX = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * slopeSpeed * Mathf.Sign(moveX);
                            }
                        }
                    }
                    else if (!groundCenter)
                    {
                        if (groundRight)
                        {
                            // Bir ayağı yerde düşüyormuş gibi yap.
                            //MyControllerManager.SetFloatAnimation("Speed", Mathf.Abs(moveX));
                            // Bool true olursa düşme animi başlar ve animde kalır, false olursa geri gider
                        }
                        else
                        {
                            Dropping();
                        }
                    }
                }
            }
            if (Input.GetKey(KeyCode.C))
            {
                myWalkingSpeed = 10;
            }
            else
            {
                myWalkingSpeed = 5;
            }
            MyControllerManager.SetFloatAnimatiorBody("DirX", DirX);
            MyControllerManager.SetFloatAnimatiorBody("DirY", DirY);
            MyControllerManager.SetFloatAnimatiorLeg("DirX", Mathf.Abs(DirX));
            //if (Input.GetKeyUp(KeyCode.Space) && slopeAngle <= maxSlope)
            //{
            //    // Jumpa geç
            //    MyControllerManager.SetState(StateType.Jumping);
            //    // Jumpla
            //    State_Jumping state_Jumping = MyControllerManager.myState as State_Jumping;
            //    if (state_Jumping is not null)
            //    {
            //        state_Jumping.WalkingJump();
            //    }
            //}
            //else if (Input.GetKeyUp(KeyCode.V))
            //{
            //    // Dash yapacak
            //    Dash();
            //}
            //if (Input.GetKeyUp(KeyCode.B))
            //{
            //    // Bomb atacak
            //    MyControllerManager.ThrowBomb("Player", "Enemy", MyControllerManager.MyBombPoint.position);
            //}
            //else
            //{
            //    if (Input.GetKeyUp(KeyCode.F))
            //    {
            //        // Ateş etmeyi durdur
            //        MyControllerManager.SetBoolAnimatiorBody("Fire", false);
            //    }
            //    else if (Input.GetKeyDown(KeyCode.F))
            //    {
            //        // Ateş etmeye başla
            //        MyControllerManager.SetBoolAnimatiorBody("Fire", true);
            //    }
            //    else if (Input.GetKey(KeyCode.F))
            //    {
            //        myFireTimeNext += Time.deltaTime;
            //        if (myFireTimeNext >= MyControllerManager.MyFireTime)
            //        {
            //            myFireTimeNext = 0;
            //            int direcX = DirX;
            //            // Kurşunu gönder
            //            if (DirY == 0)
            //            {
            //                if (MyControllerManager.MyPlayerView.eulerAngles.y == 0)
            //                {
            //                    direcX = 1;
            //                }
            //                else
            //                {
            //                    direcX = -1;
            //                }
            //            }
            //            Debug.DrawLine(transform.position + Vector3.up * 2 + Vector3.right * 0.495f, transform.position + Vector3.up * 2 + Vector3.right * 2 * direcX, Color.green);
            //            RaycastHit2D enemyHit = Physics2D.Raycast(transform.position + Vector3.up * 2 + Vector3.right * 0.495f, Vector2.right * direcX, 1.0f, enemyMask);
            //            if (enemyHit)
            //            {
            //                // Önünde düşman var bıçakla
            //                MyControllerManager.CreateKnife("Player", "Enemy", MyControllerManager.MyBombPoint.position, transform.eulerAngles.y);
            //            }
            //            else
            //            {
            //                MyControllerManager.CreateBullet("Player", "Enemy", MyControllerManager.MyBulletPoint.position, new Vector2(direcX, DirY));
            //            }
            //        }
            //    }
            //}
        }
    }
    private void SetSlopeSameDirection(Vector2 normal)
    {
        slopeAngle = Vector2.Angle(normal, Vector2.up);
        if (slopeAngle != 0)
        {
            if (Mathf.Abs(slopeAngle) <= maxSlope)
            {
                float moveDis = Mathf.Abs(moveX);
                moveY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDis;
                moveX = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDis * Mathf.Sign(moveX);
            }
            else
            {
                moveY = -Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * slopeSpeed;
                moveX = -Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * slopeSpeed * Mathf.Sign(moveX);
            }
        }
    }
    private void SetSlopeDifferentDirection(Vector2 normal)
    {
        slopeAngle = Vector2.Angle(normal, Vector2.up);
        if (slopeAngle != 0)
        {
            if (Mathf.Abs(slopeAngle) <= maxSlope)
            {
                float moveDis = Mathf.Abs(moveX);
                moveY = -Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDis;
                moveX = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDis * Mathf.Sign(moveX);
            }
            else
            {
                moveY = -Mathf.Sin(Mathf.Abs(slopeAngle) * Mathf.Deg2Rad) * slopeSpeed;
                moveX = Mathf.Cos(Mathf.Abs(slopeAngle) * Mathf.Deg2Rad) * slopeSpeed * Mathf.Sign(moveX);
            }
        }
    }
    #endregion

    #region Jump
    public override void Jump()
    {
        if (slopeAngle <= maxSlope)
        {
            // Jumpa geç
            MyControllerManager.SetState(StateType.Jumping);
            // Jumpla
            State_Jumping state_Jumping = MyControllerManager.myState as State_Jumping;
            if (state_Jumping is not null)
            {
                state_Jumping.WalkingJump();
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
        MyControllerManager.SetGravity(0);
        if (transform.eulerAngles.y == 0)
        {
            moveX = MyDashSpeed;
        }
        else
        {
            moveX = -MyDashSpeed;
        }
        MyControllerManager.SetTriggerAnimatiorLeg("Dash");
    }
    public override void OnDashCoolDown()
    {
        StateSpecial();
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
                state_Falling.SetFalling(1);
            }
            else
            {
                state_Falling.SetFalling(-1);
            }
        }
    }
    #endregion
}