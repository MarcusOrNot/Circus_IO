using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KolobokForm : HunterVisualForm
{
    public override void SetAnimationState(HunterMovingState movingState)
    {
        switch (movingState)
        {
            case HunterMovingState.IDLE: _animator?.SetBool("Go", false); _animator?.SetBool("TurnLeft", false); _animator?.SetBool("TurnRight", false); break;
            case HunterMovingState.TURN_LEFT: _animator?.SetBool("TurnRight", false); _animator?.SetBool("TurnLeft", true); break;
            case HunterMovingState.TURN_RIGHT: _animator?.SetBool("TurnLeft", false); _animator?.SetBool("TurnRight", true); break;
            case HunterMovingState.GO_FORWARD: _animator?.SetBool("Go", true); break;
            case HunterMovingState.STOP: _animator?.SetBool("Go", false); break;
            case HunterMovingState.NO_TURN: _animator?.SetBool("TurnLeft", false); _animator?.SetBool("TurnRight", false); break;
            case HunterMovingState.ATTACK: _animator.SetTrigger("Attack"); break;
        }
    }



    protected override void Start()
    {
        StartCoroutine(IdleAnimatorChanger());       
    }

    private IEnumerator IdleAnimatorChanger()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 10f));
            _animator?.SetInteger("IdleVariant", Random.Range(1, 4));
        }   
    }
}
