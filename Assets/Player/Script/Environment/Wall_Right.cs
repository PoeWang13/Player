using UnityEngine;

public class Wall_Right : Wall
{
    public override void SetWall(Controller_Manager controller)
    {
        State_Walling_Right walling_Right = controller.GetState(MyStateType) as State_Walling_Right;
        walling_Right.SetWallSide(transform.position.x > controller.transform.position.x);
    }
    public override void ExitWalling(Controller_Manager controller)
    {
        State_Walling_Right walling_Right = controller.GetState(MyStateType) as State_Walling_Right;
        if (walling_Right.DirY == -1)
        {
            walling_Right.Dropping();
        }
    }
}