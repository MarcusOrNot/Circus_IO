using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaufmoSpikeAnimator : MonoBehaviour, INeedKaufmoColor
{
    public Color Color { set { GetComponentInChildren<Renderer>().materials[0].color = value; } }

    [SerializeField] private float _maxScaleMultiplierX = 2f;
    [SerializeField] private float _minScaleMultiplierX = 0.01f;

    [SerializeField] private float _maxScaleMultiplierY = 2f;
    [SerializeField] private float _minScaleMultiplierY = 0.01f;

    [SerializeField] private float _maxScaleMultiplierZ = 2f;
    [SerializeField] private float _minScaleMultiplierZ = 0.01f;

    [SerializeField] private float _maxAnimationChangePeriod = 0.5f;
    [SerializeField] private float _minAnimationChangePeriod = 0.1f;

    [SerializeField] private float _timeBetweenAnimationFrames = 0.05f;
    [SerializeField] private int _maxAnimationFramesCount = 250;
    [SerializeField] private int _minAnimationFramesCount = 50;

    private void Start()
    {   
        StartCoroutine(SpikeAnimator());
    }

    private IEnumerator SpikeAnimator()
    {
        Vector3 baseScale = transform.localScale;
        while (true)
        {            
            Vector3 targetScale = Vector3.Scale(baseScale, new Vector3(Random.Range(_minScaleMultiplierX, _maxScaleMultiplierX), 
                Random.Range(_minScaleMultiplierY, _maxScaleMultiplierY), Random.Range(_minScaleMultiplierZ, _maxScaleMultiplierZ)));
            int animationFramesCount = Random.Range(_minAnimationFramesCount, _maxAnimationFramesCount + 1);
            transform.localScale = Vector3.Scale(baseScale, new Vector3(_minScaleMultiplierX, _minScaleMultiplierY, _minScaleMultiplierZ));
            for (int i = animationFramesCount; i > 0; i--)
            {
                transform.localScale = targetScale / i;                
                yield return new WaitForSeconds(_timeBetweenAnimationFrames);
            }
            yield return new WaitForSeconds(Random.Range(_minAnimationChangePeriod, _maxAnimationChangePeriod));
        }
    }

}
