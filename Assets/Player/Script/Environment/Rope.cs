using UnityEngine;
using System.Collections.Generic;

public class Rope : MonoBehaviour
{
    [SerializeField] private StateType myStateType;

    private bool myRoped;
    private List<Controller_Manager> myRopedList = new List<Controller_Manager>();

    public StateType MyStateType { get { return myStateType; } }
    public List<Controller_Manager> MyRopedList { get { return myRopedList; } }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            myRoped = true;
            if (collision.TryGetComponent(out Controller_Manager controller))
            {
                if (!myRopedList.Contains(controller))
                {
                    myRopedList.Add(controller);
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out Controller_Manager controller))
            {
                ExitRope(controller);
            }
        }
    }
    public void CheckRopeList()
    {
        if (myRopedList.Count == 0)
        {
            myRoped = false;
        }
    }
    private void Update()
    {
        if (myRoped)
        {
            for (int e = 0; e < myRopedList.Count; e++)
            {
                if (myRopedList[e].CheckState(myStateType))
                {
                    continue;
                }
                CheckDistance(e);
            }
        }
    }
    public virtual void CheckDistance(int order)
    {

    }
    public virtual void ExitRope(Controller_Manager controller)
    {

    }
}