using UnityEngine;

public class Rope_Up : Rope
{
    public override void CheckDistance(int order)
    {
        if (Mathf.Abs(transform.position.x - MyRopedList[order].transform.position.x) < 0.1f)
        {
            MyRopedList[order].SetState(MyStateType);
            MyRopedList.RemoveAt(order);
            CheckRopeList();
        }
    }
    public override void ExitRope(Controller_Manager controller)
    {
        State_Roping_Up roping_Up = controller.GetState(MyStateType) as State_Roping_Up;
        roping_Up.Dropping();
    }
}