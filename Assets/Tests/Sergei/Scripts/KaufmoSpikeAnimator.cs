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

    [SerializeField] private float _maxAnimationChangePeriod = 0.1f;
    [SerializeField] private float _minAnimationChangePeriod = 0.05f;

    [SerializeField] private int _maxAnimationFramesCount = 50;
    [SerializeField] private int _minAnimationFramesCount = 1;

    private void Start()
    {   
        StartCoroutine(SpikeAnimator());
    }

    private IEnumerator SpikeAnimator()
    {
        Vector3 baseScale = transform.localScale;
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minAnimationChangePeriod, _maxAnimationChangePeriod));
            Vector3 targetScale = new Vector3(baseScale.x * Random.Range(_minScaleMultiplierX, _maxScaleMultiplierX),
                baseScale.y * Random.Range(_minScaleMultiplierY, _maxScaleMultiplierY),
                baseScale.z * Random.Range(_minScaleMultiplierZ, _maxScaleMultiplierZ));
            transform.rotation = Random.rotation;
            int animationFramesCount = Random.Range(_minAnimationFramesCount, _maxAnimationFramesCount);
            for (int i = animationFramesCount; i > 0; i--)
            {
                transform.localScale = targetScale / i;
                yield return null;
            }
        }
    }

}
