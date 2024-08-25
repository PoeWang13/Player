using UnityEngine;

public class Wall_Up : Wall
{
    public override void SetWall(Controller_Manager controller)
    {
        //controller.SetState(MyStateType);
    }
    public override void ExitWalling(Controller_Manager controller)
    {
        State_Walling_Up walling_Up = controller.GetState(MyStateType) as State_Walling_Up;
        if (walling_Up.DirY == -1)
        {
            walling_Up.Dropping();
        }
    }
}