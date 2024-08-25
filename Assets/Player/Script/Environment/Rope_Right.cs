using UnityEngine;

public class Rope_Right : Rope
{
    public override void CheckDistance(int order)
    {
        if (Mathf.Abs(transform.position.y - MyRopedList[order].transform.position.y) < 0.1f)
        {
            MyRopedList[order].SetState(MyStateType);
            MyRopedList.RemoveAt(order);
            CheckRopeList();
        }
    }
    public override void ExitRope(Controller_Manager controller)
    {
        State_Roping_Right roping_Right = controller.GetState(MyStateType) as State_Roping_Right;
        if (!roping_Right.GetSomersault())
        {
            roping_Right.Dropping();
        }
    }
}