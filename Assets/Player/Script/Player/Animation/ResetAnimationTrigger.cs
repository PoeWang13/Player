using UnityEngine;

public class ResetAnimationTrigger : StateMachineBehaviour
{
    [SerializeField] public string myAnim;
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(myAnim);
    }
}
