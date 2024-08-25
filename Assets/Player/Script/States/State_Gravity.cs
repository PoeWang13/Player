using UnityEngine;

public class State_Gravity : State
{
    // Gravity 25 ve üzeri eğimli zeminlere geçişlerde hatalı geçiş yapıyor ilerde çözlecektir.
    [Header("Gravity")]
    [SerializeField] private Transform playerView;
    [SerializeField] private float myGravitySpeed = 5;

    private int gravityMask;

    private float moveX;
    private float moveY;
    private float slopeAngle;

    #region Unity
    public override void OnAwake()
    {
        gravityMask = LayerMask.GetMask("Gravity");
    }
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            Debug.Log("Ground bulduk.");
            MyControllerManager.SetState(StateType.Walking);
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
        SetIsDash(false);
        SetCanMove(true);
        SetCanControl(true);
        SetDashTime(0);
        MyControllerManager.SetGravity(0);
    }
    private RaycastHit2D groundLeft;
    private RaycastHit2D groundCenter;
    private RaycastHit2D groundRight;
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
            groundLeft = Physics2D.Raycast(transform.position + transform.up * 0.4f - transform.right * 0.495f, -transform.up, 0.5f, gravityMask);
            Debug.DrawLine(transform.position + transform.up * 0.4f - transform.right * 0.495f, transform.position - transform.right * 0.495f - transform.up * 0.1f, Color.blue);


            groundCenter = Physics2D.Raycast(transform.position + transform.up * 0.4f, -transform.up, 0.5f, gravityMask);
            Debug.DrawLine(transform.position + transform.up * 0.4f, transform.position - transform.up * 0.1f, Color.yellow);


            groundRight = Physics2D.Raycast(transform.position + transform.up * 0.4f + transform.right * 0.495f, -transform.up, 0.5f, gravityMask);
            Debug.DrawLine(transform.position + transform.up * 0.4f + transform.right * 0.495f, transform.position + transform.right * 0.495f - transform.up * 0.1f, Color.red);

            moveX = DirX;
            moveY = 0;
            if (DirX == 1)
            {
                MyControllerManager.SetAngleY(0);
                if (groundCenter)
                {
                    if (groundCenter.transform == groundRight.transform)
                    {
                        SetSlopeSameDirection(groundCenter.normal, true);
                        Debug.Log("DirX == 1 -> groundCenter : " + groundCenter.transform.name + " -> groundCenter Normal : " + groundCenter.normal + " -> groundCenter Angle : " + Vector2.Angle(groundCenter.normal, Vector2.up) + " -> groundCenter Distance : " + groundCenter.distance);
                    }
                    else if (groundCenter.transform == groundLeft.transform)
                    {
                        SetSlopeDifferentDirection(groundLeft.normal, false);
                        Debug.Log("DirX == 1 -> groundLeft : " + groundLeft.transform.name + " -> groundLeft Normal : " + groundLeft.normal + " -> groundLeft Angle : " + (-Vector2.Angle(groundCenter.normal, Vector2.up)) + " -> groundLeft Distance : " + groundLeft.distance);
                    }
                }
            }
            else if (DirX == -1)
            {
                MyControllerManager.SetAngleY(180);
                if (groundCenter)
                {
                    if (groundCenter.transform == groundLeft.transform)
                    {
                        SetSlopeSameDirection(groundCenter.normal, true);
                        Debug.Log("DirX == 1 -> groundCenter : " + groundCenter.transform.name + " -> groundCenter Normal : " + groundCenter.normal + " -> groundCenter Angle : " + Vector2.Angle(groundCenter.normal, Vector2.up) + " -> groundCenter Distance : " + groundCenter.distance);
                    }
                    else if (groundCenter.transform == groundRight.transform)
                    {
                        SetSlopeDifferentDirection(groundRight.normal, false);
                        Debug.Log("DirX == 1 -> groundRight : " + groundRight.transform.name + " -> groundRight Normal : " + groundRight.normal + " -> groundRight Angle : " + (-Vector2.Angle(groundCenter.normal, Vector2.up)) + " -> groundRight Distance : " + groundRight.distance);
                    }
                }
            }
            MyControllerManager.SetFloatAnimatiorLeg("DirX", Mathf.Abs(moveX));
            SetDirection(new Vector2(moveX, moveY) * myGravitySpeed);
            if (Input.GetKeyUp(KeyCode.Space))
            {
                // Jumpa geç
                MyControllerManager.SetState(StateType.Jumping);
                // Jumpla
                State_Jumping state_Jumping = MyControllerManager.myState as State_Jumping;
                if (state_Jumping is not null)
                {
                    state_Jumping.SittingJump();
                }
            }
            else if (Input.GetKeyUp(KeyCode.B))
            {
                // Bomb atacak
                MyControllerManager.SetTriggerAnimatiorLeg("Bomb");
            }
        }
    }
    [ContextMenu("Ground Angle")]
    private void GroundAngle()
    {
        if (groundRight)
        {
            Debug.Log("GroundRight : " + groundRight.transform.name + " -> GroundRight Normal : " + groundRight.normal + " -> GroundRight Angle : " + Vector2.Angle(groundRight.normal, Vector2.up) + " -> GroundRight Distance : " + groundRight.distance);
        }
        if (groundCenter)
        {
            Debug.Log("groundCenter : " + groundCenter.transform.name + " -> groundCenter Normal : " + groundCenter.normal + " -> groundCenter Angle : " + Vector2.Angle(groundCenter.normal, Vector2.up) + " -> groundCenter Distance : " + groundCenter.distance);
        }
        if (groundLeft)
        {
            Debug.Log("groundLeft : " + groundLeft.transform.name + " -> groundLeft Normal : " + groundLeft.normal + " -> groundLeft Angle : " + Vector2.Angle(groundLeft.normal, Vector2.up) + " -> groundLeft Distance : " + groundLeft.distance);
        }
    }
    private void SetSlopeSameDirection(Vector2 normal, bool isSameDirection)
    {
        slopeAngle = Vector2.Angle(normal, Vector2.up);
        if (slopeAngle != 0)
        {
            float moveDis = Mathf.Abs(moveX);
            moveY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDis;
            moveX = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDis * Mathf.Sign(moveX);
            SetPlayerAngle(isSameDirection ? slopeAngle : -slopeAngle);
        }
    }
    private void SetSlopeDifferentDirection(Vector2 normal, bool isSameDirection)
    {
        slopeAngle = Vector2.Angle(normal, Vector2.up);
        if (slopeAngle != 0)
        {
            float moveDis = Mathf.Abs(moveX);
            moveY = -Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDis;
            moveX = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDis * Mathf.Sign(moveX);
            SetPlayerAngle(isSameDirection ? slopeAngle : -slopeAngle);
        }
    }
    private void SetPlayerAngle(float angle)
    {
        playerView.eulerAngles = new Vector3(0, playerView.eulerAngles.y, angle);
    }
    #endregion
}