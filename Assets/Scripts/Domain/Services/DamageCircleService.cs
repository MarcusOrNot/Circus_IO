using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCircleService
{
    private Vector3 _circlePosition;
    private int circleSize = 10;
    public DamageCircleService(Vector3 circlePosition)
    {
        _circlePosition = circlePosition;
    }
    public bool IsOutsideCircle(Vector3 position)
    {
        return Vector3.Distance(_circlePosition, position)<circleSize;
    }
}
