using UnityEngine;

public class NotifyAnimatorStateChange : StateMachineBehaviour
{
    [SerializeField] private string animatorState = "IdleIn";
    private AnimatorStateObserver animatorStateObserver;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animatorStateObserver == null)
        {
            animatorStateObserver = animator.GetComponent<AnimatorStateObserver>();
        }
        if (animatorStateObserver != null)
        {
            animatorStateObserver.SignalHeartbeat(animatorState, stateInfo.shortNameHash);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animatorStateObserver == null)
        {
            animatorStateObserver = animator.GetComponent<AnimatorStateObserver>();
        }
        if (animatorStateObserver != null)
        {
            animatorStateObserver.SignalHeartbeat(animatorState, stateInfo.shortNameHash);
        }
    }
}