using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Entity : MonoBehaviour
{
    [SerializeField] private EntityModel _model;
    
    [SerializeField] private Renderer[] _valuableColoredComponents;        
    
    private Color[] colors = new Color[] { Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow };
    private Color color;

    private void Start()
    {
        color = colors[Random.Range(0, colors.Length)];
        foreach (Renderer valuableColoredComponent in _valuableColoredComponents)
            valuableColoredComponent.material.color = color;        
    }    
}
