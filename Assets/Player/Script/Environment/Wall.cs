using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private StateType myStateType;

    public StateType MyStateType { get { return myStateType; } }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent(out Controller_Manager controller))
            {
                SetWall(controller);
                controller.SetState(myStateType);
            }
        }
    }
    public virtual void SetWall(Controller_Manager controller)
    {

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent(out Controller_Manager controller))
            {
                ExitWalling(controller);
            }
        }
    }
    public virtual void ExitWalling(Controller_Manager controller)
    {

    }
}