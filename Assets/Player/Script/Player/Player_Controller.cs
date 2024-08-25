using UnityEngine;

public class Player_Controller : Controller_Manager
{
    #region Context Menu
    //public StateType changedState;
    //[ContextMenu("State Change")]
    //private void StateChange()
    //{
    //    SetState(changedState);
    //}
    #endregion
    //private int directionCount;

    //private bool isWalking;      // Karakterin yürümesi
    //private bool isSitting;      // Karakterin oturması
    //private bool isJumping;      // Karakterin zıplaması
    //private bool isRopingUp;     // Karakterin yukarı doğru ipe tırmanması - Pang'de merdiven çıkması gibi
    //private bool isRopingRight;  // Karakterin yatay ipe tırmanması
    //private bool isWallingUp;    // Karakterin duvarda yukarı - dağa tırmanması
    //private bool isWallingRight; // Karakterin kenardan duvara tırmanması - Flingstone oyunu gibi
    //private bool isSpacingUp;    // Karakterin uzayda sırtından bakarak ilerlemesi - Metal slug 3 gibi
    //private bool isSpacingRight; // Karakterin uzayda yandan gitmesi - Three Wonders: Chariot-Lou gibi
    private static Player_Controller instance;
    public static Player_Controller Instance
    {
        get { return instance;}
    }

    private float myFireTimeNext;
    private float myBombTimeNext;

    #region Unity
    public override void OnStart()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        myBombTimeNext += Time.deltaTime;
        if (Input.GetKeyUp(KeyCode.Space))
        {
            myState.Jump();
        }
        else if (Input.GetKeyUp(KeyCode.V))
        {
            // Dash yapacak
            myState.Dash();
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            if (myBombTimeNext > MyBombTime)
            {
                // Bomb atacak
                Bomb();
                myBombTimeNext = 0;
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                // Fire etmeyi durdur
                FireUp();
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                // Ateş etmeye başla
                FireDown();
            }
            else if (Input.GetKey(KeyCode.F))
            {
                myFireTimeNext += Time.deltaTime;
                if (myFireTimeNext >= MyFireTime)
                {
                    // Ateş et
                    myFireTimeNext = 0;
                    Fire();
                }
            }
        }
    }
    public float directZ = 1;
    #endregion
}