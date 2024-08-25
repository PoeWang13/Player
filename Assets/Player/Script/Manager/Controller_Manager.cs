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

    [SerializeField] private float myFireTime = 1;

    [SerializeField] private Bomb myBomb;
    [SerializeField] private Knife myKnife;
    [SerializeField] private Bullet myBullet;
    [SerializeField] private Transform myBombPoint;
    [SerializeField] private Transform myBulletPoint;

    private Transform myPlayerView;
    private Animator myAnimatorLeg;
    private Animator myAnimatorBody;
    private Rigidbody2D myRigidbody;
    private BoxCollider2D myBoxCollider;

    public Bullet MyBullet { get { return myBullet; } }
    public Bomb MyBomb { get { return myBomb; } }
    public Knife MyKnife { get { return myKnife; } }
    public float MyFireTime { get { return myFireTime; } }
    public Transform MyBulletPoint { get { return myBulletPoint; } }
    public Transform MyBombPoint { get { return myBombPoint; } }
    public Transform MyPlayerView { get { return myPlayerView; } }
    public Rigidbody2D MyRigidbody { get { return myRigidbody; } }

    #region Unity
    private void Awake()
    {
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
    public void ThrowBomb(string owner, string enemy, Vector2 exitPoint)
    {
        Bomb bomb = Instantiate(MyBomb, exitPoint, Quaternion.identity);
        bomb.SetBomb(owner, enemy, myPlayerView.eulerAngles.y);
        SetTriggerAnimatiorBody("Bomb");
    }
    public void CreateBullet(string owner, string enemy, Vector2 exitPoint, Vector2 direction)
    {
        Bullet bullet = Instantiate(MyBullet, exitPoint, Quaternion.identity);
        bullet.SetBullet(owner, enemy, direction);
    }
    public void CreateKnife(string owner, string enemy, Vector2 exitPoint, float angle)
    {
        Knife knife = Instantiate(MyKnife, exitPoint, Quaternion.identity);
        knife.SetKnife(owner, enemy, angle);
        SetTriggerAnimatiorBody("Knife");
    }
    #endregion
}