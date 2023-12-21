using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour
{
    [SerializeField] private HatModel _model;

    public HatModel HatModel { get { return _model; } }
}
