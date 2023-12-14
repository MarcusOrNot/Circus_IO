using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterVisualForm : MonoBehaviour
{   

    public void SetVisiblityStatus(bool isVisible) {  foreach (Renderer visualElement in _visualElements) { visualElement.enabled = isVisible; } }

    public void SetAnimationState(HunterMovingState movingState)
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

    public void SetAnimationSpeed(float speed) { _animator?.SetFloat("Speed", speed); }



    protected List<Renderer> _visualElements = new List<Renderer>();

    

    private Animator _animator;

    protected virtual void Awake()
    {
        _visualElements.AddRange(GetComponentsInChildren<Renderer>(true)); 
        _animator = GetComponent<Animator>();
    }    



}
