using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KolobokHand : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        StartCoroutine(AnimationChanger());
    }


    private IEnumerator AnimationChanger()
    {
        while (true) 
        {
            yield return new WaitForSeconds(Random.Range(3f, 10f));
            if (Random.Range(0, 2) == 0) _animator?.SetTrigger("Open"); else _animator?.SetTrigger("Like");
            yield return new WaitForSeconds(Random.Range(3f, 10f));
            _animator?.SetTrigger("Kulak");
        }
    }
}
