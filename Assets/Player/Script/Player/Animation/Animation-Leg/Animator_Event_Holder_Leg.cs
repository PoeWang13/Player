using UnityEngine;

public class Animator_Event_Holder_Leg : MonoBehaviour
{
    [SerializeField] private State_Roping_Right state_Roping_Right;
    public void TaklaFinish()
    {
        state_Roping_Right.TaklaFinish();
    }
}