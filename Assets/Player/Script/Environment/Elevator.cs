using UnityEngine;
using System.Collections.Generic;

public enum ElevatorType
{
    Circle,
    PingPong,
    Randomly
}
public class Elevator : MonoBehaviour
{
    [SerializeField] private float mySpeed = 5;
    [SerializeField] private float myStopTime = 1;
    [SerializeField] private ElevatorType myElevatorType;
    [SerializeField] private List<Vector2> myStations = new List<Vector2>();

    private int direction = 1;
    private int currentStationPoint = 0;
    private int nextCurrentStationPoint = 1;

    private float myStopTimeNext;

    private bool canMove = true;
    private bool canControl = true;

    private Vector2 myDirection;
    private Rigidbody2D myRigidbody;

    private List<Controller_Manager> controller_Managers = new List<Controller_Manager>();

    public Rigidbody2D MyRigidbody { get { return myRigidbody; } }

    #region Unity
    public void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        transform.position = myStations[0];
        myDirection = (myStations[nextCurrentStationPoint] - myStations[currentStationPoint]).normalized;
    }
    private void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }
        if (!canControl)
        {
            myStopTimeNext += Time.deltaTime;
            if (myStopTimeNext > myStopTime)
            {
                canControl = true;
                myStopTimeNext = 0;
                myRigidbody.velocity = Vector2.zero;
            }
            return;
        }
        myDirection = (myStations[nextCurrentStationPoint] - (Vector2)transform.position).normalized;
        myRigidbody.velocity = myDirection * mySpeed;
        if (Vector2.SqrMagnitude(myStations[nextCurrentStationPoint] - (Vector2)transform.position) < 0.01f)
        {
            UpdateElevator();
        }
    }
    private void UpdateElevator()
    {
        canControl = false;
        myRigidbody.velocity = Vector2.zero;
        transform.position = myStations[nextCurrentStationPoint];
        if (myElevatorType == ElevatorType.Circle)
        {
            if (nextCurrentStationPoint == myStations.Count - 1)
            {
                nextCurrentStationPoint = 0;
            }
            nextCurrentStationPoint += direction;
            currentStationPoint = nextCurrentStationPoint - direction;
        }
        else if (myElevatorType == ElevatorType.PingPong)
        {
            if (nextCurrentStationPoint == myStations.Count - 1)
            {
                direction = -1;
            }
            else if (nextCurrentStationPoint == 0)
            {
                direction = 1;
            }
            nextCurrentStationPoint += direction;
            currentStationPoint = nextCurrentStationPoint - direction;
        }
        else if (myElevatorType == ElevatorType.Randomly)
        {
            currentStationPoint = nextCurrentStationPoint;
            nextCurrentStationPoint = Random.Range(0, myStations.Count);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out Controller_Manager controller))
        {
            if (!controller_Managers.Contains(controller))
            {
                controller_Managers.Add(controller);
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out Controller_Manager controller))
        {
            if (controller_Managers.Contains(controller))
            {
                controller_Managers.Remove(controller);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        for (int e = 0; e < myStations.Count; e++)
        {
            Debug.DrawLine(myStations[e] + Vector2.left * 0.25f, myStations[e] + Vector2.right * 0.25f, Color.white);
            Debug.DrawLine(myStations[e] + Vector2.down * 0.25f, myStations[e] + Vector2.up * 0.25f, Color.yellow);
        }
    }
    #endregion
}