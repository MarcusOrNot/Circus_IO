using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaufmoAttackProxy : MonoBehaviour
{

    private Hunter _hunter;

    private void Awake()
    {
        _hunter = GetComponentInParent<Hunter>();
    }



    public void SendAttackDamagingEvent()
    {
        _hunter.AttackCurrentCollidedHunter();
    }

}
