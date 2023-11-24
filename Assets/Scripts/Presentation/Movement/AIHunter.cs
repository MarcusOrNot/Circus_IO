using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AIHunter : MonoBehaviour
{
    //[SerializeField] private int _warningAvoidDistance= 15;
    //[SerializeField] private int _aggressionDistance = 10;
    [Range(0, 1)] public float FearDamageZone = 0.8f;
    [Range(0, 1)] public float FearHunter = 0.7f;
    [Range(0, 1)] public float Agressive = 0.6f;
    [Range(0, 1)] public float Appetite = 0.5f;
    private Hunter _hunter;
    private List<AIBehavior> _behaviors = new List<AIBehavior>();
    //private Vector2 _aiDirection = Vector2.zero;
    private AIBehaviorModel _state = new AIBehaviorModel();
    private void Awake()
    {
        _hunter = GetComponent<Hunter>();
        //_hunter.SetOnHealthChanged((health) => { RefreshDirection(); });
    }
    // Start is called before the first frame update
    void Start()
    {
        //var damageZone = Level.Instance.GetDamageZone();
        //Собираем поведение
        _behaviors.Add(new FoodCollectBehavior(Appetite, transform));
        _behaviors.Add(new HunterFollowBehaviour(Agressive, _hunter));
        _behaviors.Add(new AvoidHunterBehaviour(FearHunter, _hunter));
        _behaviors.Add(new AvoidDamageZoneBehaviour(FearDamageZone, _hunter));

        StartCoroutine(AIControlCoroutine());
        
    }

    // Update is called once per frame
    void Update()
    {
        _hunter.Move(_state.Direction);
        if (_state.Accelerate) _hunter.Boost();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator AIControlCoroutine()
    {
        while (true)
        {
            //_aiDirection = GetResultVector();
            yield return new WaitForSeconds(0.5f);
            //_aiDirection = RefreshDirection();
            RefreshState();
        }
    }

    private void RefreshState()
    {
        //Vector2 result = Vector2.zero;
        //float weight = 0;
        var currentState = new AIBehaviorModel();
        foreach (AIBehavior behavior in _behaviors)
        {
            var newState = behavior.Update();
            if (newState.Weight > currentState.Weight * 2)
            {
                //result = behaviorModel.Direction;
                //weight = behaviorModel.Weight;
                currentState = newState;
            }
            else if (newState.Weight > currentState.Weight)
                currentState = SumStates(currentState, newState);
                //currentState = newState;
        }
        _state = currentState;
    }

    private AIBehaviorModel SumStates(AIBehaviorModel stateSource, AIBehaviorModel stateAdd)
    {
        var resState = new AIBehaviorModel();
        resState.Weight = stateAdd.Weight;
        float angle = Vector3.Angle(stateSource.Direction, stateAdd.Direction);
        //Debug.Log("Angle food is "+angle.ToString());
        if (angle < 20) 
            resState.Direction = stateSource.Direction;
        else
            resState.Direction = stateAdd.Direction;
        resState.Accelerate = stateAdd.Accelerate;
        return resState;
    }

}
