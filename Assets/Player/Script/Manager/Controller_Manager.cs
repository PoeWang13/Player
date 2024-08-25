using UnityEngine;
using System.Collections.Generic;

public enum StateType
{
    None = 99,
    Walking = 0,
    Sitting = 1,
    Jumping = 2,
    Falling = 3,
    Gravity = 4,
    RopingRight = 5,
    RopingUp = 6,
    SpacingRight = 7,
    SpacingUp = 8,
    WallingRight = 9,
    WallingUp = 10,
}
[System.Serializable]
public class AllState
{
    public State myState;
    public StateType stateType;

    public AllState(State myState, StateType stateType)
    {
        this.myState = myState;
        this.stateType = stateType;
    }
}
[System.Serializable]
public class ScreenLimit
{
    public float minLimit;
    public float maxLimit;
    public ScreenLimit(float min, float max)
    {
        minLimit = min;
        maxLimit = max;
    }
}
public class Controller_Manager : MonoBehaviour
{
    //public State myState;
    [HideInInspector] public State myState;
    [HideInInspector] public List<AllState> allStates = new List<AllState>();

    [SerializeField] private int knifeLimit = 2;
    [SerializeField] private float myFireTime = 1;
    [SerializeField] private float myBombTime = 1;

    [SerializeField] private int myBombLimit = 1;

    [SerializeField] private Bomb myBomb;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Knife myKnife;
    [SerializeField] private Bullet myBullet;
    [SerializeField] private Transform myBombPoint;
    [SerializeField] private Transform myKnifePoint;
    [SerializeField] private Transform myBulletPoint;

    [SerializeField] private List<string> allEnemiesLayerName = new List<string>();

    private int grondMask;
    //private int enemyMask;
    private Transform myPlayerView;
    private Animator myAnimatorLeg;
    private Animator myAnimatorBody;
    private Rigidbody2D myRigidbody;
    private BoxCollider2D myBoxCollider;

    public int GrondMask { get { return grondMask; } }
    public int EnemyMask { get { return enemyMask; } }
    public float MyFireTime { get { return myFireTime; } }
    public float MyBombTime { get { return myBombTime; } }
    public Rigidbody2D MyRigidbody { get { return myRigidbody; } }

    #region Unity
    private void Awake()
    {
        grondMask = LayerMask.GetMask("Ground");
        //enemyMask = LayerMask.GetMask(allEnemiesLayerName.ToArray());
        myPlayerView = transform.Find("PlayerView");
        myAnimatorLeg = myPlayerView.Find("PlayerLegView").GetComponent<Animator>();
        myAnimatorBody = myPlayerView.Find("PlayerBodyView").GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        State[] player_States = GetComponents<State>();
        for (int i = 0; i < player_States.Length; i++)
        {
            allStates.Add(new AllState(player_States[i], player_States[i].MyStateType));
        }
    }
    private void Start()
    {
        OnStart();

        myState = allStates[(int)StateType.Falling].myState;
        myState.SetState();
    }
    public virtual void OnStart()
    {
    }
    #endregion

    #region Set - Get
    public void SetState(StateType stateType)
    {
        myState.StopState();
        for (int e = 0; e < allStates.Count; e++)
        {
            if (stateType == allStates[e].stateType)
            {
                allStates[e].myState.SetState();
                myState = allStates[e].myState;
                return;
            }
        }
    }
    public bool CheckState(StateType stateType)
    {
        return stateType == myState.MyStateType;
    }
    public State GetState(StateType stateType)
    {
        for (int e = 0; e < allStates.Count; e++)
        {
            if (stateType == allStates[e].stateType)
            {
                return allStates[e].myState;
            }
        }
        return null;
    }
    public void SetBoxColliderSize(Vector2 size)
    {
        myBoxCollider.size = size;
    }
    public void SetBoxColliderOffSet(Vector2 offSet)
    {
        myBoxCollider.offset = offSet;
    }

    public void SetAngleY(float angle)
    {
        myPlayerView.eulerAngles = new Vector3(0, angle, myPlayerView.eulerAngles.z);
    }
    public void SetGravity(float gravity)
    {
        myRigidbody.gravityScale = gravity;
    }
    public void SetCollider(bool isEnable)
    {
        myBoxCollider.enabled = isEnable;
    }
    #endregion

    #region Animator Leg
    public void SetAnimationAnimatiorLeg(string animName)
    {
        myAnimatorLeg.Play(animName);
    }
    public void SetIntegerAnimatiorLeg(string animName, int animValue)
    {
        myAnimatorLeg.SetInteger(animName, animValue);
    }
    public void SetFloatAnimatiorLeg(string animName, float animValue)
    {
        myAnimatorLeg.SetFloat(animName, animValue);
    }
    public void SetTriggerAnimatiorLeg(string animName)
    {
        myAnimatorLeg.SetTrigger(animName);
    }
    public void SetBoolAnimatiorLeg(string animName, bool isTrue)
    {
        myAnimatorLeg.SetBool(animName, isTrue);
    }
    public void SetAnimationAnimatorLeg(Vector2 animName)
    {
        //myAnimator.Play(animName);
    }
    public bool GetAnimationAnimatorLeg(string animName)
    {
        return myAnimatorLeg.GetCurrentAnimatorStateInfo(0).IsName(animName);
    }
    #endregion

    #region Animator Body
    public void SetAnimationAnimatiorBody(string animName)
    {
        myAnimatorBody.Play(animName);
    }
    public void SetIntegerAnimatiorBody(string animName, int animValue)
    {
        myAnimatorBody.SetInteger(animName, animValue);
    }
    public void SetFloatAnimatiorBody(string animName, float animValue)
    {
        myAnimatorBody.SetFloat(animName, animValue);
    }
    public void SetTriggerAnimatiorBody(string animName)
    {
        myAnimatorBody.SetTrigger(animName);
    }
    public void SetBoolAnimatiorBody(string animName, bool isTrue)
    {
        myAnimatorBody.SetBool(animName, isTrue);
    }
    public void SetAnimationAnimatorBody(Vector2 animName)
    {
        //myAnimator.Play(animName);
    }
    public bool GetAnimationAnimatorBody(string animName)
    {
        return myAnimatorBody.GetCurrentAnimatorStateInfo(0).IsName(animName);
    }
    #endregion

    #region Genel
    public void CreateBullet(string owner, string enemy, Vector2 exitPoint, Vector2 direction)
    {
        Bullet bullet = Instantiate(myBullet, exitPoint, Quaternion.identity);
        bullet.SetBullet(owner, enemy, direction);
    }
    public void CreateKnife(string owner, string enemy, Vector2 exitPoint, float angle)
    {
        Knife knife = Instantiate(myKnife, exitPoint, Quaternion.identity);
        knife.SetKnife(owner, enemy, angle);
        SetTriggerAnimatiorBody("Knife");
    }
    public void ThrowBomb(string owner, string enemy, Vector2 exitPoint)
    {
        Bomb bomb = Instantiate(myBomb, exitPoint, Quaternion.identity);
        bomb.SetBomb(owner, enemy, myPlayerView.eulerAngles.y);
        SetTriggerAnimatiorBody("Bomb");
    }
    #endregion

    #region Bomb
    public virtual void Bomb()
    {
        if (myBombLimit > 0)
        {
            // Bomb atacak
            ThrowBomb("Player", "Enemy", myBombPoint.position);
            myBombLimit--;
        }
    }
    #endregion

    #region Fire
    public virtual void FireUp()
    {
        // Ateş etmeyi durdur
        SetBoolAnimatiorBody("Fire", false);
    }
    public virtual void FireDown()
    {
        SetBoolAnimatiorBody("Fire", true);
    }
    public virtual void Fire()
    {
        int direcX = myState.DirX;
        // Kurşunu gönder
        if (myState.DirY == 0)
        {
            if (myPlayerView.eulerAngles.y == 0)
            {
                direcX = 1;
            }
            else
            {
                direcX = -1;
            }
        }
        Debug.DrawLine(transform.position + Vector3.up * 2 + Vector3.right * 0.495f * direcX, transform.position + Vector3.up * 2 + Vector3.right * 2 * direcX, Color.green);
        RaycastHit2D enemyHit = Physics2D.Raycast(transform.position + Vector3.up * 2 + Vector3.right * 0.495f * direcX, Vector2.right * direcX, knifeLimit, enemyMask);
        if (enemyHit)
        {
            // Önünde düşman var bıçakla
            CreateKnife("Player", "Enemy", myKnifePoint.position, transform.eulerAngles.y);
        }
        else
        {
            CreateBullet("Player", "Enemy", myBulletPoint.position, new Vector2(direcX, myState.DirY));
        }
    }
    #endregion
}