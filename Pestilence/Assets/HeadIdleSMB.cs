using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HeadIdleSMB : StateMachineBehaviour
{
    [SerializeField, Range(0f, 1f)] private float normalizedTriggerTime;
    private bool _wasTriggered;

    public event Action Entered;
    public event Action Triggered;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Entered?.Invoke();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_wasTriggered)
            return;

        if (stateInfo.normalizedTime < normalizedTriggerTime)
            return;

        Triggered?.Invoke();
        _wasTriggered = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _wasTriggered = false;
    }
}
