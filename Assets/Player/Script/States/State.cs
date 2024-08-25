using UnityEngine;

public class State : MonoBehaviour
{
    [HideInInspector] public bool isActive;
    [HideInInspector] public float myFireTimeNext;

    [Header("Genel")]
    [SerializeField] private StateType myStateType;
    [SerializeField] private Vector2 mySize;
    [SerializeField] private Vector2 myOffSet;

    [Header("Dash")]
    [SerializeField] private float myDashTime = 1;
    [SerializeField] private float myDashSpeed = 5;

    private int dirX;
    private int dirY;

    private float myDashTimeNext;

    private bool isDash = false;
    private bool canMove = true;
    private bool canControl = true;

    private Vector2 myDirection;
    private Rigidbody2D myRigidbody;
    private Controller_Manager myControllerManager;

    public Controller_Manager MyControllerManager { get { return myControllerManager; } }
    public int DirX { get { return dirX; } }
    public int DirY { get { return dirY; } }
    public bool IsDash { get { return isDash; } }
    public bool CanMove { get { return canMove; } }
    public bool CanControl { get { return canControl; } }
    public float MyDashTime { get { return myDashTime; } }
    public float MyDashSpeed { get { return myDashSpeed; } }
    public Vector2 MyDirection { get { return myDirection; } }
    public Rigidbody2D MyRigidbody { get { return myRigidbody; } }
    public StateType MyStateType { get { return myStateType; } }

    #region Unity
    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myControllerManager = GetComponent<Controller_Manager>();
        OnAwake();
    }
    public virtual void OnAwake()
    {
    }
    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        dirX = 0;
        dirY = 0;
        if (Input.GetKey(KeyCode.W))
        {
            // Yukarı yönlendir
            dirY = +1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            // Sağa yönlendir
            dirX = +1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            // Aşağı yönlendir
            dirY = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            // Sola yönlendir
            dirX = -1;
        }
        UpdateState();
    }
    #endregion

    #region State
    public virtual void SetState()
    {
        isActive = true;
        Player_Controller.Instance.SetBoxColliderSize(mySize);
        Player_Controller.Instance.SetBoxColliderOffSet(myOffSet);
        Player_Controller.Instance.SetIntegerAnimatiorBody("Layer", (int)myStateType);
        Player_Controller.Instance.SetIntegerAnimatiorLeg("Layer", (int)myStateType);
    }
    public virtual void StateSpecial()
    {
    }
    public virtual void StopState()
    {
        isActive = false;
    }
    public virtual void UpdateState()
    {
    }
    #endregion

    #region Set
    public void SetDirX(int newDirX)
    {
        dirX = newDirX;
    }
    public void SetDirY(int newDirY)
    {
        dirY = newDirY;
    }
    public void SetIsDash(bool dash)
    {
        isDash = dash;
    }
    public void SetCanMove(bool move)
    {
        canMove = move;
    }
    public void SetCanControl(bool control)
    {
        canControl = control;
    }
    public void SetDirection(Vector2 direc)
    {
        myDirection = direc;
    }
    public void SetDashTime(float time)
    {
        myDashTimeNext = time;
    }
    public void DashCoolDown()
    {
        myDashTimeNext -= Time.deltaTime;
        if (myDashTimeNext <= 0)
        {
            OnDashCoolDown();
        }
    }
    #endregion

    #region Jump
    public virtual void Jump()
    {
    }
    #endregion

    #region Dash
    public virtual void Dash()
    {
    }
    public virtual void OnDashCoolDown()
    {
    }
    #endregion

    #region Dropping
    public virtual void Dropping()
    {
    }
    #endregion
}