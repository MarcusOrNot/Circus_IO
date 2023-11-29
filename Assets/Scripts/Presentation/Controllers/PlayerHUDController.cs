using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUDController : MonoBehaviour
{
    private Hunter _hunter;
    private void Awake()
    {
        _hunter = GetComponentInParent<Hunter>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Canvas>().worldCamera = Camera.main;
        gameObject.transform.parent = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _hunter.transform.position;
    }
}
